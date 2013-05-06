using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using PYPProfileDotNet.Models;

namespace PYPProfileDotNet.Controllers
{
    public class GameController : Controller
    {
        private PYPContext db = new PYPContext();

        //
        // GET: /Game/

        public ViewResult Index()
        {
            return View(db.Games.ToList());
        }

        [HttpGet]
        public ViewResult Horse()
        {
            string baseNodeUrl = WebConfigurationManager.ConnectionStrings["Node"].ConnectionString;
            ViewBag.FormAction = String.Format("'{0}horseform/'", baseNodeUrl);

            return View(GameOptions.HorseSuits);
        }

        [HttpGet]
        public ViewResult Coin()
        {
            string baseNodeUrl = WebConfigurationManager.ConnectionStrings["Node"].ConnectionString;
            ViewBag.FormAction = String.Format("'{0}coinform/'", baseNodeUrl);

            return View(GameOptions.CoinSides);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}