using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iforgot.Models;

namespace iforgot.Classes
{
    public class iforgotInsert
    {
        public static _Database db = new _Database();
        public static void pwdResetInsert(string pwdReset_emailVar, string pwdReset_selectorVar, string pwdReset_tokenVar, int pwdReset_expiresVar)
        {

            var pwd = new pwdReset //Make sure you have a table called test in DB
            {
                pwdReset_email = pwdReset_emailVar,
                pwdReset_selector = pwdReset_selectorVar,
                pwdReset_token = pwdReset_tokenVar,
                pwdReset_expires = pwdReset_expiresVar,

            };

            db.pwdReset.Add(pwd);
            db.SaveChanges();
        }


        public static void pwdResetDelete(string pwdReset_emailVar)
        {
            db.pwdReset.RemoveRange(db.pwdReset.Where(x => x.pwdReset_email == pwdReset_emailVar));
            db.SaveChanges();
        }




    }
}