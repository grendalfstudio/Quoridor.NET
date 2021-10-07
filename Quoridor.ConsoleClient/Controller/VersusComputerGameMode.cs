using System;
using HavocAndCry.Quoridor.ConsoleClient.Abstract;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using HavocAndCry.Quoridor.Model.Services;
using static HavocAndCry.Quoridor.ConsoleClient.Menu;

namespace HavocAndCry.Quoridor.ConsoleClient.Controller
{
    public class VersusComputerGameMode : IGameMode
    {
        private readonly IGameField _gameField;
        private readonly ITurnService _turnService;

        private int _currentPlayerId = 0;

        public VersusComputerGameMode()
        {
            _gameField = new GameField(2);
            _turnService = new TurnService(_gameField, new WavePathFinder(), OnPlayerReachedFinish);
        }

        private void OnPlayerReachedFinish(int obj)
        {
            throw new NotImplementedException();
        }

        public void StartMainCycle()
        {
            while (true)
            {
                if (_currentPlayerId is 0)
                {
                    
                }
            }
        }

        private void MakePlayerTurn()
        {
            var turn = RequestTurnMenuOption();
            
        }
    }
}