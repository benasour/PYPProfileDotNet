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
        private int curUser = 0;
        //
        // GET: /Friend/

        public ActionResult Index(int id = 0)
        {
                          
            //do a query so when we go to the view we only show this user's friends
            IQueryable<FriendResult> friendQuery =
                from friend in db.Friends
                join user in db.Users on friend.user_id2 equals user.UserId //gives name of second user
                where friend.user_id1 == curUser
                select new FriendResult { id = friend.id, friendStatus = friend.status_id, friendName = user.Name };
            
            return View(friendQuery.ToList()); 
        }

        //
        // GET: /Friend/Details/5
        //view friend status? i guess?
        public ActionResult Details(int id = 0)
        {
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        //
        // GET: /Friend/Create
        //adds friend
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Friend/Create

        [HttpPost]
        public ActionResult Create(Friend friend)
        {
            if (ModelState.IsValid)
            {
                friend.user_id1 = curUser;
                db.Friends.Add(friend);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(friend);
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
            return View(friend);
        }

        //
        // POST: /Friend/Edit/5

        [HttpPost]
        public ActionResult Edit(Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friend).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(friend);
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