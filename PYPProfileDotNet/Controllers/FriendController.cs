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
    [Authorize]
    public class FriendController : Controller
    {
        private PYPContext db = new PYPContext();
        private string curUser;
        //
        // GET: /Friend/

        public ActionResult Index(int id = 0)
        {
            curUser = User.Identity.Name;
            //do a query so when we go to the view we only show this user's friends
            IQueryable<FriendResult> friendQuery =
                from friend in db.Friends
                join user in db.Users on friend.User2.UserId equals user.UserId //gives name of second user
                where friend.User1.UserName == curUser || friend.User2.UserName == curUser
                orderby friend.Status.StatusId descending, friend.User2.UserName ascending
                select new FriendResult { id = friend.id, friendStatus = friend.Status.Status, friendName = friend.User2.UserName, userName = friend.User1.UserName };

            ViewBag.name = User.Identity.Name;
            return View(friendQuery.ToList()); 
        }

        //
        // GET: /Friend/Create
        //adds friend
        public ActionResult Create(int id = 0)
        {
            User user = db.Users.Find(id);
            IEnumerable<FriendStatus> statTypes = db.FriendStatuses.ToList();
            IEnumerable<User> userList = db.Users.ToList().OrderBy(u => u.UserName);
            ViewBag.name = User.Identity.Name;
            ViewBag.statTypes = statTypes;
            ViewBag.userList = userList;
            return View(user);
        } 

        //
        // POST: /Friend/Create

        [HttpPost]
        public ActionResult Create(User user2)
        {
            curUser = User.Identity.Name;
            //first check to see if they are an existing friend
            IQueryable<Friend> fq =
                from frnd in db.Friends
                where (frnd.User1.UserName == curUser && user2.UserId == frnd.User2.UserId)
                    || (frnd.User2.UserName == curUser && user2.UserId == frnd.User1.UserId)
                select frnd; 

            foreach (Friend frnd in fq.ToList())
            {
                if (frnd.Status != null)
                {
                    if (frnd.Status.Status == "blocked" || frnd.Status.Status == "accepted" || frnd.Status.Status == "requested") //nonzero, so this relationship exists
                    {
                        ViewBag.message = "You already have a friend relation with this user.";
                        return View("Error");
                    }
                }
            }

            //They aren't, so proeed with the request
            Friend friend = new Friend();
            friend.User1 = db.Users.Single( f => f.UserName == curUser);

            IQueryable<User> friendQuery =
                from user in db.Users
                where user.UserId == user2.UserId
                select user;

            friend.User2 = friendQuery.Single();

            IQueryable<FriendStatus> statQuery =
                from stat in db.FriendStatuses
                where stat.Status == "requested"
                select stat;
            friend.Status = statQuery.Single();
            //if (ModelState.IsValid)
            //{
                db.Friends.Add(friend);
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //else Console.Write("bleh");

            //return View(friend);
        }



        public ActionResult CancelRequest(int id = 0)
        {

            Friend friend = db.Friends.Find(id);
            db.Friends.Remove(friend);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AcceptRequest(int id = 0)
        {
            Friend friend = db.Friends.Find(id);
            friend.Status = db.FriendStatuses.First(f => f.Status == "accepted");
            db.Entry(friend).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeclineRequest(int id = 0)
        {
            Friend friend = db.Friends.Find(id);
            friend.Status = db.FriendStatuses.First(f => f.Status == "declined");
            db.Entry(friend).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeFriend(int id = 0)
        {
            Friend friend = db.Friends.Find(id);
            friend.Status = db.FriendStatuses.First(f => f.Status == "defriended");
            db.Entry(friend).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Unblock(int id = 0)
        {
            Friend friend = db.Friends.Find(id);
            db.Friends.Remove(friend);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Block(int id = 0)
        {
            Friend friend = db.Friends.Find(id);
            friend.Status = db.FriendStatuses.First(f => f.Status == "blocked");
            db.Entry(friend).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

















        //
        // GET: /Friend/Delete/5
        //remove friend
        public ActionResult Delete(int id = 0)
        {
            ViewBag.name = User.Identity.Name;
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        //
        // POST: /Friend/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Friend friend = db.Friends.Find(id);
            db.Friends.Remove(friend);
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