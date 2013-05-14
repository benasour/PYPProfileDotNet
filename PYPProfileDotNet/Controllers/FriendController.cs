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
            if (fq.Count() != 0) //nonzero, so this relationship exists
            {
                ViewBag.message = "You already have a friend relation with this user.";
                return View("Error");
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

        //
        // GET: /Friend/Edit/5
        //change settings relating to friend
        public ActionResult Edit(int id=0)
        {
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                Console.Write("not found");
                return HttpNotFound();
            }

            IQueryable<FriendResult> friendQuery =
                from frnd in db.Friends
                join user in db.Users on frnd.User2.UserId equals user.UserId //gives name of second user
                where frnd.id == id
                select new FriendResult { id = frnd.id, friendStatus = frnd.Status.Status, friendName = frnd.User2.UserName, userName = frnd.User1.UserName };

            //make sure this user is actually part of this friendship before proceeding
            string curName = User.Identity.Name;
            bool isFriend = false;
            foreach (FriendResult fr in friendQuery)
                if (curName == fr.userName || curName == fr.friendName)
                    isFriend = true;
            if (!isFriend)
            {
                ViewBag.message = "You are trying to edit a friendship that you aren't a part of.";
                return View("Error"); 
            }

            IEnumerable<FriendStatus> statTypes = db.FriendStatuses.ToList();
            ViewBag.accepted = db.FriendStatuses.Single(s => s.Status == "accepted");
            ViewBag.declined = db.FriendStatuses.Single(s => s.Status == "declined");
            ViewBag.defriended = db.FriendStatuses.Single(s => s.Status == "defriended");
            ViewBag.requested = db.FriendStatuses.Single(s => s.Status == "requested");
            ViewBag.name = curName;
            ViewBag.statTypes = statTypes;
            ViewBag.StatusId = friend.Status.StatusId;
            return View(friendQuery.Single());
        }

        //
        // POST: /Friend/Edit/5

        [HttpPost]
        public ActionResult Edit(FriendResult friendRes)
        {
            //int test = friendRes.friendStatus;
            string test2 = friendRes.friendStatus;
            //if (ModelState.IsValid)
            //{

            Friend friend = db.Friends.Find(friendRes.id);
            friend.Status = db.FriendStatuses.Find(friendRes.friendStatusId);
            db.Entry(friend).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
               // return View(friendRes);
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