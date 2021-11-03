namespace HavocAndCry.Quoridor.Core.Models
{
    public class Move
    {
        public Move(Player player, MoveDirection moveDirection)
        {
            PlayerId = player.PlayerId;
            TurnType = TurnType.Move;
            MoveDirection = moveDirection;
            PlayerRow = player.Row;
            PlayerColumn = player.Column;

            var (row, col) = moveDirection.ToCoordinates(player);
            Row = row;
            Column = col;
        }

        public Move(Player player, Wall wall)
        {
            PlayerId = player.PlayerId;
            TurnType = TurnType.SetWall;
            Wall = wall;
        }

        public int PlayerId { get; }
        public TurnType TurnType { get; }
        public MoveDirection MoveDirection { get; }
        public Wall Wall { get; }
        public int PlayerRow { get; }
        public int PlayerColumn { get; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
