using System;
using System.Collections.Generic;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.Core.Abstract
{
    public interface ITurnService
    {
        event Action<int> OnPlayerReachedFinish;
        bool TryMove(MoveDirection direction, int playerId);
        bool TrySetWall(Wall wall);
        List<MoveDirection> GetPossibleMoves(int playerId);
    }
}