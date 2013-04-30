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

        [HandleError]
        public ActionResult Index( int game_id = 1, string filter = "Global" )
        {
            IEnumerable<Game> gameQuery =
                from games in db.Games
                select games;

            ViewBag.Games = gameQuery.ToList();
            ViewBag.GameId = game_id;
            ViewBag.Filter = filter;
            ViewBag.isAuthenticated = User.Identity.IsAuthenticated;

            IEnumerable<User> userQuery =
                from users in db.Users
                select users;

            ViewBag.Users = userQuery.ToList();

            IQueryable<Leaderboard> leaderboardQuery;

            // Want to filter top players differently if Global or the current User's friends
            // Friend Case
            if (filter.Equals("Friends"))
            {
                // Grab all Friend entries where the current User is Friend.User1 and Friend.User2 and the friendship is "accepted"
                User thisUser;
                try
                {
                    thisUser = db.Users.Single(u => u.UserName.Equals(User.Identity.Name));
                }
                catch (InvalidOperationException e)
                {
                    // TODO: If this Exception gets thrown, it is never handled. Must handle this Exception.
                    // This case is only in the event that the URL is manually adjusted. There is no possible navigation
                    // that will meet this condition.
                    throw new HttpException(401, "You must log in to view this.");
                }
                var thisUserFriend1Entries =
                    from f in db.Friends
                    join u in db.Users
                    on f.User1 equals u
                    where u.UserName.Equals(User.Identity.Name) && f.Status.Status.Equals("accepted")
                    select f;
                
                var thisUserFriend2Entries =
                    from f in db.Friends
                    join u in db.Users
                    on f.User2 equals u
                    where u.UserName.Equals(User.Identity.Name) && f.Status.Status.Equals("accepted")
                    select f;

                // Add this User and all Friends into a List
                List<int> friendUsers = new List<int>();
                friendUsers.Add(thisUser.UserId);

                foreach (Friend friend in thisUserFriend1Entries)
                {
                    friendUsers.Add(friend.User2.UserId);
                }
                foreach (Friend friend in thisUserFriend2Entries)
                {
                    friendUsers.Add(friend.User1.UserId);
                }

                // Query db.History for sum of all results involving this custom User list
                leaderboardQuery =
                    from h in db.History
                    where h.Game.GameId == game_id && friendUsers.Contains(h.User.UserId)
                    group h by new { user = h.User } into g
                    select new Leaderboard
                    {
                        User = g.Key.user,
                        Score = g.Sum(h => h.Score)
                    };
            }
            // Global case
            else
            {
                // Query db.History for sum of all results per player
                leaderboardQuery =
                    from h in db.History
                    where h.Game.GameId == game_id
                    group h by new { user = h.User } into g
                    select new Leaderboard
                    {
                        User = g.Key.user,
                        Score = g.Sum(h => h.Score)
                    };
            }

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