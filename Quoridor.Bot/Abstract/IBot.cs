using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;

namespace Quoridor.Bot.Abstract
{
    public interface IBot
    {
        void MakeMove(ITurnService turnService, int playerId);
    }
}
