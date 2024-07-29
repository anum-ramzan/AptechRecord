using AptechRecord.Models;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechRecord.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: Admin
        public ActionResult Dashboard()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                if (Session["UserRole"].ToString() == "2")
                {
                    return RedirectToAction("Login", "Accounts");
                }

                ViewBag.CoursesCount = db.Courses.Where(c => c.CourseStatus == "Current").ToList().Count;
                ViewBag.BatchesCount = db.Batches.Where(b => b.BatchStatus == "In Progress").ToList().Count;
                ViewBag.StudentsCount = db.Students.Where(s => s.Status == "Enrolled").ToList().Count;
                ViewBag.FacultiesCount = db.Employees.Where(e => e.EmployeeStatus == "Active").ToList().Count;

                MWFDate();
                TTSDate();
            }

            return View();
        }

        public JsonResult MWFGroupingList()
        {
            return Json(db.usp_select_MWFGrouping().ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult TTSGroupingList()
        {
            return Json(db.usp_select_TTSGrouping().ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AttendanceBarChart()
        {
            return Json(db.usp_select_attendance_barchart(DateTime.Now.AddHours(9.00000)).ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult PasswordChange()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            int id = Convert.ToInt32(Session["UserId"].ToString());
            User user = db.Users.Find(id);

            return View(user);
        }

        [HttpPost]
        public ActionResult PasswordChange([Bind(Exclude = "Id,Username,UserRole,Userstatus,UserAddUserdOn")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.usp_update_user_password(user.Password, Convert.ToInt32(Session["UserId"].ToString()));
                    TempData["SuccessMsg"] = "Password changed successfully.";
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    int id = Convert.ToInt32(Session["UserId"].ToString());
                    User users = db.Users.Find(id);
                    return View(users);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }

        }

        public JsonResult BarChartForMwf()
        {
            string days = DateTime.Now.AddHours(9.00000).ToString("dddd");
            DateTime date = DateTime.Now.AddHours(9.00000);

            DateTime? datetime = null;
            string dateDay = null;

            #region Selecting Dates
            switch (days)
            {
                case "Monday":
                    dateDay = "MWF";
                    datetime = date.AddDays(-3.000);//friday
                    ViewBag.MWFDate = date.AddDays(-3.000).ToString("d");
                    break;
                case "Tuesday":
                    datetime = date.AddDays(-1.000); //monday
                    ViewBag.MWFDate = date.AddDays(-1.000).ToString("d");
                    dateDay = "MWF";
                    break;
                case "Wednesday":
                    datetime = date.AddDays(-2.000); //monday
                    ViewBag.MWFDate = date.AddDays(-2.000).ToString("d");
                    dateDay = "MWF";
                    break;
                case "Thursday":
                    datetime = date.AddDays(-1.000); //wednesday
                    ViewBag.MWFDate = date.AddDays(-1.000).ToString("d");
                    dateDay = "MWF";
                    break;
                case "Friday":
                    datetime = date.AddDays(-2.000); //wednesday
                    ViewBag.MWFDate = date.AddDays(-2.000).ToString("d");
                    dateDay = "MWF";
                    break;
                case "Saturday":
                    datetime = date.AddDays(-1.000); //friday
                    ViewBag.MWFDate = date.AddDays(-1.000).ToString("d");
                    dateDay = "MWF";
                    break;
            }
            #endregion

            var twoDays = db.usp_select_bar_chart(dateDay, datetime).ToList();
            return Json(twoDays, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BarChartForTts()
        {
            string days = DateTime.Now.AddHours(9.00000).ToString("dddd");
            DateTime date = DateTime.Now.AddHours(9.00000);

            DateTime? datetime = null;
            string dateDay = null;

            #region Selecting Dates
            switch (days)
            {
                case "Monday":
                    datetime = date.AddDays(-2.000);//saturday
                    ViewBag.TTSDate = date.AddDays(-2.000).ToString("d");
                    dateDay = "TTS";
                    break;
                case "Tuesday":
                    datetime = date.AddDays(-3.000); //saturday
                    ViewBag.TTSDate = date.AddDays(-3.000).ToString("d");
                    dateDay = "TTS";
                    break;
                case "Wednesday":
                    datetime = date.AddDays(-1.000); //tuesday
                    ViewBag.TTSDate = date.AddDays(-1.000).ToString("d");
                    dateDay = "TTS";
                    break;
                case "Thursday":
                    datetime = date.AddDays(-2.000); //tuesday
                    ViewBag.TTSDate = date.AddDays(-2.000).ToString("d");
                    dateDay = "TTS";
                    break;
                case "Friday":
                    datetime = date.AddDays(-1.000); //thursday
                    ViewBag.TTSDate = date.AddDays(-1.000).ToString("d");
                    dateDay = "TTS";
                    break;
                case "Saturday":
                    datetime = date.AddDays(-2.000); //thursday
                    ViewBag.TTSDate = date.AddDays(-2.000).ToString("d");
                    dateDay = "TTS";
                    break;
            }
            #endregion

            var twoDays = db.usp_select_bar_chart(dateDay, datetime).ToList();
            return Json(twoDays, JsonRequestBehavior.AllowGet);
        }

        private void TTSDate()
        {
            string days = DateTime.Now.AddHours(9.00000).ToString("dddd");
            DateTime date = DateTime.Now.AddHours(9.00000);

            #region Selecting Dates
            switch (days)
            {
                case "Monday":
                    ViewBag.TTSDate = date.AddDays(-2.000).ToString("d");
                    break;
                case "Tuesday":
                    ViewBag.TTSDate = date.AddDays(-3.000).ToString("d");
                    break;
                case "Wednesday":
                    ViewBag.TTSDate = date.AddDays(-1.000).ToString("d");
                    break;
                case "Thursday":
                    ViewBag.TTSDate = date.AddDays(-2.000).ToString("d");
                    break;
                case "Friday":
                    ViewBag.TTSDate = date.AddDays(-1.000).ToString("d");
                    break;
                case "Saturday":
                    ViewBag.TTSDate = date.AddDays(-2.000).ToString("d");
                    break;
            }
            #endregion

        }

        private void MWFDate()
        {
            string days = DateTime.Now.AddHours(9.00000).ToString("dddd");
            DateTime date = DateTime.Now.AddHours(9.00000);

            #region Selecting Dates
            switch (days)
            {
                case "Monday":
                    ViewBag.MWFDate = date.AddDays(-3.000).ToString("d");
                    break;
                case "Tuesday":
                    ViewBag.MWFDate = date.AddDays(-1.000).ToString("d");
                    break;
                case "Wednesday":
                    ViewBag.MWFDate = date.AddDays(-2.000).ToString("d");
                    break;
                case "Thursday":
                    ViewBag.MWFDate = date.AddDays(-1.000).ToString("d");
                    break;
                case "Friday":
                    ViewBag.MWFDate = date.AddDays(-2.000).ToString("d");
                    break;
                case "Saturday":
                    ViewBag.MWFDate = date.AddDays(-1.000).ToString("d");
                    break;
            }
            #endregion

        }

    }
}