//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Arbs_Football_MatchWinner_Expired
    {
        public int ID { get; set; }
        public int OriginalID { get; set; }
        public System.DateTime MatchDateTime { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Bookie { get; set; }
        public decimal BookieOdds { get; set; }
        public string BetFairLayLevel { get; set; }
        public decimal BetFairOdds { get; set; }
        public Nullable<decimal> BetFairCash { get; set; }
        public string Predication { get; set; }
        public System.DateTime BetFairUpdated { get; set; }
        public System.DateTime OddsCheckerUpdated { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public Nullable<int> ParentID { get; set; }
    }
}
