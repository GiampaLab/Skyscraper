using System.Collections.Generic;

namespace SkyscraperCore
{
    public class GameStats
    {
        public IList<Player> _players { get; private set; }

        public GameStats(IList<Player> _players)
        {
            this._players = _players;
        }
    }
}