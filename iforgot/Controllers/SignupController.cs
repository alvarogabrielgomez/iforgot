using DevOne.Security.Cryptography.BCrypt;
using iforgot.Classes;
using iforgot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace iforgot.Controllers
{
    public class SignupController : Controller
    {
        public static _Database db = new _Database();
        // GET: Signup
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(newUsers newUsers)
        {
            // Generate a token for verification
            var token = randomBytes.GenerateRandomBytes(32);

            // Retrieve data from post method
            string newUser_name = newUsers.newUser_name;
            string newUser_email = newUsers.newUser_email;
            string newUser_pwd = newUsers.newUser_pwd;
            string newUser_pwdRepeat = newUsers.newUser_pwdRepeat;

            try
            {
                if (newUser_pwd == null || newUser_pwdRepeat == null)
                {
                    // Verify if boxes are empty
                    throw new PasswordException();
                }
                else if (newUser_pwd != newUser_pwdRepeat)
                {
                    // Verify if pwd is the same on both boxes
                    throw new PasswordException();
                }
                // Verify if user exists
                var areUser = db.users.Where(x => x.user_email == newUser_email).FirstOrDefault();

                //If NOT exists, continue
                if (areUser == null)
                {
                    // Making Hashed Password for security with BCrypt
                    var hashedpwd = BCryptHelper.HashPassword(newUser_pwd, BCryptHelper.GenerateSalt(7));

                    // Insert a new User on Users table with INACTIVE state
                    userInsert(newUser_email, newUser_name, hashedpwd);

                    // Making HashedToken for security with BCrypt
                    string hashedToken = BCryptHelper.HashPassword(token, BCryptHelper.GenerateSalt(7));
                    // Time now on seconds (UNIX TIME)
                    var dateU = Convert.ToInt32(DateTimeOffset.Now.ToUnixTimeSeconds());
                    // Expire date from now to 30 min 
                    int newUser_expires = dateU + 1800;
                    // Delete any data from database before now
                    newUserDelete(newUser_email);
                    // Inserting new data for recovery with expiration time setted on 30 min
                    newUserInsert(newUser_email, hashedToken, newUser_expires);
                    // Construct URL
                    string url = "localhost:62403/signup/continue?validator=" + token+"&email="+newUser_email;

                    string message = url;

                    bool sending = sendEmail(newUser_email, message, "Confirm your registration");

                    if (sending)
                    {
                        return RedirectToAction("confirmationPending", "Signup");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Signup", new { error = "error" });
                    }

                }
                else if (areUser != null)
                {
                    // If Exists
                    new UserExistsException();
                }
            }
            catch (UserExistsException)
            {
                return RedirectToAction("Index", "Signup", new { error = "userexists" });
            }

            return RedirectToAction("Index", "Signup");
        }
        [HttpGet]
        public ActionResult Continue(string validator, string email)
        {
            // Time now on seconds (UNIX TIME)
            var dateU = Convert.ToInt32(DateTimeOffset.Now.ToUnixTimeSeconds());

            // Searching the user on newUsers table using the email and verifying if the expiration time are passed
            var resultToken = db.newUsers.Where(x => x.newUser_email == email && x.newUser_expires >= dateU).FirstOrDefault();

            // Checking the validator
            bool tokenCheck = BCryptHelper.CheckPassword(validator, resultToken.newUser_token);

            if (tokenCheck == false)
            {
                // If NOT valid
                throw new TokenOrValidationException();

            }
            else if (tokenCheck == true)
            {
                // If is valid
                // Searching the user on users table with the email and activate
                var resultUser = db.users.Where(x => x.user_email == resultToken.newUser_email).FirstOrDefault();
                resultUser.active = true;
                db.newUsers.RemoveRange(db.newUsers.Where(x => x.newUser_email == resultUser.user_email));
                db.SaveChanges();
                ViewBag.message = "Done";
                return View();
            }

            return View();
        }

        public ActionResult confirmationPending()
        {
            return View();
        }

        public static void newUserInsert(string newUser_emailVar, string newUser_tokenVar, int newUser_expiresVar)
        {

            var pwd = new newUsers //Make sure you have a table called test in DB
            {
                newUser_email = newUser_emailVar,
                newUser_token = newUser_tokenVar,
                newUser_expires = newUser_expiresVar,

            };

            db.newUsers.Add(pwd);
            db.SaveChanges();
        }

        public static void userInsert(string user_emailVar, string user_name, string user_pwd)
        {

            var pwd = new users //Make sure you have a table called test in DB
            {
                user_email = user_emailVar,
                user_name = user_name,
                user_pwd = user_pwd,
                active = false,

            };

            db.users.Add(pwd);
            db.SaveChanges();
        }

        public static void newUserDelete(string newUser_emailVar)
        {
            db.newUsers.RemoveRange(db.newUsers.Where(x => x.newUser_email == newUser_emailVar));
            db.SaveChanges();
        }

        public bool sendEmail(string newUser_emailVar, string messageVar, string subVar)
        {
            try
            {
                // Sending URL to user 
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("email@gmail.com", "TEST");
                    var receiverEmail = new MailAddress(newUser_emailVar, "Receiver");
                    var password = "123123123"; ;
                    var body = messageVar;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subVar,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                        return true;
                    }

                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public class TokenOrValidationException : Exception
        {
            public TokenOrValidationException()
            {
            }

            public TokenOrValidationException(string message)
                : base(message)
            {
            }

            public TokenOrValidationException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }

        public class UserExistsException : Exception
        {
            public UserExistsException()
            {
            }

            public UserExistsException(string message)
                : base(message)
            {
            }

            public UserExistsException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }

        public class PasswordException : Exception
        {
            public PasswordException()
            {
            }

            public PasswordException(string message)
                : base(message)
            {
            }

            public PasswordException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }
    }
}