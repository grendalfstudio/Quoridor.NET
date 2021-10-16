using HavocAndCry.Quoridor.ConsoleClient.Abstract;
using HavocAndCry.Quoridor.ConsoleClient.Models;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Model.Services;
using System;

namespace HavocAndCry.Quoridor.ConsoleClient.Controller
{
    public class VersusComputerGameMode : AbstractGameMode
    {
        private readonly Random _random;

        public VersusComputerGameMode() : base(2)
        {
            _random = new Random();
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
            var turn = (TurnMenuOptions)_random.Next(1, 3);
            switch (turn)
            {
                case TurnMenuOptions.Move:
                    MakeMove();
                    break;
                case TurnMenuOptions.SetWall:
                    SetWall();
                    break;
            }
        }

        protected new void MakeMove()
        {
            var possibleMoves = _turnService.GetPossibleMoves(_currentPlayerId);
            int randomIndex = _random.Next(possibleMoves.Count);
            _turnService.TryMove(possibleMoves[randomIndex], _currentPlayerId);
            _consoleView.SetFieldChanged();
        }

        protected new void SetWall()
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
            } while (!_turnService.TrySetWall(randomWall, _currentPlayerId));

            _consoleView.SetFieldChanged();
        }
    }
}