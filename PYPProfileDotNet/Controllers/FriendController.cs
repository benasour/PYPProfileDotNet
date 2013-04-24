using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PYPProfileDotNet.Controllers
{
    public class FriendController : Controller
    {
        //
        // GET: /Friend/

        //default page shows all friends
        public ActionResult Index()
        {
            //default page for /friend/, return the html you want shown
            return View();
        }

        //specific page shows the 4 options given a friend's id
        /*public ActionResult index(int friendId)
        {
            //page for /friend/accept, return the html you want shown
            return View();
        }*/

        public ActionResult accept()
        {
            //page for /friend/accept, return the html you want shown
            return View();
        }

        public ActionResult reject()
        {
            //page for /friend/reject, return the html you want shown
            return View();
        }

        public ActionResult delete()
        {
            //page for /friend/delete, return the html you want shown
            return View();
        }

        public ActionResult cancel()
        {
            //page for /friend/cancel, return the html you want shown
            return View();
        }

    }
}
