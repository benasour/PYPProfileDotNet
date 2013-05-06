using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PYPProfileDotNet.Models;

namespace PYPProfileDotNet.Controllers.Api
{
    public class HistoryController : ApiController
    {
        private PYPContext db = new PYPContext();

        // GET api/History
        public IEnumerable<History> GetHistories()
        {
            return db.History.AsEnumerable();
        }

        // GET api/History/5
        public History GetHistory(int id)
        {
            History history = db.History.Find(id);
            if (history == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return history;
        }

        // PUT api/History/5
        public HttpResponseMessage PutHistory(int id, History history)
        {
            if (ModelState.IsValid && id == history.id)
            {
                db.Entry(history).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/History
        public HttpResponseMessage PostHistory(CompletedGame game)
        {
            History history = new History();
            history.Score = game.Score;
            history.Game = db.Games.SingleOrDefault(g => g.Name == game.GameName);
            history.User = db.Users.SingleOrDefault(u => u.UserName == game.UserName);
            history.Date = DateTime.Now;

            //TODO: Validate History not game
            if (ModelState.IsValid)
            {
                db.History.Add(history);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, history);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = history.id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/History/5
        public HttpResponseMessage DeleteHistory(int id)
        {
            History history = db.History.Find(id);
            if (history == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.History.Remove(history);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, history);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}