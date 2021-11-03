using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using Quoridor.Bot.Abstract;

namespace Quoridor.Bot
{
    public class SimpleBot : IBot
    {
        private Random _random;

        public SimpleBot()
        {
            _random = new Random();
        }

        public Move MakeMove(ITurnService turnService, int playerId)
        {
            Player currentPlayer = turnService.Players.First(p => p.PlayerId == playerId);
            TurnType turnType = currentPlayer.WallsCount > 0
                ? (TurnType)_random.Next(1, 3)
                : TurnType.Move;

            switch (turnType)
            {
                case TurnType.Move:
                    return MakeRandomPawnMove(turnService, playerId);
                case TurnType.SetWall:
                    return SetRandomWall(turnService, playerId);
                default:
                    return null;
            }
        }

        private Move MakeRandomPawnMove(ITurnService turnService, int playerId)
        {
            List<Position> possibleMoves = turnService.GetPossibleMoves(playerId);
            int randomIndex = _random.Next(possibleMoves.Count);
            var randomMoveDirection = possibleMoves[randomIndex];
            
            var move = new Move(turnService.Players.First(p => p.PlayerId == playerId), randomMoveDirection);
            var moveSucceeded = turnService.MakeMove(move);
            
            if (!moveSucceeded)
                return null;
            return move;
        }

        private Move SetRandomWall(ITurnService turnService, int playerId)
        {
            int randomRow;
            int randomColumn;
            WallType randomWallType;
            Wall randomWall;
            do
            {
                randomRow = _random.Next(0, 8);
                randomColumn = _random.Next(0, 8);

                randomWallType = _random.NextDouble() >= 0.5 ? WallType.Horizontal : WallType.Vertical;
                randomWall = new Wall(randomWallType, new WallCenter(randomRow, randomColumn));
            } while (!turnService.TrySetWall(randomWall, playerId));
            return new Move(turnService.Players.First(p => p.PlayerId == playerId), randomWall);
        }
    }
}
