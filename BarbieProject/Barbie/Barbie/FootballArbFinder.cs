using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

namespace Barbie
{
    public class FootballArbFinder
    {
        // TODO: Load this variable from somewhere external
        //private double betFairCommision = 0.05;
        private decimal? betFairCommision = new Decimal(0.05);

        public void FindArbs()
        {
            using (var barbieEntity = new bARBieEntities())
            {
                var footballMatches = barbieEntity.FootballMatches.ToList();

                foreach (var footballMatch in footballMatches)
                {
                    var oddscheckerOdds = new OddsCheckerMatchOdds(barbieEntity, footballMatch);

                    if (oddscheckerOdds.AreOddsSet == false)
                        continue;

                    var betfairOdds = new BetFairMatchOdds(barbieEntity, footballMatch);

                    if (betfairOdds.AreOddsSet == false)
                        continue;

                    findArbs(barbieEntity, oddscheckerOdds.HomeWinData, footballMatch, betfairOdds.HomeWinData.LayLow,
                            betfairOdds.HomeWinData.LayLowCash, betfairOdds.HomeWinData.Updated);
                    findArbs(barbieEntity, oddscheckerOdds.HomeWinData, footballMatch, betfairOdds.HomeWinData.LayMid,
                            betfairOdds.HomeWinData.LayMidCash, betfairOdds.HomeWinData.Updated);
                    findArbs(barbieEntity, oddscheckerOdds.HomeWinData, footballMatch, betfairOdds.HomeWinData.LayHigh,
                            betfairOdds.HomeWinData.LayHighCash, betfairOdds.HomeWinData.Updated);

                    findArbs(barbieEntity, oddscheckerOdds.AwayWinData, footballMatch, betfairOdds.AwayWinData.LayLow,
                            betfairOdds.AwayWinData.LayLowCash, betfairOdds.AwayWinData.Updated);
                    findArbs(barbieEntity, oddscheckerOdds.AwayWinData, footballMatch, betfairOdds.AwayWinData.LayMid,
                            betfairOdds.AwayWinData.LayMidCash, betfairOdds.AwayWinData.Updated);
                    findArbs(barbieEntity, oddscheckerOdds.AwayWinData, footballMatch, betfairOdds.AwayWinData.LayHigh,
                            betfairOdds.AwayWinData.LayHighCash, betfairOdds.AwayWinData.Updated);

                    findArbs(barbieEntity, oddscheckerOdds.DrawData, footballMatch, betfairOdds.DrawData.LayLow,
                            betfairOdds.DrawData.LayLowCash, betfairOdds.DrawData.Updated);
                    findArbs(barbieEntity, oddscheckerOdds.DrawData, footballMatch, betfairOdds.DrawData.LayMid,
                            betfairOdds.DrawData.LayMidCash, betfairOdds.DrawData.Updated);
                    findArbs(barbieEntity, oddscheckerOdds.DrawData, footballMatch, betfairOdds.DrawData.LayHigh,
                            betfairOdds.DrawData.LayHighCash, betfairOdds.DrawData.Updated);
                }
            }

        }

