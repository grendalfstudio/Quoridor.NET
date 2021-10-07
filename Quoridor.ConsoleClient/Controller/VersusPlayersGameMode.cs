using HavocAndCry.Quoridor.ConsoleClient.Abstract;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using HavocAndCry.Quoridor.Model.Services;

namespace HavocAndCry.Quoridor.ConsoleClient.Controller
{
    public class VersusPlayersGameMode : IGameMode
    {
        private readonly IGameField _gameField;
        private readonly ITurnService _turnService;
        
        public VersusPlayersGameMode(int playersCount)
        {
            _gameField = new GameField(playersCount);
            _turnService = new TurnService(_gameField, new WavePathFinder(), OnPlayerReachedFinish);
        }

        private void OnPlayerReachedFinish(int obj)
        {
            throw new System.NotImplementedException();
        }

        public void StartMainCycle()
        {
            throw new System.NotImplementedException();
        }
    }
}