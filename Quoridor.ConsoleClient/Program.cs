using System;
using System.Collections.Generic;
using HavocAndCry.Quoridor.ConsoleClient.Controller;
using Quoridor.Bot;

//var game = new GameController();
//game.StartGame();

var minimaxTree = new MinimaxTree();
minimaxTree.AddChildrenNodes(new List<string> { "B4", "X3h", "A3", "Z7v" });
minimaxTree.Children[0].AddChildrenNodes(new List<string> { "B4", "X3h", "A3", "Z7v" });
Console.WriteLine(minimaxTree.EvaluateScore());

Console.ReadKey();