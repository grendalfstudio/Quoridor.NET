using System;
using System.Collections.Generic;
using System.Linq;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using HavocAndCry.Quoridor.Model.Validators;

namespace HavocAndCry.Quoridor.Model.Services
{
    public class TurnService : ITurnService
    {
        private readonly IGameField _gameField;
        private readonly IPathFinder _pathFinder;
        private Stack<Move> _movesHistory = new Stack<Move>();

        public TurnService(IGameField gameField, IPathFinder pathFinder, Action<int> onPlayerReachedFinish)
        {
            _gameField = gameField;
            _pathFinder = pathFinder;
            OnPlayerReachedFinish = onPlayerReachedFinish;
        }

        public event Action<int> OnPlayerReachedFinish;

        public IReadOnlyList<Player> Players => _gameField.Players;

        public IReadOnlyList<Wall> Walls => _gameField.Walls;

        public bool TryMove(MoveDirection direction, int playerId)
        {
            var player = _gameField.Players.First(p => p.PlayerId == playerId);
            var coordinates = GetCoordinateFromDirection(direction, player);
            coordinates = UpdateCoordinatesIfJump(coordinates, player);
            
            if (!TurnValidator.IsMoveValid(_gameField, coordinates.Item1, coordinates.Item2, player))
                return false;

            player.MovePlayer(coordinates.Item1, coordinates.Item2);
            CheckWinCondition(player);
            return true;
        }

        public bool TrySetWall(Wall wall, int playerId)
        {
            if (!TurnValidator.IsWallValid(_gameField, wall, playerId, _pathFinder))
                return false;

            _gameField.AddWall(wall);
            _gameField.Players[playerId-1].SetWall();
            return true;
        }

        public void MakeMove(Move move)
        {
            if (move == null)
            {
                Console.WriteLine("Move is null.\n Press any key ...");
                Console.ReadKey();
                return;
            }

            switch (move.TurnType)
            {
                case TurnType.Move:
                    Player player = _gameField.Players.First(p => p.PlayerId == move.PlayerId);
                    var coordinates = GetCoordinateFromDirection(move.MoveDirection, player);
                    coordinates = UpdateCoordinatesIfJump(coordinates, player);

                    player.MovePlayer(coordinates.Item1, coordinates.Item2);
                    break;
                case TurnType.SetWall:
                    _gameField.AddWall(move.Wall);
                    _gameField.Players[move.PlayerId - 1].SetWall();
                    break;
            }

            _movesHistory.Push(move);
        }

        public void UndoLastMove()
        {
            if (!_movesHistory.Any()) return;

            Move lastMove = _movesHistory.Pop();
            switch (lastMove.TurnType)
            {
                case TurnType.Move:
                    var player = _gameField.Players.First(p => p.PlayerId == lastMove.PlayerId);
                    player.MovePlayer(lastMove.PlayerRow, lastMove.PlayerColumn);
                    break;
                case TurnType.SetWall:
                    _gameField.RemoveWall(lastMove.Wall);
                    _gameField.Players[lastMove.PlayerId - 1].RemoveWall();
                    break;
            }
        }

        public bool IsWallValid(Wall wall, int playerId)
        {
            return TurnValidator.IsWallValid(_gameField, wall, playerId, _pathFinder);
        }

        public List<MoveDirection> GetPossibleMoves(int playerId)
        {
            List<MoveDirection> directions = new List<MoveDirection>();
            var player = _gameField.Players.First(p => p.PlayerId == playerId);
            for (int i = 1; i <= 8; i++)
            {
                var coordinates = GetCoordinateFromDirection((MoveDirection)i, player);
                coordinates = UpdateCoordinatesIfJump(coordinates, player);
                if (TurnValidator.IsMoveValid(_gameField, coordinates.Item1, coordinates.Item2, player))
                {
                    directions.Add((MoveDirection)i);
                }
            }

            return directions;
        }

        private void CheckWinCondition(Player player)
        {
            if (player.Row == player.FinishRow)
            {
                OnPlayerReachedFinish?.Invoke(player.PlayerId);
            }
        }

        private Tuple<int, int> GetCoordinateFromDirection(MoveDirection direction, Player player)
        {
            int row = 0;
            int column = 0;
            switch (direction)
            {
                case MoveDirection.Up:
                    row = player.Row - 1;
                    column = player.Column;
                    break;
                case MoveDirection.Down:
                    row = player.Row + 1;
                    column = player.Column;
                    break;
                case MoveDirection.Left:
                    row = player.Row;
                    column = player.Column - 1;
                    break;
                case MoveDirection.Right:
                    row = player.Row;
                    column = player.Column + 1;
                    break;
                case MoveDirection.UpLeft:
                    row = player.Row - 1;
                    column = player.Column - 1;
                    break;
                case MoveDirection.UpRight:
                    row = player.Row - 1;
                    column = player.Column + 1;
                    break;
                case MoveDirection.DownLeft:
                    row = player.Row + 1;
                    column = player.Column - 1;
                    break;
                case MoveDirection.DownRight:
                    row = player.Row + 1;
                    column = player.Column + 1;
                    break;
                default:
                    break;
            }
            return new Tuple<int, int>(row, column);
        }

        private Tuple<int, int> UpdateCoordinatesIfJump(Tuple<int, int> coordinates, Player player)
        {
            if (_gameField.Players.Any(p => p.Row == coordinates.Item1 && p.Column == coordinates.Item2))
            {
                var newRow = player.Row - (player.Row - coordinates.Item1) * 2;
                var newColumn = player.Column - (player.Column - coordinates.Item2) * 2;
                coordinates = new Tuple<int, int>(newRow, newColumn);
            }

            return coordinates;
        }

        public int EvaluatePosition(int playerId)
        {
            Player currentPlayer = _gameField.Players.First(p => p.PlayerId == playerId);
            Player oppositePlayer = _gameField.Players.First(p => p.PlayerId == playerId % 2 + 1);
            int currentPlayerDistance = _pathFinder.DistanseToFinish(currentPlayer, _gameField);
            int oppositePlayerDistance = _pathFinder.DistanseToFinish(oppositePlayer, _gameField);
            return oppositePlayerDistance - currentPlayerDistance;
        }
    }
}