using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PYPProfileDotNet.Models;

namespace PYPProfileDotNet.Controllers
{
    public class ProfileController : Controller
    {
        private PYPContext db = new PYPContext();

        //
        // GET: /Profile/

        public ActionResult index(int user_id = 0)
        {
            string curUser = User.Identity.Name;

            IEnumerable<Game> gameQuery =
                from games in db.Games
                select games;

            ViewBag.Games = gameQuery.ToList();

            IEnumerable<User> userQuery =
                from users in db.Users
                select users;

            ViewBag.Users = userQuery.ToList();

            User thisUser = db.Users.Single(u => u.UserId == user_id);
            ViewBag.User = thisUser;
            ViewBag.doLink = (thisUser.UserName != curUser);
            // Grab all Friend entries where the desired User is Friend.User1 and Friend.User2 and the friendship is "accepted"
            var thisUserFriend1Entries =
                from f in db.Friends
                join u in db.Users
                on f.User1 equals u
                where u.UserId == user_id && f.Status.Status.Equals("accepted")
                select f;

            var thisUserFriend2Entries =
                from f in db.Friends
                join u in db.Users
                on f.User2 equals u
                where u.UserId == user_id && f.Status.Status.Equals("accepted")
                select f;

            // Add this User and all Friends into a List
            List<int> friendUserIds = new List<int>();
            List<string> friendUserNames = new List<string>();

            foreach (Friend friend in thisUserFriend1Entries)
            {
                friendUserIds.Add(friend.User2.UserId);
                friendUserNames.Add(friend.User2.UserName);
                if (curUser == friend.User2.UserName||curUser == friend.User1.UserName)
                    ViewBag.doLink = false;
            }
            foreach (Friend friend in thisUserFriend2Entries)
            {
                friendUserIds.Add(friend.User1.UserId);
                friendUserNames.Add(friend.User1.UserName); 
                if (curUser == friend.User1.UserName||curUser == friend.User2.UserName)
                    ViewBag.doLink = false;
            }

            ViewBag.FriendUserIds = friendUserIds;
            ViewBag.FriendUserNames = friendUserNames;

            IEnumerable<History> query =
                from history in db.History
                where history.User.UserId == user_id
                orderby history.Date descending
                select history;

            return View(query.ToList());
        }
    }
}
