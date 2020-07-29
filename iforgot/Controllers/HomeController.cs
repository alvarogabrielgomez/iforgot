using iforgot.Classes;
using iforgot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevOne.Security.Cryptography.BCrypt;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Data.Entity;

namespace iforgot.Controllers
{
    public class HomeController : Controller
    {
        public static _Database db = new _Database();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult confirmedNewPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult About(pwdReset pwdReset)
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }





    }
}