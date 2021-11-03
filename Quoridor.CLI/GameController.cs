using Quoridor.Bot;

namespace HavocAndCry.Quoridor.CLI
{
    public class GameController
    {
        private IGameMode _gameMode;

        public void StartGame()
        {
            _gameMode = new VersusComputerGameMode(new MinimaxBasedBot());
            var color = ConsoleHelper.GetColor();
        }
    }
}