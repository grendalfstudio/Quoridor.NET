using System.Linq;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;

namespace HavocAndCry.Quoridor.Model.Validators
{
    public static class TurnValidator
    {
        public static bool IsMoveValid(IGameField gameField, int row, int column, int playerId)
        {
            if (gameField.Players.Any(p => p.Row == row && p.Column == column))
            {
                return false;
            }

            if (row < 0 || row > gameField.Size - 1 || column < 0 || column > gameField.Size - 1)
            {
                return false;
            }
            var player = gameField.Players.First(p => p.PlayerId == playerId);
            if (player.Row != row && player.Column != column || player.Row - row == 2 || player.Column - column == 2)
            {
                return IsSpecialMoveValid(gameField, row, column, player);
            }

            return IsStandartMoveValid(gameField, row, column, player);
        }

        private static bool IsStandartMoveValid(IGameField gameField, int row, int column, Player player)
        {
            if (player.Row == row && player.Column > column)
            {
                return !(gameField.IsWallAt(new WallCenter(row - 1, column - 1), WallType.Vertical) ||
                           gameField.IsWallAt(new WallCenter(row, column - 1), WallType.Vertical));
            }
             
            if (player.Row == row && player.Column < column)
            {
                return !(gameField.IsWallAt(new WallCenter(row - 1, column), WallType.Vertical) ||
                         gameField.IsWallAt(new WallCenter(row, column), WallType.Vertical));
            }

            if (player.Column == column && player.Row > row)
            {
                return !(gameField.IsWallAt(new WallCenter(row, column - 1), WallType.Horizontal) ||
                         gameField.IsWallAt(new WallCenter(row, column), WallType.Horizontal));
            }
            
            if (player.Column == column && player.Row < row)
            {
                return !(gameField.IsWallAt(new WallCenter(row - 1, column - 1), WallType.Horizontal) ||
                         gameField.IsWallAt(new WallCenter(row - 1, column), WallType.Horizontal));
            }

            return false;
        }

        private static bool IsSpecialMoveValid(IGameField gameField, int row, int column, Player player)
        {
            return false;
        }

        public static bool IsWallValid(IGameField gameField, Wall newWall, int playerId, IPathFinder pathFinder)
        {
            if (gameField.Players.First(p => p.PlayerId == playerId).WallsCount <= 0)
            {
                return false;
            }

            if (gameField.IsWallAt(newWall.WallCenter))
            {
                return false;
            }

            if (gameField.Players.Any(p => !pathFinder.IsPathToFinishExists(p, gameField)))
            {
                return false;
            }

            return true;
        }
    }
}