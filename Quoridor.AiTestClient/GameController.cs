using HavocAndCry.Quoridor.Bot;

namespace HavocAndCry.Quoridor.AiTestClient
{
    public class GameController
    {
        private IGameMode? _gameMode;

        public void StartGame()
        {
            var color = ConsoleHelper.GetColor();
            _gameMode = new AiTestGameMode(new MinimaxBasedBot(), color);
            _gameMode.StartMainCycle();
        }
    }
}