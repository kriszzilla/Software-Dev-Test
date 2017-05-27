using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLogin.Models;

namespace WebLogin.Controllers
{
    public class HomeController : Controller
    {
        private UsersDBEntities _db = new UsersDBEntities();

        public ActionResult Index()
        {
            if (Session["Username"] == null)
            {
                ViewBag.PassSession = "0";
            }
            else
            {
                ViewBag.PassSession = "1";
            }
            return View();
        }

        #region Ajax Post
        public PartialViewResult UserLogin(string email, string password)
        {
           int CheckUser = 0;

            var mostRecentEntries =
                   from UsersList in _db.UserCredentials
                   where UsersList.emailAddress.Equals(email) && UsersList.password.Equals(password)
                    select UsersList;

            CheckUser = mostRecentEntries.Count();

            if (CheckUser == 0)
            {
                ViewBag.Message = "Failed, please try again.";             
                Session["Username"] = null;
                ViewBag.PassSession = "0";
                return PartialView("UserLogin");
            }
            else {
                ViewBag.Message = "Welcome, " + email;
                Session["Username"] = email;
                ViewBag.PassSession = "1";
                return PartialView("UserLogin");
            }

            
        }
        #endregion

        public ActionResult About()
        {
            ViewBag.Message = "This is a test application.";

            if (Session["Username"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Here's my basic info.";

            if (Session["Username"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(UserCredential obj)

        {
            if (ModelState.IsValid)
            {
                try
                {
                    UsersDBEntities db = new UsersDBEntities();
                    db.UserCredentials.Add(obj);
                    db.SaveChanges();

                }
                catch (Exception err)
                {
                    ViewBag.Message = "The email address is already been taken.";
                    return View(obj);
                }
            }

            
            return View();
        }

    }
}