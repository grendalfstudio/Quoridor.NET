using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Model.Services;
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

        public void MakeMove(ITurnService turnService, int playerId)
        {
            Player currentPlayer = turnService.Players.Where(p => p.PlayerId == playerId).First();
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
        }
    }
}
