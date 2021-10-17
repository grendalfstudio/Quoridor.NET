using HavocAndCry.Quoridor.ConsoleClient.Abstract;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using HavocAndCry.Quoridor.Model.Services;
using HavocAndCry.Quoridor.Model.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavocAndCry.Quoridor.ConsoleClient.Models
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

        public TurnMenuOptions RequestTurn(IGameField gameField, int playerId)
        {
            Player currentPlayer = gameField.Players.Where(p => p.PlayerId == playerId).First();
            return currentPlayer.WallsCount > 0
                ? (TurnMenuOptions)_random.Next(1, 3)
                : TurnMenuOptions.Move;
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
