using System;
using System.Collections.Generic;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.Core.Pathfinding
{
    public class AStarPathFinder : IPathFinder
    {
        public bool IsPathToFinishExists(Player player, IGameField gameField)
        {
            if (DistanceToFinish(player, gameField) > 0)
            {
                return true;
            }
            
            return false;
        }

        public int DistanceToFinish(Player player, IGameField gameField)
        {
            var labyrinth = new int[gameField.Size, gameField.Size];
            var queue = new PriorityQueue<WaveStep, int>();
            
            var startWaveStep = new WaveStep(player.Row, player.Column, 1);
            int startPathLength = startWaveStep.Step + Math.Abs(startWaveStep.Row - player.FinishRow);
            queue.Enqueue(startWaveStep, startPathLength);

            while (queue.Count > 0)
            {
                WaveStep waveStep = queue.Dequeue();

                if (waveStep.Row < 0 || waveStep.Row >= gameField.Size || waveStep.Column < 0 || waveStep.Column >= gameField.Size)
                {
                    continue;
                }
                
                if (labyrinth[waveStep.Row, waveStep.Column] > 0)
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

                if (!gameField.IsWallAt(northWestWallPoint, WallType.Horizontal)
                    && !gameField.IsWallAt(northEastWallPoint, WallType.Horizontal))
                {
                    var nextWaveStep = new WaveStep(waveStep.Row - 1, waveStep.Column, waveStep.Step + 1);
                    int pathLength = nextWaveStep.Step + Math.Abs(nextWaveStep.Row - player.FinishRow);
                    queue.Enqueue(nextWaveStep, pathLength);
                }
                if (!gameField.IsWallAt(southWestWallPoint, WallType.Horizontal)
                    && !gameField.IsWallAt(southEastWallPoint, WallType.Horizontal))
                {
                    var nextWaveStep = new WaveStep(waveStep.Row + 1, waveStep.Column, waveStep.Step + 1);
                    int pathLength = nextWaveStep.Step + Math.Abs(nextWaveStep.Row - player.FinishRow);
                    queue.Enqueue(nextWaveStep, pathLength);
                }
                if (!gameField.IsWallAt(northWestWallPoint, WallType.Vertical)
                    && !gameField.IsWallAt(southWestWallPoint, WallType.Vertical))
                {
                    var nextWaveStep = new WaveStep(waveStep.Row, waveStep.Column - 1, waveStep.Step + 1);
                    int pathLength = nextWaveStep.Step + Math.Abs(nextWaveStep.Row - player.FinishRow);
                    queue.Enqueue(nextWaveStep, pathLength);
                }
                if (!gameField.IsWallAt(northEastWallPoint, WallType.Vertical)
                    && !gameField.IsWallAt(southEastWallPoint, WallType.Vertical))
                {
                    var nextWaveStep = new WaveStep(waveStep.Row, waveStep.Column + 1, waveStep.Step + 1);
                    int pathLength = nextWaveStep.Step + Math.Abs(nextWaveStep.Row - player.FinishRow);
                    queue.Enqueue(nextWaveStep, pathLength);
                }
            }
            
            for (int i = 0; i < gameField.Size; i++)
            {
                if (labyrinth[player.FinishRow, i] > 0)
                {
                    return labyrinth[player.FinishRow, i];
                }
            }
            return -1;
        }
    }
}