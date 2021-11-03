using HavocAndCry.Quoridor.TUI.Abstract;
using Quoridor.Bot.Abstract;

namespace HavocAndCry.Quoridor.TUI.Controller
{
    public class VersusComputerGameMode : AbstractGameMode
    {
        private readonly IBot _bot;

        public VersusComputerGameMode(IBot bot) : base(2)
        {
            _bot = bot;
        }

        public override void StartMainCycle()
        {
            while (!IsGameEnded)
            {
                ConsoleView.CurrentPlayerId = CurrentPlayerId;
                ConsoleView.SetFieldChanged();
                ConsoleView.Redraw();
                
                if (CurrentPlayerId is 1)
                {
                    MakePlayerTurn();
                    CurrentPlayerId = 2;
                }
                else
                {
                    MakeBotTurn();
                    CurrentPlayerId = 1;
                }
                
                ConsoleView.Redraw();
            }
        }

        private void MakeBotTurn()
        {
            _bot.MakeMove(TurnService, CurrentPlayerId);
            ConsoleView.SetFieldChanged();
        }
    }
}