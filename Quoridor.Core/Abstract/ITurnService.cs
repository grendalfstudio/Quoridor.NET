using System;
using System.Collections.Generic;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.Core.Abstract
{
    public interface ITurnService
    {
        event Action<int> OnPlayerReachedFinish;
        IReadOnlyList<Player> Players { get; }
        IReadOnlyList<Wall> Walls { get; }
        bool TryMove(MoveDirection direction, int playerId);
        bool TrySetWall(Wall wall, int playerId);
        void MakeMove(Move move);
        void UndoLastMove();
        bool IsWallValid(Wall wall, int playerId);
        int EvaluatePosition(int playerId);
        List<MoveDirection> GetPossibleMoves(int playerId);
    }
}