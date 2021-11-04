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

        public override string ToString()
        {
            return $"Row: {NorthRow}; Column: {WestColumn}";
        }
    }
}