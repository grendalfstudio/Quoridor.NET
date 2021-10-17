using HavocAndCry.Quoridor.ConsoleClient.Models;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavocAndCry.Quoridor.ConsoleClient.Abstract
{
    public interface IBot
    {
        TurnMenuOptions RequestTurn(IGameField gameField, int playerId);
        void SetRandomWall(ITurnService turnService, int playerId);
        MoveDirection RequestMoveDirection(List<MoveDirection> possibleDirections);
    }
}
