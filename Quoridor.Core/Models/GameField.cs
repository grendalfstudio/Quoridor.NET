using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HavocAndCry.Quoridor.Core.Abstract;

namespace HavocAndCry.Quoridor.Core.Models
{
    public class GameField : IGameField
    {
        private readonly List<Wall> __walls;
        private readonly List<Player> _players;

        public GameField(int playersAmount)
        {
            __walls = new List<Wall>();
            /*for (int i = 0; i < playersAmount; i++)
            {
                _players.Add(new Player());
            }*/
            //FirstPlayer = new Player(8, 4, 0);
            //SecondPlayer = new Player(0, 4, 8);
        }

        public int Size => 9;
        public IReadOnlyList<Wall> Walls => __walls;
        public IReadOnlyList<Player> Players => _players;

        public bool TryAddWall(WallCenter wallCenter, WallType wallType)
        {
            if (wallCenter.NorthRow < 0 || wallCenter.NorthRow > Size - 2
                || wallCenter.WestColumn < 0 || wallCenter.WestColumn > Size - 2)
            {
                return false;
            }

            switch (wallType)
            {
                case WallType.Horizontal:
                    if (IsWallAt(wallCenter)
                        || IsHorizontalWallAt(new WallCenter(wallCenter.NorthRow, wallCenter.WestColumn - 1))
                        || IsHorizontalWallAt(new WallCenter(wallCenter.NorthRow, wallCenter.WestColumn + 1)))
                    {
                        return false;
                    }
                    break;
                case WallType.Vertical:
                    if (IsWallAt(wallCenter)
                        || IsVerticalWallAt(new WallCenter(wallCenter.NorthRow - 1, wallCenter.WestColumn))
                        || IsVerticalWallAt(new WallCenter(wallCenter.NorthRow + 1, wallCenter.WestColumn)))
                    {
                        return false;
                    }
                    break;
            }

            __walls.Add(new Wall(wallType, wallCenter));
            return true;
        }

        public bool IsWallAt(WallCenter wallCenter)
        {
            return false; //_walls.Contains();
        }

        public bool IsHorizontalWallAt(WallCenter wallCenter)
        {
            return false; //_walls.ContainsKey(wallCenter) && _walls[wallCenter] == WallType.Horizontal;
        }

        public bool IsVerticalWallAt(WallCenter wallCenter)
        {
            return false; //_walls.ContainsKey(wallCenter) && _walls[wallCenter] == WallType.Vertical;
        }
    }
}
