using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.Core
{
    internal static class MoveDirectionExtensions
    {
        internal static (int, int) ToCoordinates(this MoveDirection direction, Player player)
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