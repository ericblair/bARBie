using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public partial class Repository
    {
        public int GetOddscheckerFixtureIdForArb(int arbFixtureMapID)
        {
            var fixtureID = _barbieEntity.FootballFixturesMap.FirstOrDefault(x => x.ID == arbFixtureMapID).OddsCheckerFixtureID;

            return fixtureID;
        }
    }
}
