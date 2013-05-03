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
                where friend.User1.UserName == curUser
                select new FriendResult { id = friend.id, friendStatus = friend.Status.Status, friendName = user.Name };
            
            return View(friendQuery.ToList()); 
        }

        //
        // GET: /Friend/Details/5
        //view friend status? i guess?
        public ActionResult Details(int id = 0)
        {
            Friend friend = db.Friends.Find(id);
            var user1 = friend.User1;
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        //
        // GET: /Friend/Create
        //adds friend
        public ActionResult Create(int id = 0)
        {
            User user = db.Users.Find(id);
            IEnumerable<FriendStatus> statTypes = db.FriendStatuses.ToList();
            IEnumerable<User> userList = db.Users.ToList().OrderBy(u => u.UserName);
            ViewBag.statTypes = statTypes;
            ViewBag.userList = userList;
            return View(user);
        }

        //
        // POST: /Friend/Create

        [HttpPost]
        public ActionResult Create(User user2)
        {
            Friend friend = new Friend();
            curUser = User.Identity.Name;
            friend.User1 = db.Users.Single( f => f.UserName == curUser);

            IQueryable<User> friendQuery =
                from user in db.Users
                where user.UserName == user2.UserName
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
                select new FriendResult { id = frnd.id, friendStatus = frnd.Status.Status, friendName = user.Name };

            IEnumerable<FriendStatus> statTypes = db.FriendStatuses.ToList();
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