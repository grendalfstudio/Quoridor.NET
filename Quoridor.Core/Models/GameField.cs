using System.Collections.Generic;
using System.Linq;
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
                _players.Add(new Player(PlayersPresets.Players[i]));
            }
        }
        public GameField(List<Player> players)
        {
            _walls = new List<Wall>();
            _players = new List<Player>();
            foreach (var player in players)
            {
                _players.Add(new Player(player));
            }
        }
        public int Size => 9;
        public IReadOnlyList<Wall> Walls => _walls;
        public IReadOnlyList<Player> Players => _players;

        public void AddWall(Wall wall)
        {
            _walls.Add(wall);
        }

        public void RemoveWall(Wall wall)
        {
            _walls.Remove(wall);
        }

        public bool IsWallAt(WallCenter wallCenter)
        {
            return _walls.Any(w => w.WallCenter.Equals(wallCenter));
        }

        public bool IsWallAt(WallCenter wallCenter, WallType wallType)
        {
            return _walls.Any(w => w.WallCenter.Equals(wallCenter) && w.Type == wallType);
        }

        public object Clone()
        {
            var clone = new GameField(_players);
            foreach (var wall in Walls)
            {
                clone.AddWall(wall);
            }
            return clone;
        }
    }
}
