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
        private readonly List<Wall> _walls;
        private readonly List<Player> _players;
        public GameField(int playersAmount)
        {
            _walls = new List<Wall>();
            _players = new List<Player>();
            for (int i = 0; i < playersAmount; i++)
            {
                _players.Add(PlayersPresets.Players[i]);
            }
        }
        public int Size => 9;
        public IReadOnlyList<Wall> Walls => _walls;
        public IReadOnlyList<Player> Players => _players;

        public void AddWall(Wall wall)
        {
            _walls.Add(wall);
        }

        public bool IsWallAt(WallCenter wallCenter)
        {
            return _walls.Any(w => w.WallCenter.Equals(wallCenter));
        }

        public bool IsWallAt(WallCenter wallCenter, WallType wallType)
        {
            return _walls.Any(w => w.WallCenter.Equals(wallCenter) && w.Type == wallType);
        }
    }
}
