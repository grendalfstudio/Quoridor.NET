using HavocAndCry.Quoridor.ConsoleClient.Abstract;
using HavocAndCry.Quoridor.ConsoleClient.Models;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Model.Services;
using System;
using Quoridor.Bot;
using Quoridor.Bot.Abstract;

namespace HavocAndCry.Quoridor.ConsoleClient.Controller
{
    public class VersusComputerGameMode : AbstractGameMode
    {
        private readonly IBot _bot;

        public VersusComputerGameMode() : base(2)
        {
            _bot = new SimpleBot();
        }

        public override void StartMainCycle()
        {
            while (!_isGameEnded)
            {
                _consoleView.CurrentPlayerId = _currentPlayerId;
                _consoleView.SetFieldChanged();
                _consoleView.Redraw();
                
                if (_currentPlayerId is 1)
                {
                    MakePlayerTurn();
                    _currentPlayerId = 2;
                }
                else
                {
                    MakeBotTurn();
                    _currentPlayerId = 1;
                }
                
                _consoleView.Redraw();
            }
        }

        private void MakeBotTurn()
{
            var turn = _bot.RequestTurn(_gameField, _currentPlayerId);
            switch (turn)
            {
                case TurnType.Move:
                    MakeMove();
                    break;
                case TurnType.SetWall:
                    SetWall();
                    break;
            }
        }

        protected new void MakeMove()
        {
            var possibleMoves = _turnService.GetPossibleMoves(_currentPlayerId);
            var randomMoveDirection = _bot.RequestMoveDirection(possibleMoves);
            _turnService.TryMove(randomMoveDirection, _currentPlayerId);
            _consoleView.SetFieldChanged();
        }

        protected new void SetWall()
        {
            _bot.SetRandomWall(_turnService, _currentPlayerId);
            _consoleView.SetFieldChanged();
        }
    }
}