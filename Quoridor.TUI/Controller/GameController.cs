using System;
using HavocAndCry.Quoridor.TUI.Abstract;
using HavocAndCry.Quoridor.TUI.Models;
using HavocAndCry.Quoridor.Bot;
using static HavocAndCry.Quoridor.TUI.Menu;

namespace HavocAndCry.Quoridor.TUI.Controller
{
    public class GameController
    {
        private IGameMode _gameMode;

        public void StartGame()
        {
            while(true)
            {
                var selectedOption = RequestMainMenuOption();

                switch (selectedOption)
                {
                    case MainMenuOptions.Start:
                        SelectGameMode();
                        _gameMode.StartMainCycle();
                        break;
                    case MainMenuOptions.Help:
                        PrintHelp();
                        break;
                    case MainMenuOptions.Exit:
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SelectGameMode()
        {
            var numOfPlayers = RequestNumberOfPlayers();

            _gameMode = numOfPlayers switch
            {
                //1 => new VersusComputerGameMode(new SimpleBot()),
                1 => new VersusComputerGameMode(new MinimaxBasedBot()),
                < 5 => new VersusPlayersGameMode(numOfPlayers),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}