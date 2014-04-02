using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DAL;
using System.Data.Objects.SqlClient;

namespace Barbie.FixtureMappers
{
    /// <summary>
    /// This class provides methods to match fixtures between the betfair and oddschecker
    /// match winner fixtures
    /// </summary>
    public class MatchWinner_BF_OC
    {
        bARBieEntities _barbieEntity;
        IConfigHelper _configHelper;

        List<BetFairFootballFixtures> _unmappedBetFairFixtures;
        List<OddsCheckerFootballFixtures> _unmappedOddsCheckerFixtures;

        public MatchWinner_BF_OC(bARBieEntities barbieEntity, IConfigHelper configHelper)
        {
            _barbieEntity = barbieEntity;
            _configHelper = configHelper;
        }

        /// <summary>
        /// This method looks for fixtures in the same competition and being played at the same time.
        /// It then uses a levenshtein matching algorithm to match the home and away teams.
        /// </summary>
        public void MapFixtures()
        {
            var maxLevenshteinValue = _configHelper.StringMatchingMaxLevenshteinValue();

            // Gradually increase the difference allowed between the strings, first matching the fixtures 
            // that are most similar. The team names can vary quite significantly between the sites so
            // by increasing the value it maps the fixtures..
            for (var i = 0; i <= maxLevenshteinValue; i++)
            {
                _unmappedBetFairFixtures = GetAllUnmappedBetFairFixtures();
                _unmappedOddsCheckerFixtures = GetAllUnmappedOddsCheckerFixtures();

                var mappedFixtures = 
                              (from bf in _unmappedBetFairFixtures
                              join oc in _unmappedOddsCheckerFixtures
                              on bf.CompetitionID equals oc.CompetitionID
                              where bf.MatchDateTime == oc.MatchDateTime
                              && GetLevenshteinValueOfStrings(bf.HomeTeam, oc.HomeTeam) <= i
                              && GetLevenshteinValueOfStrings(bf.AwayTeam, oc.AwayTeam) <= i
                              select new FootballFixturesMap
                              {
                                  BetFairFixtureID = bf.ID,
                                  OddsCheckerFixtureID = oc.ID,
                              }).ToList();

                foreach (var fixture in mappedFixtures)
                {
                    _barbieEntity.FootballFixturesMap.Add(fixture);
                }

                _barbieEntity.SaveChanges();
            }
 
        }

        private List<BetFairFootballFixtures> GetAllUnmappedBetFairFixtures()
        {
            using (var t = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                var unmappedFixtures = (from bf in _barbieEntity.BetFairFootballFixtures
                                        where !(from map in _barbieEntity.FootballFixturesMap
                                                select map.BetFairFixtureID)
                                                .Contains(bf.ID)
                                        select bf).ToList();

                return unmappedFixtures;
            }
        }

        private List<OddsCheckerFootballFixtures> GetAllUnmappedOddsCheckerFixtures()
        {
            var unmappedFixtures = (from oc in _barbieEntity.OddsCheckerFootballFixtures
                                    where !(from map in _barbieEntity.FootballFixturesMap
                                            select map.OddsCheckerFixtureID)
                                            .Contains(oc.ID)
                                    select oc).ToList();

            return unmappedFixtures;
        }

        // The following articles have been invaluble with the design of this method:
        // http://www.sportshacker.net/posts/fuzzy_string_matching.html
        // http://www.tsjensen.com/blog/post/2011/05/27/Four+Functions+For+Finding+Fuzzy+String+Matches+In+C+Extensions.aspx
        private Int32 GetLevenshteinValueOfStrings(String a, String b)
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
