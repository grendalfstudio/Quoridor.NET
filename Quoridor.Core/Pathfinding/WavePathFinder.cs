using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.Core.Pathfinding
{
    // Path finder that uses wave (Lee) algorithm to find a path
    public class WavePathFinder : IPathFinder
    {
        public bool IsPathToFinishExists(Player player, GameField gameField)
        {
            int[,] labyrinth = new int[gameField.Size, gameField.Size];
            Queue<WaveStep> queue = new Queue<WaveStep>();
            queue.Enqueue(new WaveStep(player.Row, player.Column, 1));

            while (queue.Count > 0)
            {
                WaveStep waveStep = queue.Dequeue();

                if (waveStep.Row < 0 || waveStep.Row >= gameField.Size || waveStep.Column < 0 || waveStep.Column >= gameField.Size)
                {
                    continue;
                }
                else if (labyrinth[waveStep.Row, waveStep.Column] > 0)
                {
                    continue;
                }

                labyrinth[waveStep.Row, waveStep.Column] = waveStep.Step;
                if (waveStep.Row == player.FinishRow)
                {
                    break;
                }

                var northWestWallPoint = new WallCenter(waveStep.Row - 1, waveStep.Column - 1);
                var northEastWallPoint = new WallCenter(waveStep.Row - 1, waveStep.Column);
                var southWestWallPoint = new WallCenter(waveStep.Row, waveStep.Column - 1);
                var southEastWallPoint = new WallCenter(waveStep.Row, waveStep.Column);

                if (!gameField.IsHorizontalWallAt(northWestWallPoint)
                    && !gameField.IsHorizontalWallAt(northEastWallPoint))
                {
                    queue.Enqueue(new WaveStep(waveStep.Row - 1, waveStep.Column, waveStep.Step + 1));
                }
                if (!gameField.IsHorizontalWallAt(southWestWallPoint)
                    && !gameField.IsHorizontalWallAt(southEastWallPoint))
                {
                    queue.Enqueue(new WaveStep(waveStep.Row + 1, waveStep.Column, waveStep.Step + 1));
                }
                if (!gameField.IsVerticalWallAt(northWestWallPoint)
                    && !gameField.IsVerticalWallAt(southWestWallPoint))
                {
                    queue.Enqueue(new WaveStep(waveStep.Row, waveStep.Column - 1, waveStep.Step + 1));
                }
                if (!gameField.IsVerticalWallAt(northEastWallPoint)
                    && !gameField.IsVerticalWallAt(southEastWallPoint))
                {
                    queue.Enqueue(new WaveStep(waveStep.Row, waveStep.Column + 1, waveStep.Step + 1));
                }
            }

            for (int i = 0; i < gameField.Size; i++)
            {
                if (labyrinth[player.FinishRow, i] > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
