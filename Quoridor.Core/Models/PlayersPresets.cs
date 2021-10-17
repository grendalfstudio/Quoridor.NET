using System.Collections.Generic;

namespace HavocAndCry.Quoridor.Core.Models
{
    public static class PlayersPresets
    {
        public static List<Player> Players = new List<Player>()
        {
            new Player(1, 8, 4, 0),
            new Player(2, 0, 4, 8),
            new Player(3, 4, 0, 8),
            new Player(4, 4, 8, 0)
        };
    }
    
}