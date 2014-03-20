using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Barbie.DbMaintenance
{
    public class ArchiveOdds_MatchWinner_BF_OC
    {
        bARBieEntities barbieEntity;

        public ArchiveOdds_MatchWinner_BF_OC()
        {
            barbieEntity = new bARBieEntities();
        }
        
        public void ArchiveExpiredOdds()
        {
            var expiredOdds = barbieEntity.Arbs_Football_MatchWinner.Where(x => x.Expired == true);

            var oddsToArchive = new List<Arbs_Football_MatchWinner_Expired>();
            var oddsToDelete = new List<Arbs_Football_MatchWinner>();

            foreach (var oddsRecord in expiredOdds)
            {
                var expiredOddsRecord = new Arbs_Football_MatchWinner_Expired
                {
                    OriginalID = oddsRecord.ID,
                    MatchDateTime = oddsRecord.MatchDateTime,
                    HomeTeam = oddsRecord.HomeTeam,
                    AwayTeam = oddsRecord.AwayTeam,
                    Bookie = oddsRecord.Bookie,
                    BookieOdds = oddsRecord.BookieOdds,
                    BetFairLayLevel = oddsRecord.BetFairLayLevel,
                    BetFairOdds = oddsRecord.BetFairOdds,
                    BetFairCash = oddsRecord.BetFairCash,
                    Predication = oddsRecord.Predication,
                    BetFairUpdated = oddsRecord.BetFairUpdated,
                    OddsCheckerUpdated = oddsRecord.OddsCheckerUpdated,
                    Created = oddsRecord.Created,
                    Updated = oddsRecord.Updated,
                    ParentID = oddsRecord.ParentID
                };

                oddsToDelete.Add(oddsRecord);
                oddsToArchive.Add(expiredOddsRecord);
            }

            foreach (var record in oddsToArchive)
            {
                barbieEntity.Arbs_Football_MatchWinner_Expired.Add(record);
            }

            foreach (var record in oddsToDelete)
            {
                barbieEntity.Arbs_Football_MatchWinner.Remove(record);
            }

            barbieEntity.SaveChanges();
        }
    }
}
