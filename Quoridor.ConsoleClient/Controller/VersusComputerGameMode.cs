using System;
using HavocAndCry.Quoridor.ConsoleClient.Abstract;
using HavocAndCry.Quoridor.ConsoleClient.Models;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using HavocAndCry.Quoridor.Model.Services;
using static HavocAndCry.Quoridor.ConsoleClient.Menu;

namespace HavocAndCry.Quoridor.ConsoleClient.Controller
{
    public class VersusComputerGameMode : IGameMode
    {
        private readonly ITurnService _turnService;
        private readonly ConsoleView _consoleView;

        private int _currentPlayerId = 2;
        private bool _isGameEnded;

        public VersusComputerGameMode()
        {
            var gameField = new GameField(2);
            _consoleView = new ConsoleView(gameField);
            _turnService = new TurnService(gameField, new WavePathFinder(), OnPlayerReachedFinish);
            
            InitializeWithView(_consoleView);
        }

        private void OnPlayerReachedFinish(int playerId)
        {
            _isGameEnded = true;
            _consoleView.Clear();
            _consoleView.WriteLine($"Player with ID {playerId} won!\nPress any key...");
            _consoleView.Redraw();
            Console.ReadKey();
        }

        public void StartMainCycle()
        {
            _consoleView.DrawField = true;
            while (!_isGameEnded)
            {
                if (_currentPlayerId is 2)
                {
                    MakePlayerTurn();
                    _currentPlayerId = 1;
                }
                else
                {
                    MakeBotTurn();
                    _currentPlayerId = 2;
                }
                _consoleView.Redraw();
            }
        }

        private void MakePlayerTurn()
        {
            var turn = RequestTurnMenuOption();
            switch (turn)
            {
                case TurnMenuOptions.Move:
                    MakeMove();
                    break;
                case TurnMenuOptions.SetWall:
                    var wall = RequestWall();
                    _turnService.TrySetWall(wall, _currentPlayerId);
                    _consoleView.SetFieldChanged();
                    break;
            }
        }

        private void MakeMove()
        {
            var possibleMoves = _turnService.GetPossibleMoves(_currentPlayerId);
            var direction = RequestMoveDirection(possibleMoves);
            _turnService.TryMove(direction, _currentPlayerId);
            _consoleView.SetFieldChanged();
        }

        private void MakeBotTurn()
        {
            _consoleView.WriteLine("Bot's turn");
            _consoleView.Redraw();
        }
    }
}