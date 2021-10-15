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

        private readonly IGameField _gameField;
        private readonly int _numOfPlayers;
        private readonly char[,] _internalGameField;

        public GameFieldViewModel(IGameField gameField)
        {
            _gameField = gameField;
            _numOfPlayers = gameField.Players.Count;
            _internalGameField = new char[FieldViewSize, FieldViewSize];
            IsChanged = true;
        }

        public bool IsChanged { get; set; }

        public void UpdateFieldView()
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

            IReadOnlyCollection<Wall> walls = _gameField.Walls;
            foreach (var wall in walls)
            {
                int wallRow = 2 * wall.WallCenter.NorthRow + 2;
                int wallColumn = 2 * wall.WallCenter.WestColumn + 2;
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
                _internalGameField[2 * _gameField.Players[i].Row+1, 2 * _gameField.Players[i].Column+1] = PlayerMarkers[i];
            }
        }

        public string PrintField()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('\n', 2);
            for (int i = 0; i < FieldViewSize; i++)
            {
                stringBuilder.Append(' ', 25);
                for (int j = 0; j < FieldViewSize; j++)
                {
                    var sym = _internalGameField[i, j];
                    if (sym is >= '1' and <= '9')
                        stringBuilder.Append(' ').Append(sym);
                    else 
                        stringBuilder.Append(sym, 2);
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}