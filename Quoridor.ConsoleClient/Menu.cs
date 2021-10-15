using System;
using System.Collections.Generic;
using HavocAndCry.Quoridor.ConsoleClient.Models;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.ConsoleClient
{
    public static class Menu
    {
        private static ConsoleView _consoleView;

        public static void InitializeWithView(ConsoleView consoleView) => _consoleView = consoleView;
        
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
            _consoleView.WriteLine("\t1 - Move");
            _consoleView.WriteLine("\t2 - Set wall");
            _consoleView.Redraw();
            
            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out var result) && Enum.IsDefined(typeof(TurnMenuOptions), result) ) 
                    return (TurnMenuOptions)result;
                
                _consoleView.WriteLine("\tInvalid input. Press any key");
                _consoleView.Redraw();
                Console.ReadKey();
            }
        }

        public static Wall RequestWall()
        {
            _consoleView.Clear();
            _consoleView.WriteLine("Wall will be placed under the selected cells if it is horizontal,\n or to the right if it is vertical\n");
            _consoleView.WriteLine("\tSelect wall orientation:");
            _consoleView.WriteLine("\t1 - Horizontal");
            _consoleView.WriteLine("\t2 - Vertical");
            _consoleView.Redraw();
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

                _consoleView.WriteLine("Invalid input, try again");
                _consoleView.Redraw();
            }

            _consoleView.WriteLine("\tEnter cells' coordinates, separated by a comma");
            _consoleView.WriteLine("\tFormat: row1,col1,row2,col2");
            _consoleView.Redraw();

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
                    _consoleView.WriteLine("Invalid input, try again");
                    _consoleView.Redraw();
                }
            }

            return new Wall(orientation, center);
        }

        public static MoveDirection RequestMoveDirection(IList<MoveDirection> possibleMoves)
        {
            _consoleView.Clear();
            _consoleView.WriteLine("\tChoose move direction:");
            for (int i = 1; i <= possibleMoves.Count; i++)
            {
                _consoleView.WriteLine($"\t{i} - {possibleMoves[i-1].ToString()}");
            }
            _consoleView.Redraw();

            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out var result)
                    && result >= 1
                    && result <= possibleMoves.Count)
                    return possibleMoves[result-1];

                _consoleView.WriteLine("Invalid input, try again");
                _consoleView.Redraw();
            }
        }

        public static void PrintHelp()
        {
            _consoleView.WriteLine("Help");
            _consoleView.Redraw();
            Console.ReadKey();
        }
    }
}