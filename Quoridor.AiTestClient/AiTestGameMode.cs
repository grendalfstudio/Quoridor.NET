using System.Text;
using System.Text.Json;
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
            _turnService = new TurnService(_gameField, new AStarPathFinder(), (id) => { });
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
            if(!_turnService.TryMove(turn.Position, turn.PlayerId))
            {
                var sb = new StringBuilder();
                sb.AppendLine($"\n//[{DateTime.Now}] Can't move to {turn.Position}");
                var field = JsonSerializer.Serialize(_gameField);
                sb.AppendLine(field);
                File.AppendAllText(@"./log.jsonc", sb.ToString());
            }
        }

        private void SetWall(Move turn)
        {
            if(!_turnService.TrySetWall(turn.Wall, turn.PlayerId, true))
            {
                var sb = new StringBuilder();
                sb.AppendLine($"\n//[{DateTime.Now}] Can't place wall at {turn.Wall}");
                var field = JsonSerializer.Serialize(_gameField);
                sb.AppendLine(field);
                File.AppendAllText(@"./log.jsonc", sb.ToString());
            }
        }

        private void MakeBotTurn()
        {
            var turn = _bot.MakeMove(_turnService, (int)_activeColor);
            ConsoleHelper.WriteTurn(turn);
        }
    }
}