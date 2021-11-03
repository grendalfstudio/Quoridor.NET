using HavocAndCry.Quoridor.Bot.Abstract;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.Bot
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
                    MakeRandomPawnMove(turnService, playerId);
                    break;
                case TurnType.SetWall:
                    SetRandomWall(turnService, playerId);
                    break;
            }
            //TODO: refactor to return real move (or delete this bot completely, why not)
            return null;
        }

        private void MakeRandomPawnMove(ITurnService turnService, int playerId)
        {
            List<MoveDirection> possibleMoves = turnService.GetPossibleMoves(playerId);
            int randomIndex = _random.Next(possibleMoves.Count);
            var randomMoveDirection = possibleMoves[randomIndex];
            turnService.TryMove(randomMoveDirection, playerId);
        }

        private void SetRandomWall(ITurnService turnService, int playerId)
        {
            Wall randomWall;
            do
            {
                var randomRow = _random.Next(0, 8);
                var randomColumn = _random.Next(0, 8);

                var randomWallType = _random.NextDouble() >= 0.5 ? WallType.Horizontal : WallType.Vertical;
                randomWall = new Wall(randomWallType, new WallCenter(randomRow, randomColumn));
            } while (!turnService.TrySetWall(randomWall, playerId));
        }
    }
}
