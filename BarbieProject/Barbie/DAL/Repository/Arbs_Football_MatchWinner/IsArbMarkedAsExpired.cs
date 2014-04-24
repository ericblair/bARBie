using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public partial class Repository : IRepository
    {
        public bool IsArbMarkedAsExpired(int arbID)
        {
            // TODO: Think about calling First() and dealing with error should arb not exist in table
            var expired = _barbieEntity.Arbs_Football_MatchWinner.FirstOrDefault(x => x.ID == arbID).Expired;

            if (expired.HasValue && expired.Value == true)
                return true;
            else
                return false;
        }
    }
}
