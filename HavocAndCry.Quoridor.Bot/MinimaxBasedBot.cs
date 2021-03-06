using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using HavocAndCry.Quoridor.Bot.Abstract;

namespace HavocAndCry.Quoridor.Bot
{
    public class MinimaxBasedBot : IBot
    {
        private const int MaxDepth = 2;
        private const int MaxWallsRange = 4;

        public Move MakeMove(ITurnService turnService, int playerId)
        {
            var timer = new Stopwatch();
            timer.Start();
            int bestScore = int.MinValue;
            Move bestMove = null;
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            foreach (Move possibleMove in GetPossibleMoves(turnService, playerId))
            {
                bool moveSuccess = turnService.MakeMove(possibleMove, false);
                if (!moveSuccess)
                {
                    continue;
                }
                int score = Minimax(turnService, MaxDepth - 1, playerId % 2 + 1, alpha, beta, false);
                turnService.UndoLastMove();
                
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = possibleMove;
                }
                else if (bestMove == null && score == bestScore)
                {
                    bestMove = possibleMove;
                    File.AppendAllText(@"./log.jsonc", $"\n//[{DateTime.Now}] All moves lead to opponent's victory. We can't do anything... \n");

                }

                alpha = Math.Max(alpha, bestScore);
            }

            if (bestMove == null)
            {
                return null;
            }

            bool moveSucceeded = turnService.MakeMove(bestMove);
            
            var time = timer.ElapsedMilliseconds;
            
            if (time > 2000)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"//[{DateTime.Now}] Bad bot speed = {time}");
                File.AppendAllText(@"./log.jsonc", sb.ToString());
            }

            return bestMove;
        }
        
        private int Minimax(ITurnService turnService, int depth, int playerId, int alpha, int beta, bool isBot)
        {
            if (depth <= 0)
            {
                int id = isBot ? playerId : playerId % 2 + 1;
                return turnService.EvaluatePosition(id);
            }

            int bestScore = isBot ? int.MinValue : int.MaxValue;
            foreach (Move possibleMove in GetPossibleMoves(turnService, playerId))
            {
                if ((isBot && bestScore == int.MaxValue) ||
                    (!isBot && bestScore == int.MinValue))
                    return bestScore;
                
                bool moveSuccess = turnService.MakeMove(possibleMove, false);
                if (!moveSuccess)
                {
                    continue;
                }
                int score = Minimax(turnService, depth - 1, playerId % 2 + 1, alpha, beta, !isBot);
                turnService.UndoLastMove();

                if (isBot)
                {
                    if (score > bestScore)
                    {
                        bestScore = score;
                    }

                    alpha = Math.Max(alpha, bestScore);
                }
                else
                {
                    if (score <= bestScore)
                    {
                        bestScore = score;
                    }

                    beta = Math.Min(beta, bestScore);
                }

                if (alpha > beta)
                {
                    break;
                }
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
                    
                    possibleWallsToSet.Add(horizontalWall);
                    possibleWallsToSet.Add(verticalWall);
                }
            }
            return possibleWallsToSet;
        }
    }
}
