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

        public static TurnType RequestTurnMenuOption()
        {
            _consoleView.Clear();
            _consoleView.WriteLine("\t1 - Move");
            _consoleView.WriteLine("\t2 - Set wall");
            _consoleView.Redraw();
            
            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out var result) && Enum.IsDefined(typeof(TurnType), result) ) 
                    return (TurnType)result;
                
                _consoleView.WriteLine("\tInvalid input. Press any key");
                _consoleView.Redraw();
                Console.ReadKey();
            }
        }

        public static Wall RequestWall()
        {
            _consoleView.Clear();
            _consoleView.WriteLine("\tWall will be placed under the selected cells if it is horizontal,\n\t or to the right if it is vertical\n");
            _consoleView.Redraw();
            WallType orientation;
            WallCenter center;

            _consoleView.WriteLine("\tEnter cells' coordinates, separated by a comma. Cells must be adjacent");
            _consoleView.WriteLine("\tFormat: [row1],[col1],[row2],[col2]");
            _consoleView.Redraw();

            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    var coordsTxt = input.Split(",");
                    var coordsInt = new List<int>(4);
                    foreach (var s in coordsTxt)
                    {
                        if (int.TryParse(s, out var result) && result is > 0 and < 10)
                        {
                            coordsInt.Add(result);
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                    }

                    if (coordsInt[0] == coordsInt[2])
                    {
                        orientation = WallType.Horizontal;
                    }
                    else if (coordsInt[1] == coordsInt[3])
                    {
                        orientation = WallType.Vertical;
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                    
                    center = new WallCenter(coordsInt[0]-1, coordsInt[1]-1);
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
            Console.Clear();
            Console.WriteLine("\tHelp\n");
            Console.WriteLine("If you have any questions, feel free to contact @andrry_armor via Telegram\n");
            Console.WriteLine("Press any key to return to menu");
            Console.ReadKey();
        }
    }
}