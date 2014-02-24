using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BarbieSite.Models;

namespace BarbieSite.Controllers
{
    public class FootballArbController : Controller
    {
        private bARBieEntities db = new bARBieEntities();

        //
        // GET: /FootballArb/

        public ActionResult Index()
        {
            //return View(db.FootballArbs.ToList());

            var arbCutOffDateTime = DateTime.Now.AddHours(-3);

            //var activeArbs = db.FootballArbs.Where(arb => arb.MatchDateTime > arbCutOffDateTime).ToList();

            // group results by bookie and then display only the most recent arb
            var activeArbs =
                db.FootballArbs
                .Where(arb => arb.MatchDateTime > arbCutOffDateTime)
                .GroupBy(arb => new { arb.Fixture, arb.MatchDateTime, arb.Bookie, arb.Predication })
                .Select(arb => arb.FirstOrDefault())
                .OrderByDescending(arb => arb.Updated)
                .ToList()
                .OrderBy(arb => arb.MatchDateTime)
                .ThenBy(arb => arb.Fixture)
                .ThenBy(arb => arb.Bookie)
                .ThenBy(arb => arb.Predication);

            return View(activeArbs);
        }

        //
        // GET: /FootballArb/Details/5

        public ActionResult Details(int id = 0)
        {
            FootballArb footballarb = db.FootballArbs.Find(id);
            if (footballarb == null)
            {
                return HttpNotFound();
            }
            return View(footballarb);
        }

        //
        // GET: /FootballArb/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FootballArb/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FootballArb footballarb)
        {
            if (ModelState.IsValid)
            {
                db.FootballArbs.Add(footballarb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(footballarb);
        }

        //
        // GET: /FootballArb/Edit/5

        public ActionResult Edit(int id = 0)
        {
            FootballArb footballarb = db.FootballArbs.Find(id);
            if (footballarb == null)
            {
                return HttpNotFound();
            }
            return View(footballarb);
        }

        //
        // POST: /FootballArb/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FootballArb footballarb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(footballarb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(footballarb);
        }

        //
        // GET: /FootballArb/Delete/5

        public ActionResult Delete(int id = 0)
        {
            FootballArb footballarb = db.FootballArbs.Find(id);
            if (footballarb == null)
            {
                return HttpNotFound();
            }
            return View(footballarb);
        }

        //
        // POST: /FootballArb/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FootballArb footballarb = db.FootballArbs.Find(id);
            db.FootballArbs.Remove(footballarb);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}