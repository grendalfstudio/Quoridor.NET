using HavocAndCry.Quoridor.Core.Abstract;
using Quoridor.Bot.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using System.Diagnostics;

namespace Quoridor.Bot
{
    public class MinimaxBasedBot : IBot
    {
        private const int MaxDepth = 2;
        private const int MaxWallsRange = 3;

        public Move MakeMove(ITurnService turnService, int playerId)
        {
            var timer = new Stopwatch();
            timer.Start();
            int bestScore = int.MinValue;
            Move bestMove = null;
            foreach (Move possibleMove in GetPossibleMoves(turnService, playerId))
            {
                turnService.MakeMove(possibleMove, false);
                int score = -Minimax(turnService, MaxDepth - 1, playerId % 2 + 1);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = possibleMove;
                }
                turnService.UndoLastMove();
            }

            if (bestMove == null)
            {
                //Console.WriteLine("BestMove is null.\n Press any key ...");
                //Console.ReadKey();
                return null;
            }

            var moveSucceeded = turnService.MakeMove(bestMove);
            
            var time = timer.ElapsedMilliseconds;
            Console.WriteLine($"time for move: {time} ms");
            Console.ReadKey();

            return moveSucceeded;
        }
        
        private int Minimax(ITurnService turnService, int depth, int playerId)
        {
            if (depth <= 0)
            {
                return turnService.EvaluatePosition(playerId);
            }

            int bestScore = int.MinValue;
            foreach (Move possibleMove in GetPossibleMoves(turnService, playerId))
            {
                if (bestScore == int.MaxValue)
                    return bestScore;
                turnService.MakeMove(possibleMove, false);
                bestScore = Math.Max(bestScore, -Minimax(turnService,depth - 1, playerId % 2 + 1));
                turnService.UndoLastMove();
            }

            return bestScore;
        }

        private IEnumerable<Move> GetPossibleMoves(ITurnService turnService, int playerId)
        {
            Player currentPlayer = turnService.Players.First(p => p.PlayerId == playerId);

            List<Move> possibleMoves = new List<Move>();
            possibleMoves.AddRange(turnService.GetPossibleMoves(playerId)
                .Select(movePosition => new Move(currentPlayer, movePosition)));
            possibleMoves.AddRange(GetPossibleWallsToSetInRange(turnService, playerId)
                .Select(wall => new Move(currentPlayer, wall)));
            return possibleMoves;
        }
        
        private IEnumerable<Wall> GetPossibleWallsToSetInRange(ITurnService turnService, int playerId)
        {
            List<Wall> possibleWallsToSet = new List<Wall>();
            Player currentPlayer = turnService.Players.First(p => p.PlayerId == playerId);

            var rowMinValue = (currentPlayer.Row - MaxWallsRange) >= 0 
                ? (currentPlayer.Row - MaxWallsRange) : 0;
            var rowMaxValue = (currentPlayer.Row + MaxWallsRange) <= 7 
                ? (currentPlayer.Row + MaxWallsRange) : 7;
            var columnMinValue = (currentPlayer.Column - MaxWallsRange) >= 0 
                ? (currentPlayer.Column - MaxWallsRange) : 0;
            var columnMaxValue = (currentPlayer.Column + MaxWallsRange) >= 7 
                ? (currentPlayer.Column + MaxWallsRange) : 7;

            for (int i = rowMinValue; i <= rowMaxValue; i++)
            {
                for (int j = columnMinValue; j <= rowMaxValue; j++)
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
