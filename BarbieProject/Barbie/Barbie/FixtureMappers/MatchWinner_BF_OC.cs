using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Objects.SqlClient;

namespace Barbie.FixtureMappers
{
    // The following articles have been invaluble with the design of this class:
    // http://www.sportshacker.net/posts/fuzzy_string_matching.html
    // http://www.tsjensen.com/blog/post/2011/05/27/Four+Functions+For+Finding+Fuzzy+String+Matches+In+C+Extensions.aspx
    public class MatchWinner_BF_OC
    {
        bARBieEntities barbieEntity;

        List<BetFairFootballFixtures> unmappedBetFairFixtures;
        List<OddsCheckerFootballFixtures> unmappedOddsCheckerFixtures;

        public MatchWinner_BF_OC()
        {
            barbieEntity = new bARBieEntities();
        }

        public void RunMapper()
        {
            // TODO: Externalise var to config file
            var maxLevenshteinValue = 15;

            for (var i = 0; i <= maxLevenshteinValue; i++)
            {
                unmappedBetFairFixtures = GetAllUnmappedBetFairFixtures();
                unmappedOddsCheckerFixtures = GetAllUnmappedOddsCheckerFixtures();

                var mappedFixtures = 
                              (from bf in unmappedBetFairFixtures
                              join oc in unmappedOddsCheckerFixtures
                              on bf.CompetitionID equals oc.CompetitionID
                              where bf.MatchDateTime == oc.MatchDateTime
                              && levenshtein(bf.HomeTeam, oc.HomeTeam) <= i
                              && levenshtein(bf.AwayTeam, oc.AwayTeam) <= i
                              select new FootballFixturesMap
                              {
                                  BetFairFixtureID = bf.ID,
                                  OddsCheckerFixtureID = oc.ID,
                              }).ToList();

                foreach (var fixture in mappedFixtures)
                {
                    barbieEntity.FootballFixturesMap.Add(fixture);
                }

                barbieEntity.SaveChanges();
            }
 
        }

        private List<BetFairFootballFixtures> GetAllUnmappedBetFairFixtures()
        {
            var unmappedFixtures = (from bf in barbieEntity.BetFairFootballFixtures
                                    where !(from map in barbieEntity.FootballFixturesMap
                                            select map.BetFairFixtureID)
                                            .Contains(bf.ID)
                                    select bf).ToList();

            return unmappedFixtures;
        }

        private List<OddsCheckerFootballFixtures> GetAllUnmappedOddsCheckerFixtures()
        {
            var unmappedFixtures = (from oc in barbieEntity.OddsCheckerFootballFixtures
                                    where !(from map in barbieEntity.FootballFixturesMap
                                            select map.OddsCheckerFixtureID)
                                            .Contains(oc.ID)
                                    select oc).ToList();

            return unmappedFixtures;
        }

        private Int32 levenshtein(String a, String b)
        {
            if (string.IsNullOrEmpty(a))
            {
                if (!string.IsNullOrEmpty(b))
                {
                    return b.Length;
                }
                return 0;
            }

            if (string.IsNullOrEmpty(b))
            {
                if (!string.IsNullOrEmpty(a))
                {
                    return a.Length;
                }
                return 0;
            }

            Int32 cost;
            Int32[,] d = new int[a.Length + 1, b.Length + 1];
            Int32 min1;
            Int32 min2;
            Int32 min3;

            for (Int32 i = 0; i <= d.GetUpperBound(0); i += 1)
            {
                d[i, 0] = i;
            }

            for (Int32 i = 0; i <= d.GetUpperBound(1); i += 1)
            {
                d[0, i] = i;
            }

            for (Int32 i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (Int32 j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    cost = Convert.ToInt32(!(a[i - 1] == b[j - 1]));

                    min1 = d[i - 1, j] + 1;
                    min2 = d[i, j - 1] + 1;
                    min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }
    }
}
