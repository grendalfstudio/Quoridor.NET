﻿

using System.Collections.Generic;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.Core.Abstract
{
    public interface IGameField
    {
        int Size { get; }
        IReadOnlyList<Wall> Walls { get; }
        IReadOnlyList<Player> Players { get; }
        bool IsWallAt(WallCenter wallCenter);
        bool IsWallAt(WallCenter wallCenter, WallType wallType);
    }
}