using System;
using System.Collections.Generic;
using HavocAndCry.Quoridor.ConsoleClient.Models;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.ConsoleClient
{
    public static class Menu
    {
        public static MainMenuOptions RequestMainMenuOption()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\t1 - Start game");
                Console.WriteLine("\t2 - Help");
                Console.WriteLine("\t3 - Exit");

                var input = Console.ReadLine();
                if (int.TryParse(input, out var result) && Enum.IsDefined(typeof(MainMenuOptions), result) ) 
                    return (MainMenuOptions)result;
                
                Console.WriteLine("\tInvalid input. Press any key");
                Console.ReadKey();
            }
        }

        public static int RequestNumberOfPlayers()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\tEnter number of players (from 1 to 4, 1 means VS bot)");
                
                var input = Console.ReadLine();
                if (int.TryParse(input, out var result) && result is <= 4 and >= 1 ) 
                    return result;
                
                Console.WriteLine("\tInvalid input. Press any key");
                Console.ReadKey();
            }
        }

        public static TurnMenuOptions RequestTurnMenuOption()
        {
            Console.WriteLine("\t1 - Move");
            Console.WriteLine("\t2 - Set wall");
            
            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out var result) && Enum.IsDefined(typeof(TurnMenuOptions), result) ) 
                    return (TurnMenuOptions)result;
                
                Console.WriteLine("\tInvalid input. Press any key");
                Console.ReadKey();
            }
        }

        public static MoveDirection RequestMoveDirection(ICollection<MoveDirection> possibleMoves)
        {
            Console.WriteLine("\tChoose direction:");

            foreach (var moveDirection in possibleMoves)
            {
                Console.WriteLine($"\t{(int)moveDirection} - {moveDirection.ToString()}");
            }
            
            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out var result) 
                    && Enum.IsDefined(typeof(MoveDirection), result) 
                    && possibleMoves.Contains((MoveDirection)result))
                {
                    return (MoveDirection)result;
                }
                
                Console.WriteLine("\tInvalid input. Press any key");
                Console.ReadKey();
            }
        }

        public static void PrintHelp()
        {
            Console.WriteLine("Help");
            Console.ReadKey();
        }
    }
}