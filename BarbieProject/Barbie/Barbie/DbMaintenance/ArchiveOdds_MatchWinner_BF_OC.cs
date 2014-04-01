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
        bARBieEntities _barbieEntity;

        public ArchiveOdds_MatchWinner_BF_OC()
        {
            _barbieEntity = new bARBieEntities();
        }

        public ArchiveOdds_MatchWinner_BF_OC(bARBieEntities barbieEntity)
        {
            _barbieEntity = barbieEntity;
        }
        
        public void ArchiveExpiredOdds()
        {
            var expiredOdds = _barbieEntity.Arbs_Football_MatchWinner.Where(x => x.Expired == true);

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
                _barbieEntity.Arbs_Football_MatchWinner_Expired.Add(record);
            }

            foreach (var record in oddsToDelete)
            {
                _barbieEntity.Arbs_Football_MatchWinner.Remove(record);
            }

            _barbieEntity.SaveChanges();
        }
    }
}
