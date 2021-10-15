using HavocAndCry.Quoridor.ConsoleClient.Abstract;

namespace HavocAndCry.Quoridor.ConsoleClient.Controller
{
    public class VersusComputerGameMode : AbstractGameMode
    {
        public VersusComputerGameMode() : base(2)
        {
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
            _consoleView.WriteLine("Bot's turn");
            _consoleView.Redraw();
        }
    }
}