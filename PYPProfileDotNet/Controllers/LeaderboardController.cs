using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PYPProfileDotNet.Models;

namespace PYPProfileDotNet.Controllers
{
    public class LeaderboardController : Controller
    {
        private PYPContext db = new PYPContext();

        //
        // GET: /Leaderboard/

        public ActionResult Index( int game_id = 1 )
        {
            IEnumerable<Game> gameQuery =
                from games in db.Games
                select games;

            ViewBag.Games = gameQuery.ToList();
            ViewBag.GameId = game_id;

            IEnumerable<User> userQuery =
                from users in db.Users
                select users;

            ViewBag.Users = userQuery.ToList();

            var leaderboardQuery =
                from h in db.History
                where h.Game.GameId == game_id
                group h by new { user = h.User } into g
                select new Leaderboard
                {
                    User = g.Key.user,
                    Score = g.Sum(h => h.Score)
                };
            var sortedLeaderboard = leaderboardQuery.OrderByDescending(s => s.Score);
            var leaderboardList = sortedLeaderboard.Take(10).ToList();

            return View(leaderboardList);
        }

        //
        // GET: /Leaderboard/Details/5

        public ActionResult Details(int id = 0)
        {
            History history = db.History.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        //
        // GET: /Leaderboard/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Leaderboard/Create

        [HttpPost]
        public ActionResult Create(History history)
        {
            if (ModelState.IsValid)
            {
                db.History.Add(history);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(history);
        }

        //
        // GET: /Leaderboard/Edit/5

        public ActionResult Edit(int id = 0)
        {
            History history = db.History.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        //
        // POST: /Leaderboard/Edit/5

        [HttpPost]
        public ActionResult Edit(History history)
        {
            if (ModelState.IsValid)
            {
                db.Entry(history).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(history);
        }

        //
        // GET: /Leaderboard/Delete/5

        public ActionResult Delete(int id = 0)
        {
            History history = db.History.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        //
        // POST: /Leaderboard/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            History history = db.History.Find(id);
            db.History.Remove(history);
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