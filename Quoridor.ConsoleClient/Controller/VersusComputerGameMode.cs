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
            var turn = _bot.RequestTurn(GameField, CurrentPlayerId);
            switch (turn)
            {
                case TurnType.Move:
                    MakeMove();
                    break;
                case TurnType.SetWall:
                    SetWall();
                    break;
            }
        }

        protected new void MakeMove()
        {
            var possibleMoves = TurnService.GetPossibleMoves(CurrentPlayerId);
            var randomMoveDirection = _bot.RequestMoveDirection(possibleMoves);
            TurnService.TryMove(randomMoveDirection, CurrentPlayerId);
            ConsoleView.SetFieldChanged();
        }

        protected new void SetWall()
        {
            _bot.SetRandomWall(TurnService, CurrentPlayerId);
            ConsoleView.SetFieldChanged();
        }
    }
}