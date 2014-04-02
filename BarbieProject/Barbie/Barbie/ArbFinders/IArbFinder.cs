﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.ArbFinders
{
    public interface IArbFinder
    {
        void FindArbs(List<FootballFixturesMap> mappedFixtures);
    }
}
