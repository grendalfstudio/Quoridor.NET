using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;

namespace Quoridor.Bot.Abstract
{
    public interface IBot
    {
        TurnType RequestTurn(IGameField gameField, int playerId);
        void SetRandomWall(ITurnService turnService, int playerId);
        MoveDirection RequestMoveDirection(List<MoveDirection> possibleDirections);
    }
}
