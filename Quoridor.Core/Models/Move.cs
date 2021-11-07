namespace HavocAndCry.Quoridor.Core.Models
{
    public class Move
    {
        public Move(Player player, Position position)
        {
            PlayerId = player.PlayerId;
            TurnType = TurnType.Move;
            PlayerRow = player.Row;
            PlayerColumn = player.Column;
            Position = position;
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
        
        public Position Position { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
