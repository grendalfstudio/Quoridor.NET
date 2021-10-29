using HavocAndCry.Quoridor.ConsoleClient.Abstract;
using HavocAndCry.Quoridor.ConsoleClient.Models;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Model.Services;
using System;
using Quoridor.Bot;
using Quoridor.Bot.Abstract;

namespace HavocAndCry.Quoridor.ConsoleClient.Controller
{
    public class VersusComputerGameMode : AbstractGameMode
    {
        private readonly IBot _bot;

        public VersusComputerGameMode() : base(2)
        {
            _bot = new SimpleBot();
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