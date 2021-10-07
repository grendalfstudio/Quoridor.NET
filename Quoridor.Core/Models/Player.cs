using System;

namespace HavocAndCry.Quoridor.Core.Models
{
    public class Player
    {
        public Player(int playerId, int row, int column, int finishRow)
        {
            PlayerId = playerId;
            Row = row;
            Column = column;
            FinishRow = finishRow;
            WallsCount = 10;
        }
        public int PlayerId { get; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public int FinishRow { get; }
        public int WallsCount {get; private set; }

        public void MovePlayer(int newRow, int newColumn)
        {
            Row = newRow;
            Column = newColumn;
        }

        public void SetWall()
        {
            --WallsCount;
        }
    }
}
