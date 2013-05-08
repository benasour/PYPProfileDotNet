using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Helpers;
using System.Threading;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using PYPProfileDotNet.Models;

namespace PYPProfileDotNet.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin model, string returnUrl)
        {
          
            if (ModelState.IsValid)
            {
                using (PYPContext db = new PYPContext())
                {
                    // Lookup user by unique username
                    User user = db.Users.SingleOrDefault(u => u.UserName == model.UserName);

                    if (user != null && Crypto.VerifyHashedPassword(user.Password, model.Password + user.Salt))
                    {
                        // Credentials Passed Login the User
                        FormsAuthentication.SetAuthCookie(user.UserName, model.RememberMe);
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(Response.Cookies.Get(FormsAuthentication.FormsCookieName).Value);
                        GenericPrincipal userPrincipal = new GenericPrincipal(new FormsIdentity(ticket), null);
                        System.Web.HttpContext.Current.User = userPrincipal;
                        Thread.CurrentPrincipal = userPrincipal;

                        HttpContext.Session["userId"] = user.UserId;

                        return RedirectToLocal(returnUrl);
                    }
                }

            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ViewResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegistration model)
        {
            if (ModelState.IsValid)
            {
                using (PYPContext db = new PYPContext())
                {
                    User user = new User();
                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.Salt = Crypto.GenerateSalt();
                    user.Password = Crypto.HashPassword(model.Password + user.Salt);

                    // Save the new user to the database
                    db.Users.Add(user);
                    db.SaveChanges();

                    // Login the new user
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(Response.Cookies.Get(FormsAuthentication.FormsCookieName).Value);
                    GenericPrincipal userPrincipal = new GenericPrincipal(new FormsIdentity(ticket), null);
                    System.Web.HttpContext.Current.User = userPrincipal;
                    Thread.CurrentPrincipal = userPrincipal;
                }
                
                // Redirect to Home
                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public ActionResult Manage()
        {
            UserAccount account = new UserAccount();

            using (PYPContext db = new PYPContext())
            {
                User user = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                account.Name = user.Name;
                account.UserName = user.UserName;
                account.Email = user.Email;
            }

            return View(account);
        }

        [AllowAnonymous]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult IsUniqueUserName(string username)
        {
            //return false;

            using (PYPContext db = new PYPContext())
            {
                return db.Users.Any(u => u.UserName == username) ? Json(ErrorCodeToString(MembershipCreateStatus.DuplicateUserName), JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
