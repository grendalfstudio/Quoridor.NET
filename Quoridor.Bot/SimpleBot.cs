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

        public MoveDirection RequestMoveDirection(List<MoveDirection> possibleDirections)
        {
            int randomIndex = _random.Next(possibleDirections.Count);
            return possibleDirections[randomIndex];
        }

        public TurnType RequestTurn(IGameField gameField, int playerId)
        {
            Player currentPlayer = gameField.Players.Where(p => p.PlayerId == playerId).First();
            return currentPlayer.WallsCount > 0
                ? (TurnType)_random.Next(1, 3)
                : TurnType.Move;
        }

        public void SetRandomWall(ITurnService turnService, int playerId)
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
