using HavocAndCry.Quoridor.ConsoleClient.Abstract;

namespace HavocAndCry.Quoridor.ConsoleClient.Controller
{
    public class VersusPlayersGameMode : AbstractGameMode
    {
        private readonly int _numberOfPlayers;

        public VersusPlayersGameMode(int playersCount) : base(playersCount)
        {
            _numberOfPlayers = playersCount;
        }

        public override void StartMainCycle()
        {
            while (!_isGameEnded)
            {
                _consoleView.CurrentPlayerId = _currentPlayerId;
                _consoleView.SetFieldChanged();
                _consoleView.Redraw();
                
                MakePlayerTurn();
                _consoleView.Redraw();
                
                GetNextPlayer();
            }
        }
        
        private void GetNextPlayer()
        {
            if (_currentPlayerId == _numberOfPlayers)
                _currentPlayerId = 1;
            else
                ++_currentPlayerId;
        }
    }
}