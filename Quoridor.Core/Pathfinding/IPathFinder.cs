using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.Core.Pathfinding
{
    public interface IPathFinder
    {
        bool IsPathToFinishExists(Player player, GameField gameField);
    }
}