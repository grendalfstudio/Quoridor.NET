using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.Core.Pathfinding
{
    public interface IPathFinder
    {
        bool IsPathToFinishExists(Player player, IGameField gameField);
        int DistanseToFinish(Player player, IGameField gameField);
    }
}