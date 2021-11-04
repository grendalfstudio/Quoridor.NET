using HavocAndCry.Quoridor.Core.Abstract;
using Quoridor.Bot.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HavocAndCry.Quoridor.Model;
using System.Xml.Linq;

namespace Quoridor.Bot
{
    public class MinimaxBasedBot : IBot
    {
        private const int MaxDepth = 2;

        public void MakeMove(ITurnService turnService, int playerId)
        {
            int bestScore = int.MinValue;
            Move bestMove = null;
            foreach (Move possibleMove in GetPossibleMoves(turnService, playerId))
            {
                turnService.MakeMove(possibleMove);
                int score = -Minimax(MaxDepth - 1, playerId % 2 + 1);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = possibleMove;
                }
                turnService.UndoLastMove();
            }

            if (bestMove == null)
            {
                return;
            }
            switch (bestMove.TurnType)
            {
                case TurnType.Move:
                    turnService.TryMove(bestMove.MoveDirection, bestMove.PlayerId);
                    break;
                case TurnType.SetWall:
                    turnService.TrySetWall(bestMove.Wall, bestMove.PlayerId);
                    break;
            }

            int Minimax(int depth, int playerId)
            {
                if (depth <= 0)
                {
                    return turnService.EvaluatePosition(playerId);
                }

                int bestScore = int.MinValue;
                foreach (Move possibleMove in GetPossibleMoves(turnService, playerId))
                {
                    turnService.MakeMove(possibleMove);
                    bestScore = Math.Max(bestScore, -Minimax(depth - 1, playerId % 2 + 1));
                    turnService.UndoLastMove();
                }

                return bestScore;
            }
        }

        private IEnumerable<Move> GetPossibleMoves(ITurnService turnService, int playerId)
        {
            Player currentPlayer = turnService.Players.First(p => p.PlayerId == playerId);

            List<Move> possibleMoves = new List<Move>();
            possibleMoves.AddRange(turnService.GetPossibleMoves(playerId)
                .Select(moveDirection => new Move(currentPlayer, moveDirection)));
            possibleMoves.AddRange(GetPosssibleWallsToSet(turnService, playerId)
                .Select(wall => new Move(currentPlayer, wall)));
            return possibleMoves;
        }

        private IEnumerable<Wall> GetPosssibleWallsToSet(ITurnService turnService, int playerId)
        {
            List<Wall> possibleWallsToSet = new List<Wall>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Wall horizontalWall = new Wall(WallType.Horizontal, new WallCenter(i, j));
                    Wall verticalWall = new Wall(WallType.Vertical, new WallCenter(i, j));
                    if (turnService.IsWallValid(horizontalWall, playerId))
                    {
                        possibleWallsToSet.Add(horizontalWall);
                    }
                    if (turnService.IsWallValid(verticalWall, playerId))
                    {
                        possibleWallsToSet.Add(verticalWall);
                    }
                }
            }
            return possibleWallsToSet;
        }
    }
}
