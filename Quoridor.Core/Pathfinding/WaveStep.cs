namespace HavocAndCry.Quoridor.Core.Pathfinding
{
    public class WaveStep
    {
        public WaveStep(int row, int column, int step)
        {
            Row = row;
            Column = column;
            Step = step;
        }

        public int Row { get; }
        public int Column { get; }
        public int Step { get; }
    }
}
