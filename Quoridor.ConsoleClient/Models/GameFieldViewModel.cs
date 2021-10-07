using System;
using System.Collections.Generic;
using System.Text;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.ConsoleClient.Models
{
    public class GameFieldViewModel
    {
        private const int FieldViewSize = 18;
        private static readonly char[] PlayerMarkers = { 'A', 'B', 'C', 'D' };
        
        private readonly int _numOfPlayers;
        private readonly char[,] _internalGameField;

        public GameFieldViewModel(int numOfPlayers)
        {
            _numOfPlayers = numOfPlayers;
            _internalGameField = new char[FieldViewSize, FieldViewSize];
        }

        public void UpdateFieldView(IGameField gameField)
        {
            for (int i = 0; i < FieldViewSize; i++)
            {
                for (int j = 0; j < FieldViewSize; j++)
                {
                    if (i is 0 && j % 2 == 1)
                        _internalGameField[i, j] = Convert.ToChar((j / 2 + 1).ToString());
                    else
                    {
                        if (j is 0 && i % 2 == 1)
                            _internalGameField[i, j] = Convert.ToChar((i / 2 + 1).ToString());
                        else
                            _internalGameField[i, j] = (i % 2 == 1 && j % 2 == 1)
                                ? '#'
                                : ' ';
                    }
                }
            }

            IReadOnlyCollection<Wall> walls = gameField.Walls;
            foreach (var wall in walls)
            {
                int wallRow = 2 * wall.WallCenter.NorthRow + 1;
                int wallColumn = 2 * wall.WallCenter.WestColumn + 1;
                _internalGameField[wallRow, wallColumn] = '█';
                
                switch (wall.Type)
                {
                    case WallType.Horizontal:
                        _internalGameField[wallRow, wallColumn - 1] = '█';
                        _internalGameField[wallRow, wallColumn + 1] = '█';
                        break;
                    case WallType.Vertical:
                        _internalGameField[wallRow - 1, wallColumn] = '█';
                        _internalGameField[wallRow + 1, wallColumn] = '█';
                        break;
                }
            }

            for (int i = 0; i < _numOfPlayers; i++)
            {
                _internalGameField[2 * gameField.Players[i].Row+1, 2 * gameField.Players[i].Column+1] = PlayerMarkers[i];
            }
        }

        public void PrintField()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('\n', 2);
            for (int i = 0; i < FieldViewSize; i++)
            {
                stringBuilder.Append(' ', 25);
                for (int j = 0; j < FieldViewSize; j++)
                {
                    stringBuilder.Append(_internalGameField[i, j]);
                }
                stringBuilder.AppendLine();
            }

            Console.WriteLine(stringBuilder.ToString());
        }
    }
}