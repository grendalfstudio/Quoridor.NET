using System;
using HavocAndCry.Quoridor.ConsoleClient.Controller;
using HavocAndCry.Quoridor.ConsoleClient.Models;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            GameController game = new();
            game.StartGame();

            // var fieldViewModel = new GameFieldViewModel(2);
            // fieldViewModel.UpdateFieldView(new GameField(2));
            // fieldViewModel.PrintField();
            
            Console.ReadKey();
        }
    }
}