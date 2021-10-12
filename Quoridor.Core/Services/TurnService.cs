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

        public TurnService(IGameField gameField, IPathFinder pathFinder, Action<int> onPlayerReachedFinish)
        {
            _gameField = gameField;
            _pathFinder = pathFinder;
            OnPlayerReachedFinish = onPlayerReachedFinish;
        }
        
        public event Action<int> OnPlayerReachedFinish;
        
        public bool TryMove(MoveDirection direction, int playerId)
        {
            var player = _gameField.Players.First(p => p.PlayerId == playerId);
            var coordinates = GetCoordinateFromDirection(direction, player);
            
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
            return true;
        }

        public List<MoveDirection> GetPossibleMoves(int playerId)
        {
            List<MoveDirection> directions = new List<MoveDirection>();
            var player = _gameField.Players.First(p => p.PlayerId == playerId);
            for (int i = 0; i < 8; i++)
            {
                var coordinates = GetCoordinateFromDirection((MoveDirection)i, player);
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
                    row = player.Row + 1;
                    column = player.Column;
                    break;
                case MoveDirection.Down:
                    row = player.Row - 1;
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
                    row = player.Row + 1;
                    column = player.Column - 1;
                    break;
                case MoveDirection.UpRight:
                    row = player.Row + 1;
                    column = player.Column + 1;
                    break;
                case MoveDirection.DownLeft:
                    row = player.Row - 1;
                    column = player.Column - 1;
                    break;
                case MoveDirection.DownRight:
                    row = player.Row - 1;
                    column = player.Column + 1;
                    break;
                default:
                    break;
            }
            return new Tuple<int, int>(row, column);
        }
    }
}