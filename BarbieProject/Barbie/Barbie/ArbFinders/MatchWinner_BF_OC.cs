using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.ArbFinders
{
    public class MatchWinner_BF_OC
    {
        bARBieEntities barbieEntity;

        // TODO: Load this variable from somewhere external
        private decimal? betFairCommision = new Decimal(0.05);

        public MatchWinner_BF_OC()
        {
            barbieEntity = new bARBieEntities();
        }

        public void CheckAllUnexpiredMappedFixtures()
        {
            int matchExpiryLimitMins;

            if (!Int32.TryParse(ConfigurationManager.AppSettings["MatchExpiryLimitMins"], out matchExpiryLimitMins))
            {
                // log error 
                return;
            }

            var matchExpiryDateTime = DateTime.Now.AddMinutes(-matchExpiryLimitMins);

            var mappedFixtures = barbieEntity.FootballFixturesMap
                                    .Where(x => x.OddsCheckerFootballFixtures.MatchDateTime >= matchExpiryDateTime)
                                    .OrderBy(x => x.OddsCheckerFootballFixtures.MatchDateTime)
                                    .ToList();

            foreach (var fixture in mappedFixtures)
            {
                var ocOddsCollection = new OCMatchWinnerOddsCollection(fixture);
                var bfOddsCollection = new BFMatchWinnerOddsCollection(fixture);

                if (ocOddsCollection.HomeWinOdds != null && bfOddsCollection.HomeWinOdds != null)
                {
                    findArbs(ocOddsCollection.HomeWinOdds, bfOddsCollection.HomeWinOdds.LayLow,
                            bfOddsCollection.HomeWinOdds.LayLowCash, bfOddsCollection.HomeWinOdds.Updated, fixture.ID);
                    findArbs(ocOddsCollection.HomeWinOdds, bfOddsCollection.HomeWinOdds.LayMid,
                                bfOddsCollection.HomeWinOdds.LayMidCash, bfOddsCollection.HomeWinOdds.Updated, fixture.ID);
                    findArbs(ocOddsCollection.HomeWinOdds, bfOddsCollection.HomeWinOdds.LayHigh,
                            bfOddsCollection.HomeWinOdds.LayHighCash, bfOddsCollection.HomeWinOdds.Updated, fixture.ID);
                }

                if (ocOddsCollection.AwayWinOdds != null && bfOddsCollection.AwayWinOdds != null)
                {
                    findArbs(ocOddsCollection.AwayWinOdds, bfOddsCollection.AwayWinOdds.LayLow,
                            bfOddsCollection.AwayWinOdds.LayLowCash, bfOddsCollection.AwayWinOdds.Updated, fixture.ID);
                    findArbs(ocOddsCollection.AwayWinOdds, bfOddsCollection.AwayWinOdds.LayMid,
                            bfOddsCollection.AwayWinOdds.LayMidCash, bfOddsCollection.AwayWinOdds.Updated, fixture.ID);
                    findArbs(ocOddsCollection.AwayWinOdds, bfOddsCollection.AwayWinOdds.LayHigh,
                            bfOddsCollection.AwayWinOdds.LayHighCash, bfOddsCollection.AwayWinOdds.Updated, fixture.ID);
                }

                if (ocOddsCollection.DrawOdds != null && bfOddsCollection.DrawOdds != null)
                {
                    findArbs(ocOddsCollection.DrawOdds, bfOddsCollection.DrawOdds.LayLow,
                            bfOddsCollection.DrawOdds.LayLowCash, bfOddsCollection.DrawOdds.Updated, fixture.ID);
                    findArbs(ocOddsCollection.DrawOdds, bfOddsCollection.DrawOdds.LayMid,
                            bfOddsCollection.DrawOdds.LayMidCash, bfOddsCollection.DrawOdds.Updated, fixture.ID);
                    findArbs(ocOddsCollection.DrawOdds, bfOddsCollection.DrawOdds.LayHigh,
                            bfOddsCollection.DrawOdds.LayHighCash, bfOddsCollection.DrawOdds.Updated, fixture.ID);
                }

            }
        }

        private void findArbs(OddsCheckerFootballOdds ocOdds, decimal? layOdds, decimal? layCash, DateTime bfUpdated, int fixtureID)
        {
            if (ocOdds == null)
                return;

            var layOddsArbLimit = layOdds + (layOdds * betFairCommision);
            var matchDateTime = ocOdds.OddsCheckerFootballFixtures.MatchDateTime;
            var homeTeam = ocOdds.OddsCheckerFootballFixtures.HomeTeam;
            var awayTeam = ocOdds.OddsCheckerFootballFixtures.AwayTeam;

            if (ocOdds.Bet365.HasValue && layOddsArbLimit < ocOdds.Bet365.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "Bet365", ocOdds.Bet365.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.BetDaq.HasValue && layOddsArbLimit < ocOdds.BetDaq.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "BetDaq", ocOdds.BetDaq.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.BetFair.HasValue && layOddsArbLimit < ocOdds.BetFair.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "BetFair", ocOdds.BetFair.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.BetFred.HasValue && layOddsArbLimit < ocOdds.BetFred.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "BetFred", ocOdds.BetFred.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.BetVictor.HasValue && layOddsArbLimit < ocOdds.BetVictor.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "BetVictor", ocOdds.BetVictor.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.BetWay.HasValue && layOddsArbLimit < ocOdds.BetWay.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "BetWay", ocOdds.BetWay.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.BoyleSports.HasValue && layOddsArbLimit < ocOdds.BoyleSports.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "BoyleSports", ocOdds.BoyleSports.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.Bwin.HasValue && layOddsArbLimit < ocOdds.Bwin.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "Bwin", ocOdds.Bwin.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.C32RedBet.HasValue && layOddsArbLimit < ocOdds.C32RedBet.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "C32RedBet", ocOdds.C32RedBet.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.C888Sport.HasValue && layOddsArbLimit < ocOdds.C888Sport.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "C888Sport", ocOdds.C888Sport.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.Coral.HasValue && layOddsArbLimit < ocOdds.Coral.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "Coral", ocOdds.Coral.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.Ladbrokes.HasValue && layOddsArbLimit < ocOdds.Ladbrokes.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "Ladbrokes", ocOdds.Ladbrokes.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.PaddyPower.HasValue && layOddsArbLimit < ocOdds.PaddyPower.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "PaddyPower", ocOdds.PaddyPower.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.SkyBet.HasValue && layOddsArbLimit < ocOdds.SkyBet.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "SkyBet", ocOdds.SkyBet.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.SportingBet.HasValue && layOddsArbLimit < ocOdds.SportingBet.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "SportingBet", ocOdds.SportingBet.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.SpreadEx.HasValue && layOddsArbLimit < ocOdds.SpreadEx.Value)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "SpreadEx", ocOdds.SpreadEx.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.StanJames.HasValue && layOddsArbLimit < ocOdds.StanJames)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "StanJames", ocOdds.StanJames.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.ToteSport.HasValue && layOddsArbLimit < ocOdds.ToteSport)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "ToteSport", ocOdds.ToteSport.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.UniBet.HasValue && layOddsArbLimit < ocOdds.UniBet)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "UniBet", ocOdds.UniBet.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.WilliamHill.HasValue && layOddsArbLimit < ocOdds.WilliamHill)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "WilliamHill", ocOdds.WilliamHill.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.Winner.HasValue && layOddsArbLimit < ocOdds.Winner)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "Winner", ocOdds.Winner.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }

            if (ocOdds.YouWin.HasValue && layOddsArbLimit < ocOdds.YouWin)
            {
                WriteArbToDatabase(matchDateTime, homeTeam, awayTeam,
                                    layOdds.Value, (layCash.HasValue) ? layCash.Value : 0, bfUpdated,
                                    "YouWin", ocOdds.YouWin.Value, ocOdds.Updated, ocOdds.Prediction, fixtureID);
            }
        }

        private void WriteArbToDatabase(DateTime matchDateTime, string homeTeam, string awayTeam,
                                        decimal betFairLayLow, decimal betFairCash,
                                        DateTime betFairUpdated, string bookie, decimal bookieOdds,
                                        DateTime oddsCheckerUpdated, string prediction, int fixtureID)
        {
            // Check if record exists before writing
            var record = barbieEntity.Arbs_Football_MatchWinner
                            .Where(x => x.FixtureMapID == fixtureID)
                            .Where(x => x.BetFairOdds == betFairLayLow)
                            .Where(x => x.BetFairCash == betFairCash)
                            .Where(x => x.BetFairUpdated == betFairUpdated)
                            .Where(x => x.Bookie == bookie)
                            .Where(x => x.BookieOdds == bookieOdds)
                            .Where(x => x.OddsCheckerUpdated == oddsCheckerUpdated)
                            .Where(x => x.Predication == prediction)
                            .FirstOrDefault();

            if (record != null)
                return;

            var arb = new Arbs_Football_MatchWinner();
            arb.MatchDateTime = matchDateTime;
            arb.HomeTeam = homeTeam;
            arb.AwayTeam = awayTeam;
            arb.BetFairOdds = betFairLayLow;
            arb.BetFairCash = betFairCash;
            arb.BetFairUpdated = betFairUpdated;
            arb.Bookie = bookie;
            arb.BookieOdds = bookieOdds;
            arb.OddsCheckerUpdated = oddsCheckerUpdated;
            arb.Predication = prediction;
            arb.FixtureMapID = fixtureID;
            arb.Updated = DateTime.Now;

            barbieEntity.Arbs_Football_MatchWinner.Add(arb);
            barbieEntity.SaveChanges();
        }
    }
}
