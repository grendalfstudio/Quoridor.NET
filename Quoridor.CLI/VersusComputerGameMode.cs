using HavocAndCry.Quoridor.CLI.Models;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using HavocAndCry.Quoridor.Core.Services;
using Quoridor.Bot.Abstract;

namespace HavocAndCry.Quoridor.CLI
{
    public class VersusComputerGameMode : IGameMode
    {
        private readonly IBot _bot;
        private readonly ITurnService _turnService;
        private readonly IGameField _gameField;
        private readonly PlayerColor _myColor;

        private int _currentPlayerId = 1;
        private bool _isGameEnded = false;
        private PlayerColor _activeColor = PlayerColor.White;

        public VersusComputerGameMode(IBot bot, PlayerColor color)
        {
            _bot = bot;
            _myColor = color;
            _gameField = new GameField(2);
            _turnService = new TurnService(_gameField, new WavePathFinder(), (id) => {});
        }

        private void MakePlayerTurn()
        {
            if (_gameField.Players.First(p => p.PlayerId == _currentPlayerId).WallsCount > 0)
            {
                var turn = ConsoleHelper.GetMove(_activeColor, _gameField);
                switch (turn.TurnType)
                {
                    case TurnType.Move:
                        MakeMove();
                        break;
                    case TurnType.SetWall:
                        SetWall();
                        break;
                }
            }
            else
            {
                MakeMove();
            }
        }

        private void MakeMove()
        {
            var success = false;
            while (!success)
            {
                var possibleMoves = _turnService.GetPossibleMoves(_currentPlayerId);
                var direction = RequestMoveDirection(possibleMoves);
                if (!_turnService.TryMove(direction, _currentPlayerId))
                {
                    ConsoleView.WriteLine("Can't make this move, select another");
                    ConsoleView.Redraw();
                }
                else
                {
                    success = true;
                    ConsoleView.SetFieldChanged();
                }
            }
        }

        private void SetWall()
        {
            var success = false;
            while (!success)
            {
                var wall = RequestWall();
                if (!_turnService.TrySetWall(wall, _currentPlayerId))
                {
                    ConsoleView.WriteLine("Can't place wall here, select another place\nPress any key...");
                    ConsoleView.Redraw();
                    Console.ReadKey();
                }
                else
                {
                    success = true;
                    ConsoleView.SetFieldChanged();
                }
            }
        }

        public void StartMainCycle()
        {
            while (!_isGameEnded)
            {
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
            }
        }

        private void MakeBotTurn()
        {
            _bot.MakeMove(_turnService, _currentPlayerId);
        }
    }
}