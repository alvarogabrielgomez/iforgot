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
    public class iforgotController : Controller
    {
        public static _Database db = new _Database();
        public ActionResult confirmedNewPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult resetedPassword(pwdReset pwdReset)
        {
            // Generate a selector and token for verification
            var selector = randomBytes.GenerateRandomBytes(8);
            var token = randomBytes.GenerateRandomBytes(32);

            // Retrieve data from post method
            int pwdReset_id = pwdReset.pwdReset_id;
            string pwdReset_email = pwdReset.pwdReset_email;
            string pwdReset_selector = pwdReset.pwdReset_selector;
            string pwdReset_token = pwdReset.pwdReset_token;
            int pwdReset_expires = pwdReset.pwdReset_expires;

            var areUser = db.users.Where(x => x.user_email == pwdReset_email).FirstOrDefault();

            // Verify if user exists
            if (areUser != null)
            {
                // Making HashedToken for security with BCrypt
                string hashedToken = BCryptHelper.HashPassword(token, BCryptHelper.GenerateSalt(7));

                pwdReset_selector = selector;
                pwdReset_token = token;

                // Construct URL
                string url = "localhost:62403/iforgot/createNewPassword?selector=" + pwdReset_selector + "&validator=" + pwdReset_token;

                // Time now on seconds (UNIX TIME)
                var dateU = Convert.ToInt32(DateTimeOffset.Now.ToUnixTimeSeconds());

                // Expire date from now to 30 min 
                pwdReset_expires = dateU + 1800;

                // Delete any data from database before now
                iforgotInsert.pwdResetDelete(pwdReset_email);
                // Inserting new data for recovery with expiration time setted on 30 min
                iforgotInsert.pwdResetInsert(pwdReset_email, pwdReset_selector, hashedToken, pwdReset_expires);

                // Message for the Email
                string message = url;

                try
                {
                    // Sending URL to user 
                    if (ModelState.IsValid)
                    {
                        var senderEmail = new MailAddress("email@gmail.com", "TEST");
                        var receiverEmail = new MailAddress(pwdReset_email, "Receiver");
                        var password = "123123213";
                        var sub = "Test";
                        var body = message;
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
                            Subject = sub,
                            Body = body
                        })
                        {
                            smtp.Send(mess);
                        }
                        return View();
                    }
                }
                catch (Exception e)
                {

                    ViewBag.Error = e;
                    return View();
                }
            }
            else if (areUser == null)
            {
                // Are not user
                return RedirectToAction("Index", "Home"); 
            }
            return View();
        }

        public ActionResult resetPassword()
        {
            ViewBag.Message = "Reset your password";

            return View();
        }


        [HttpGet]
        public ActionResult createNewPassword(string selector, string validator)
        {
            if (selector != null && validator != null)
            {
                // Setting a hidden input box on view, with the selector and validator, using the data from GET
                ViewBag.selector = selector;
                ViewBag.validator = validator;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult createNewPassword()
        {
            // Cant be access normaly
            return RedirectToAction("Index", "Home");
        }
        public ActionResult createNewPassword(string error)
        {
            ViewBag.Title = error;
            return View();
        }

        [HttpPost]
        public ActionResult confirmedNewPass(pwdReset pwdReset)
        {
            // Retrieve data from post method after create new password
            string pwdReset_selector = pwdReset.pwdReset_selector;
            string pwdReset_validator = pwdReset.pwdReset_token;
            string pwdReset_pwd = pwdReset.pwdReset_pwd;
            string pwdReset_pwdRepeat = pwdReset.pwdReset_pwdRepeat;

            try
            {
                if (pwdReset_pwd == null || pwdReset_pwdRepeat == null)
                {
                    // Verify if boxes are empty
                    // return RedirectToAction("createNewPassword", "Home", "Test");
                    throw new NullReferenceException();
                }
                else if (pwdReset_pwd != pwdReset_pwdRepeat)
                {
                    // Verify if pwd is the same on both boxes
                    throw new PasswordException();
                }

                // Time now on seconds (UNIX TIME)
                var dateU = Convert.ToInt32(DateTimeOffset.Now.ToUnixTimeSeconds());

                // Searching the user on pwdReset table using the selector and verifying if the expiration time are passed
                var resultToken = db.pwdReset.Where(x => x.pwdReset_selector == pwdReset_selector && x.pwdReset_expires >= dateU).FirstOrDefault();

                // Checking the validator
                bool tokenCheck = BCryptHelper.CheckPassword(pwdReset_validator, resultToken.pwdReset_token);

                if (tokenCheck == false)
                {
                    // If NOT valid
                    throw new TokenOrValidationException();
                    
                }
                else if (tokenCheck == true)
                {
                    // If is valid

                    // Searching the user on users table with the email of pwdReset_email
                    var resultUser = db.users.Where(x => x.user_email == resultToken.pwdReset_email).FirstOrDefault();

                    if (resultUser != null)
                    {
                        // Hashing the new pwd with BCrypt and updating it 
                        var hashedPwd = BCryptHelper.HashPassword(pwdReset_pwd, BCryptHelper.GenerateSalt(7));
                        resultUser.user_pwd = hashedPwd;

                        // Cleaning pwdReset Table
                        db.pwdReset.RemoveRange(db.pwdReset.Where(x => x.pwdReset_selector == pwdReset_selector));
                        db.SaveChanges();

                        // Cleaning variables
                        pwdReset_pwd = null;
                        pwdReset_pwdRepeat = null;

                        return View();
                    }
                    else if (resultUser == null)
                    {
                        // If are any error from select
                        throw new NullReferenceException();
                    }
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Index", "Home", new { error = "error" });
            }

            catch (TokenOrValidationException)
            {
                return RedirectToAction("Index", "Home", new { error = "badtoken" });
            }

            catch (PasswordException)
            {
                return RedirectToAction("Index", "Home", new { error = "badpwd" });
            }

            catch (Exception e)
            {

                // If any unknown error
                return RedirectToAction("Index", "Home");
            }
            return View();

        }




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