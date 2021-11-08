using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using HavocAndCry.Quoridor.Core.Validators;

namespace HavocAndCry.Quoridor.Core.Services
{
    public class TurnService : ITurnService
    {
        private readonly IGameField _gameField;
        private readonly IPathFinder _pathFinder;
        private readonly Stack<Move> _movesHistory = new();

        public TurnService(IGameField gameField, IPathFinder pathFinder, Action<int> onPlayerReachedFinish)
        {
            _gameField = gameField;
            _pathFinder = pathFinder;
            OnPlayerReachedFinish = onPlayerReachedFinish;
        }

        public event Action<int> OnPlayerReachedFinish;

        public IReadOnlyList<Player> Players => _gameField.Players;

        public IReadOnlyList<Wall> Walls => _gameField.Walls;

        public bool TryMove(Position position, int playerId, bool isRealMove = true)
        {
            var player = Players.First(p => p.PlayerId == playerId);
            
            if (!TurnValidator.IsMoveValid(_gameField, position.Row, position.Column, player))
                return false;

            player.MovePlayer(position.Row, position.Column);
            
            if (isRealMove && CheckWinCondition(player))
                OnPlayerReachedFinish?.Invoke(player.PlayerId);
            
            return true;
        }

        public bool TrySetWall(Wall wall, int playerId, bool fromInput = false)
        {
            if (!IsWallValid(wall, playerId, fromInput))
                return false;

            _gameField.AddWall(wall);
            _gameField.Players[playerId-1].SetWall();
            return true;
        }

        public bool MakeMove(Move move, bool isRealMove = true)
        {
            if (move == null)
            {
                return false;
            }

            bool moveSucceeded = false;
            switch (move.TurnType)
            {
                case TurnType.Move:
                    moveSucceeded = TryMove(move.Position, move.PlayerId, isRealMove);
                    break;
                case TurnType.SetWall:
                    moveSucceeded = TrySetWall(move.Wall, move.PlayerId);
                    break;
            }
            
            if (moveSucceeded)
                _movesHistory.Push(move);
            
            return moveSucceeded;
        }

        public void UndoLastMove()
        {
            if (!_movesHistory.Any()) return;

            Move lastMove = _movesHistory.Pop();
            switch (lastMove.TurnType)
            {
                case TurnType.Move:
                    var player = Players.First(p => p.PlayerId == lastMove.PlayerId);
                    player.MovePlayer(lastMove.PlayerRow, lastMove.PlayerColumn);
                    break;
                case TurnType.SetWall:
                    _gameField.RemoveWall(lastMove.Wall);
                    Players[lastMove.PlayerId - 1].RemoveWall();
                    break;
            }
        }

        public bool IsWallValid(Wall wall, int playerId, bool fromInput = false)
        {
            if (!TurnValidator.IsWallValid(_gameField, wall, playerId, fromInput))
            {
                return false;
            }
            
            _gameField.AddWall(wall);
            Players[playerId-1].SetWall();

            bool isPathDoesntExist = Players.Any(p => !_pathFinder.IsPathToFinishExists(p, _gameField));
            
            _gameField.RemoveWall(wall);
            Players[playerId - 1].RemoveWall();

            if (isPathDoesntExist)
            {
                if(fromInput)
                    File.AppendAllText(@"./log.jsonc", $"//[{DateTime.Now}] Wall {wall} from player {playerId} is not valid, because it blocks way for at least one player\n\n");
                
                return false;
            }

            return true;
        }

        public List<Position> GetPossibleMoves(int playerId)
        {
            List<Position> directions = new List<Position>();
            var player = Players.First(p => p.PlayerId == playerId);
            for (int i = 1; i <= 8; i++)
            {
                var coordinates = ((MoveDirection)i).ToCoordinates(player);
                coordinates = UpdateCoordinatesIfJump(coordinates, player);
                if (TurnValidator.IsMoveValid(_gameField, coordinates.Item1, coordinates.Item2, player))
                {
                    directions.Add(new Position(coordinates.Item1, coordinates.Item2));
                }
            }

            return directions;
        }

        private bool CheckWinCondition(Player player)
        {
            if (player.Row == player.FinishRow)
            {
                return true;
            }

            return false;
        }

        private (int, int) UpdateCoordinatesIfJump((int, int) coordinates, Player player)
        {
            if (Players.Any(p => p.Row == coordinates.Item1 && p.Column == coordinates.Item2))
            {
                var newRow = player.Row - (player.Row - coordinates.Item1) * 2;
                var newColumn = player.Column - (player.Column - coordinates.Item2) * 2;
                coordinates = (newRow, newColumn);
            }

            return coordinates;
        }

        public int EvaluatePosition(int playerId)
        {
            Player currentPlayer = Players.First(p => p.PlayerId == playerId);
            Player oppositePlayer = Players.First(p => p.PlayerId == playerId % 2 + 1);
            
            if (CheckWinCondition(currentPlayer))
                return int.MaxValue;
            if (CheckWinCondition(oppositePlayer))
                return int.MinValue;
            
            int currentPlayerDistance = _pathFinder.DistanceToFinish(currentPlayer, _gameField);
            int oppositePlayerDistance = _pathFinder.DistanceToFinish(oppositePlayer, _gameField);
            return oppositePlayerDistance - currentPlayerDistance;
        }
    }
}