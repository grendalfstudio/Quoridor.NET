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

        public static Wall RequestWall()
        {
            Console.Clear();
            Console.WriteLine("Wall will be placed under the selected cells if it is horizontal,\n or to the right if it is vertical\n");
            Console.WriteLine("\tSelect wall orientation:");
            Console.WriteLine("\t1 - Horizontal");
            Console.WriteLine("\t2 - Vertical");
            WallType orientation;
            WallCenter center;

            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out var result) && result is 1 or 2)
                {
                    orientation = result switch
                    {
                        1 => WallType.Horizontal,
                        2 => WallType.Vertical
                    };
                    break;
                }

                Console.WriteLine("Invalid input, try again");
            }

            Console.WriteLine("\tEnter cells' coordinates, separated by a comma");
            Console.WriteLine("\tFormat: row1,col1,row2,col2");

            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    var coordsTxt = input.Split(",");
                    if (!int.TryParse(coordsTxt[0], out var row) 
                        || !int.TryParse(coordsTxt[1], out var col) 
                        || row is < 0 or > 8 
                        || col is < 0 or > 8) 
                        continue;
                    
                    center = new WallCenter(row, col);
                    break;
                }
                catch (Exception _)
                {
                    Console.WriteLine("Invalid input, try again");
                }
            }

            return new Wall(orientation, center);
        }

        public static MoveDirection RequestMoveDirection(MoveDirection[] possibleMoves)
        {
            Console.Clear();
            Console.WriteLine("\tChoose move direction:");
            for (int i = 1; i <= possibleMoves.Length; i++)
            {
                Console.WriteLine($"\t{i} - {possibleMoves[i-1].ToString()}");
            }

            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out var result)
                    && result >= 1
                    && result <= possibleMoves.Length)
                    return possibleMoves[result];

                Console.WriteLine("Invalid input, try again");
            }
        }

        public static void PrintHelp()
        {
            Console.WriteLine("Help");
            Console.ReadKey();
        }
    }
}