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
        private readonly IGameField _gameField;
        private readonly ITurnService _turnService;
        private readonly GameFieldViewModel _fieldViewModel;

        private int _currentPlayerId = 0;
        private bool _isGameEnded = false;

        public VersusComputerGameMode()
        {
            _gameField = new GameField(2);
            _turnService = new TurnService(_gameField, new WavePathFinder(), OnPlayerReachedFinish);
        }

        private void OnPlayerReachedFinish(int playerId)
        {
            _isGameEnded = true;
            Console.WriteLine($"Player with ID {playerId} won!");
        }

        public void StartMainCycle()
        {
            while (!_isGameEnded)
            {
                if (_currentPlayerId is 0)
                {
                    MakePlayerTurn();
                    _currentPlayerId = 1;
                }
                else
                {
                    MakeBotTurn();
                    _currentPlayerId = 0;
                }
                _fieldViewModel.UpdateFieldView(_gameField);
                _fieldViewModel.PrintField();
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
                    break;
            }
        }

        private void MakeMove()
        {
            var possibleMoves = _turnService.GetPossibleMoves(_currentPlayerId);
            var direction = RequestMoveDirection(possibleMoves);
            _turnService.TryMove(direction, _currentPlayerId);
        }

        private void MakeBotTurn()
        {
            Console.WriteLine("Bot's turn");
        }
    }
}