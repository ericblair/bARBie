using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.Arbs
{
    public interface IArbFinder
    {
        void CheckLatestOddsForArbs(int fixtureMapID);
        void FindArbs(List<FootballFixturesMap> mappedFixtures);
    }
}
