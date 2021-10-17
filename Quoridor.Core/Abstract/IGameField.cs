

using System;
using System.Collections.Generic;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.Core.Abstract
{
    public interface IGameField : ICloneable
    {
        int Size { get; }
        IReadOnlyList<Wall> Walls { get; }
        IReadOnlyList<Player> Players { get; }
        void AddWall(Wall wall);
        bool IsWallAt(WallCenter wallCenter);
        bool IsWallAt(WallCenter wallCenter, WallType wallType);
    }
}