using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavocAndCry.Quoridor.Core.Models
{
    public class GameField
    {
        private readonly Dictionary<WallCenter, WallType> walls;

        public GameField()
        {
            walls = new Dictionary<WallCenter, WallType>();
            FirstPlayer = new Player(8, 4, 0);
            SecondPlayer = new Player(0, 4, 8);
        }

        public int Size => 9;
        public IReadOnlyDictionary<WallCenter, WallType> Walls => walls;
        public Player FirstPlayer { get; }
        public Player SecondPlayer { get; }

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

            walls.Add(wallCenter, wallType);
            return true;
        }

        public bool IsWallAt(WallCenter wallCenter)
        {
            return walls.ContainsKey(wallCenter);
        }

        public bool IsHorizontalWallAt(WallCenter wallCenter)
        {
            return walls.ContainsKey(wallCenter) && walls[wallCenter] == WallType.Horizontal;
        }

        public bool IsVerticalWallAt(WallCenter wallCenter)
        {
            return walls.ContainsKey(wallCenter) && walls[wallCenter] == WallType.Vertical;
        }
    }
}
