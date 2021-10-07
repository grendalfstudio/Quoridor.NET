using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavocAndCry.Quoridor.Model
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
