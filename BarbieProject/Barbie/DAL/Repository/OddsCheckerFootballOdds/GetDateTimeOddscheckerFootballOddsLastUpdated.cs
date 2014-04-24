﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public partial class Repository
    {
        public DateTime GetDateTimeOddscheckerFootballOddsLastUpdated(int fixtureID)
        {
            var fixtureOddsLastUpdated = _barbieEntity.OddsCheckerFootballOdds
                .Where(x => x.FixtureID == fixtureID)
                .OrderByDescending(x => x.ID)
                .Select(x => x.Updated)
                .First();

            return fixtureOddsLastUpdated;
        }
    }
}
