using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Bot
{
    public class Move
    {
        public Move(Player player, MoveDirection moveDirection)
        {
            PlayerId = player.PlayerId;
            TurnType = TurnType.Move;
            (Row, Column) = GetCoordinateFromDirection(moveDirection, player);
            WallType = null;
        }

        public Move(Player player, Wall wall)
        {
            PlayerId = player.PlayerId;
            TurnType = TurnType.SetWall;
            Row = wall.WallCenter.NorthRow;
            Column = wall.WallCenter.WestColumn;
            WallType = wall.Type;
        }

        public int PlayerId { get; }
        public TurnType TurnType { get; }
        public int Row { get; }
        public int Column { get; }
        public WallType? WallType { get; }

        private (int Row, int Column) GetCoordinateFromDirection(MoveDirection direction, Player player)
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
            return (row, column);
        }
    }
}
