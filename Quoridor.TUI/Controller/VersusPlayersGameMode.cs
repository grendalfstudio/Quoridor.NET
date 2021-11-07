using HavocAndCry.Quoridor.TUI.Abstract;

namespace HavocAndCry.Quoridor.TUI.Controller
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
            while (!IsGameEnded)
            {
                ConsoleView.CurrentPlayerId = CurrentPlayerId;
                ConsoleView.SetFieldChanged();
                ConsoleView.Redraw();
                
                MakePlayerTurn();
                ConsoleView.Redraw();
                
                GetNextPlayer();
            }
        }
        
        private void GetNextPlayer()
        {
            if (CurrentPlayerId == _numberOfPlayers)
                CurrentPlayerId = 1;
            else
                ++CurrentPlayerId;
        }
    }
}