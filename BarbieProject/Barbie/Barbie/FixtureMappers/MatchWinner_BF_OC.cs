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

        List<BetFairFootballFixtures> unmatchedBetFairFixtures;
        List<OddsCheckerFootballFixtures> unmatchedOddsCheckerFixtures;

        public MatchWinner_BF_OC()
        {
            barbieEntity = new bARBieEntities();
        }

        public void RunMapper()
        {
            unmatchedBetFairFixtures = GetAllUnmappedBetFairFixtures();
            unmatchedOddsCheckerFixtures = GetAllUnmappedOddsCheckerFixtures();

            var exactMatches = GetExactMatches();
            UpdateUnmappedFixtures(exactMatches);
            

        }

        private List<BetFairFootballFixtures> GetAllUnmappedBetFairFixtures()
        {
            var unmappedFixtures = (from bf in barbieEntity.BetFairFootballFixtures
                                    where !(from map in barbieEntity.FootballFixturesMap
                                            select map.ID)
                                            .Contains(bf.ID)
                                    select bf).ToList();

            return unmappedFixtures;
                
        }

        private List<OddsCheckerFootballFixtures> GetAllUnmappedOddsCheckerFixtures()
        {
            var unmappedFixtures = (from oc in barbieEntity.OddsCheckerFootballFixtures
                                    where !(from map in barbieEntity.FootballFixturesMap
                                            select map.ID)
                                            .Contains(oc.ID)
                                    select oc).ToList();

            return unmappedFixtures;

        }

        /// <summary>
        /// Returns the IDs of any fixtures where the MatchDateTime, HomeTeam and AwayTeam
        /// values in both BF and OC tables match exactly
        /// </summary>
        /// <returns></returns>
        private List<FootballFixturesMap> GetExactMatches()
        {
            var exactMatches = (from bf in barbieEntity.BetFairFootballFixtures
                                join oc in barbieEntity.OddsCheckerFootballFixtures
                                on bf.MatchDateTime equals oc.MatchDateTime
                                where bf.CompetitionID == oc.CompetitionID
                                && bf.HomeTeam == oc.HomeTeam
                                && bf.AwayTeam == oc.AwayTeam
                                select new FootballFixturesMap
                                {
                                    BetFairFixtureID = bf.ID,
                                    OddsCheckerFixtureID = oc.ID
                                }).ToList();

            return exactMatches;
        }

        private void UpdateUnmappedFixtures(List<FootballFixturesMap> mappedFixtures)
        {
            var unmappedBFFixtures = (from bf in unmatchedBetFairFixtures
                                      where !(from map in mappedFixtures
                                              select map.BetFairFixtureID)
                                            .Contains(bf.ID)
                                      select bf).ToList();

            var unmappedOCFixtures = (from oc in unmatchedOddsCheckerFixtures
                                      where !(from map in mappedFixtures
                                              select map.OddsCheckerFixtureID)
                                            .Contains(oc.ID)
                                      select oc).ToList();

            unmatchedOddsCheckerFixtures = unmappedOCFixtures;
        }
    }
}
