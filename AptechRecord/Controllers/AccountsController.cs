using AptechRecord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AptechRecord.Controllers
{
    [AllowAnonymous]
    public class AccountsController : Controller
    {
        //entity model object 
        AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: Accounts/Maintenance
        public ActionResult Maintenance()
        {
            return View();
        }


        // GET: Accounts/Login
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }


        //Post: Accounts/Login
        [HttpPost]
        public ActionResult Login(User user, string ReturnUrl)
        {
            try
            {
                DateTime? datetime = Convert.ToDateTime(DateTime.Now).AddHours(9.00000);
                usp_login_Result result = db.usp_login(user.Username, user.Password).SingleOrDefault();
                if (result != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);

                    Session["UserId"] = result.Id;
                    Session["Username"] = result.Username;
                    Session["UserRole"] = result.UserRole;
                    Session["Name"] = result.Name;
                    db.usp_login_history(Convert.ToInt32(db.usp_auto_loginhistory().Single()), result.Id, datetime);
                    if (result.UserRole == 1)
                    {
                        //if (!string.IsNullOrEmpty(ReturnUrl))
                        //{
                        //    return RedirectToAction(HttpUtility.UrlDecode("http://localhost:6976" + ReturnUrl));
                        //}
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (result.UserRole == 2)
                    {
                        //if (!string.IsNullOrEmpty(ReturnUrl))
                        //{
                        //    return RedirectToAction(HttpUtility.UrlDecode("http://localhost:6976" + ReturnUrl));
                        //}
                        return RedirectToAction("Index", "Management");
                    }
                    else if (result.UserRole == 3)
                    {
                        //if (!string.IsNullOrEmpty(ReturnUrl))
                        //{
                        //    return RedirectToAction(HttpUtility.UrlDecode("http://localhost:6976" + ReturnUrl));
                        //}
                        return RedirectToAction("Index", "Admission");
                    }
                    else if (result.UserRole == 4)
                    {
                        //if (!string.IsNullOrEmpty(ReturnUrl))
                        //{
                        //    return RedirectToAction(HttpUtility.UrlDecode("http://localhost:6976" + ReturnUrl));
                        //}
                        return RedirectToAction("Index", "Coordinator");
                    }
                    else if (result.UserRole == 5)
                    {
                        //if (!string.IsNullOrEmpty(ReturnUrl))
                        //{
                        //    return RedirectToAction(HttpUtility.UrlDecode("http://localhost:6976" + ReturnUrl));
                        //}
                        return RedirectToAction("Dashboard", "RecoveryOfficer");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    ViewBag.InvalidUser = "Invalid username or password or user does not exist.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Login", "Accounts");
        }



    }
}