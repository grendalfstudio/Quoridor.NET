using System;

namespace Model
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