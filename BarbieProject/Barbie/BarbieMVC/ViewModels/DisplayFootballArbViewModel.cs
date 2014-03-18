using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BarbieMVC.Models;

namespace BarbieMVC.ViewModels
{
    public class DisplayFootballArbViewModel
    {
        [Required, Display(Name = "Match Date Time")]
        public DateTime MatchDateTime { get; set; }
        [Required, Display(Name = "Home Team"), MaxLength(100)]
        public string HomeTeam { get; set; }
        [Required, Display(Name = "Away Team"), MaxLength(100)]
        public string AwayTeam { get; set; }
        [Required, Display(Name = "Predication"), MaxLength(100)]
        public string Predication { get; set; }
        [Required, Display(Name = "Bookie"), MaxLength(100)]
        public string Bookie { get; set; }
        [Required, Display(Name = "Bookie Odds")]
        public decimal BookieOdds { get; set; }
        [Required, Display(Name = "BetFair Odds")]
        public decimal BetFairOdds { get; set; }
        [Required, Display(Name = "BetFair Cash")]
        public Nullable<decimal> BetFairCash { get; set; }
        [Required, Display(Name = "BetFair Data Updated")]
        public DateTime BetFairUpdated { get; set; }
        [Required, Display(Name = "Bookie Data Updated")]
        public DateTime OddsCheckerUpdated { get; set; }
        [Required, Display(Name = "Arb Last Updated")]
        public Nullable<System.DateTime> Updated { get; set; }
        [Required, Display(Name = "Arb Found")]
        public DateTime Created { get; set; }
    }
}