        private void findArbs(bARBieEntities barbieEntity, OddsCheckerFootballOdd osscheckerOdds,
                              FootballMatch matchDetails, decimal? layOdds, decimal? layCash, DateTime betFairRecordUpdated)
        {
            decimal? layOddsArbLimit = layOdds + (layOdds * betFairCommision);

            if (osscheckerOdds.Bet365.HasValue && layOddsArbLimit < osscheckerOdds.Bet365.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value, 
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "Bet365", osscheckerOdds.Bet365.Value, matchDetails.Fixture, 
                                    matchDetails.MatchDateTime, osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.BetDaq.HasValue && layOddsArbLimit < osscheckerOdds.BetDaq.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value, 
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "BetDaq",
                                    osscheckerOdds.BetDaq.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.BetFair.HasValue && layOddsArbLimit < osscheckerOdds.BetFair.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "BetFair",
                                    osscheckerOdds.BetFair.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.BetFred.HasValue && layOddsArbLimit < osscheckerOdds.BetFred.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "BetFred",
                                    osscheckerOdds.BetFred.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.BetVictor.HasValue && layOddsArbLimit < osscheckerOdds.BetVictor.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "BetVictor",
                                    osscheckerOdds.BetVictor.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.BetWay.HasValue && layOddsArbLimit < osscheckerOdds.BetWay.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "BetWay",
                                    osscheckerOdds.BetWay.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.BoyleSports.HasValue && layOddsArbLimit < osscheckerOdds.BoyleSports.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "BoyleSports",
                                    osscheckerOdds.BoyleSports.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.Bwin.HasValue && layOddsArbLimit < osscheckerOdds.Bwin.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "Bwin",
                                    osscheckerOdds.Bwin.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.C32RedBet.HasValue && layOddsArbLimit < osscheckerOdds.C32RedBet.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "C32RedBet",
                                    osscheckerOdds.C32RedBet.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.C888Sport.HasValue && layOddsArbLimit < osscheckerOdds.C888Sport.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "C888Sport",
                                    osscheckerOdds.C888Sport.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.Coral.HasValue && layOddsArbLimit < osscheckerOdds.Coral.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "Coral",
                                    osscheckerOdds.Coral.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.Ladbrokes.HasValue && layOddsArbLimit < osscheckerOdds.Ladbrokes.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "Ladbrokes",
                                    osscheckerOdds.Ladbrokes.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.PaddyPower.HasValue && layOddsArbLimit < osscheckerOdds.PaddyPower.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "PaddyPower",
                                    osscheckerOdds.PaddyPower.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.SkyBet.HasValue && layOddsArbLimit < osscheckerOdds.SkyBet.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "SkyBet",
                                    osscheckerOdds.SkyBet.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.SportingBet.HasValue && layOddsArbLimit < osscheckerOdds.SportingBet.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "SportingBet",
                                    osscheckerOdds.SportingBet.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.SpreadEx.HasValue && layOddsArbLimit < osscheckerOdds.SpreadEx.Value)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "SpreadEx",
                                    osscheckerOdds.SpreadEx.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.StanJames.HasValue && layOddsArbLimit < osscheckerOdds.StanJames)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "StanJames",
                                    osscheckerOdds.StanJames.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.ToteSport.HasValue && layOddsArbLimit < osscheckerOdds.ToteSport)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "ToteSport",
                                    osscheckerOdds.ToteSport.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.UniBet.HasValue && layOddsArbLimit < osscheckerOdds.UniBet)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "UniBet",
                                    osscheckerOdds.UniBet.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.WilliamHill.HasValue && layOddsArbLimit < osscheckerOdds.WilliamHill)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "WilliamHill",
                                    osscheckerOdds.WilliamHill.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.Winner.HasValue && layOddsArbLimit < osscheckerOdds.Winner)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "Winner",
                                    osscheckerOdds.Winner.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }

            if (osscheckerOdds.YouWin.HasValue && layOddsArbLimit < osscheckerOdds.YouWin)
            {
                WriteArbToDatabase(barbieEntity, layOdds.Value,
                                    (layCash.HasValue) ? layCash.Value : 0, 
                                    betFairRecordUpdated, "YouWin",
                                    osscheckerOdds.YouWin.Value, matchDetails.Fixture, matchDetails.MatchDateTime,
                                    osscheckerOdds.Updated, osscheckerOdds.Prediction);
            }
        }

        private void WriteArbToDatabase(bARBieEntities barbieEntity, decimal betFairLayLow, decimal betFairCash, 
                        DateTime betFairUpdated, string bookie, decimal bookieOdds, string fixture, DateTime matchDateTime,
                        DateTime oddsCheckerUpdated, string prediction)
        {
            var arb = new FootballArb();
            arb.BetFairOdds = betFairLayLow;
            arb.BetFairCash = betFairCash;
            arb.BetFairUpdated = betFairUpdated;
            arb.Bookie = bookie;
            arb.BookieOdds = bookieOdds;
            arb.Fixture = fixture;
            arb.MatchDateTime = matchDateTime;
            arb.OddsCheckerUpdated = oddsCheckerUpdated;
            arb.Predication = prediction;
            arb.Updated = DateTime.Now;

            barbieEntity.FootballArbs.Add(arb);
            barbieEntity.SaveChanges();
        }
    }
}
