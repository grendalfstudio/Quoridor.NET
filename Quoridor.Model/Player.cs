using System;

namespace HavocAndCry.Quoridor.Model
{
    public class Player
    {
        public Player(int row, int column, int finishRow)
        {
            Row = row;
            Column = column;
            FinishRow = finishRow;
        }

        public int Row { get; private set; }
        public int Column { get; private set; }
        public int FinishRow { get; }

        public void MovePlayer(int newRow, int newColumn)
        {
            Row = newRow;
            Column = newColumn;
        }
    }
}
