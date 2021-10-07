

using System.Collections.Generic;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.Core.Abstract
{
    public interface IGameField
    {
        IReadOnlyList<Wall> Walls { get; }
        IReadOnlyList<Player> Players { get; }
        bool IsWallAt(WallCenter wallCenter);
        bool IsHorizontalWallAt(WallCenter wallCenter);
        bool IsVerticalWallAt(WallCenter wallCenter);
    }
}