using System;

namespace HavocAndCry.Quoridor.Core.Models
{
    public struct WallCenter
    {
        public WallCenter(int northRow, int westColumn)
        {
            NorthRow = northRow;
            WestColumn = westColumn;
        }

        public int NorthRow { get; }
        public int WestColumn { get; }
    }
}