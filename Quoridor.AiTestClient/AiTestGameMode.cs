using HavocAndCry.Quoridor.AiTestClient.Models;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using HavocAndCry.Quoridor.Core.Services;
using HavocAndCry.Quoridor.Bot.Abstract;

namespace HavocAndCry.Quoridor.AiTestClient
{
    public class AiTestGameMode : IGameMode
    {
        private readonly IBot _bot;
        private readonly ITurnService _turnService;
        private readonly IGameField _gameField;
        private readonly PlayerColor _myColor;

        private bool _isGameEnded = false;
        private PlayerColor _activeColor = PlayerColor.White;

        public AiTestGameMode(IBot bot, PlayerColor color)
        {
            _bot = bot;
            _myColor = color;
            _gameField = new GameField(2);
            _turnService = new TurnService(_gameField, new WavePathFinder(), (id) => { });
        }

        public void StartMainCycle()
        {
            while (!_isGameEnded)
            {
                if (_activeColor != _myColor)
                {
                    MakePlayerTurn();
                    _activeColor = _activeColor is PlayerColor.Black ? PlayerColor.White : PlayerColor.Black;
                }
                else
                {
                    MakeBotTurn();
                    _activeColor = _activeColor is PlayerColor.Black ? PlayerColor.White : PlayerColor.Black;
                }
            }
        }

        private void MakePlayerTurn()
        {
            var turn = ConsoleHelper.GetTurn(_activeColor, _gameField);
            switch (turn.TurnType)
            {
                case TurnType.Move:
                    MakeMove(turn);
                    break;
                case TurnType.SetWall:
                    SetWall(turn);
                    break;
            }
        }

        private void MakeMove(Move turn)
        {
            _turnService.TryMove(turn.MoveDirection, turn.PlayerId);
        }

        private void SetWall(Move turn)
        {
            _turnService.TrySetWall(turn.Wall, turn.PlayerId);
        }

        private void MakeBotTurn()
        {
            var turn = _bot.MakeMove(_turnService, (int)_activeColor);
            ConsoleHelper.WriteTurn(turn);
        }
    }
}