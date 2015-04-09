using System.Collections.Generic;
using System.Linq;

namespace SkyscraperCore
{
    public class GameStats
    {
        public string winner { get; private set; }
        public int points { get; private set; }

        public GameStats(IList<Player> _players)
        {
            var player = _players.OrderBy(p => p.Cards.Count).Last();
            winner = player.DisplayName;
            points = player.Cards.Count;
        }
    }
}