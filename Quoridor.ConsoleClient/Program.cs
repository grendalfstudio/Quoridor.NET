using System;
using HavocAndCry.Quoridor.ConsoleClient.Controller;

var game = new GameController();
game.StartGame();

// var fieldViewModel = new GameFieldViewModel(2);
// fieldViewModel.UpdateFieldView(new GameField(2));
// fieldViewModel.PrintField();

Console.ReadKey();