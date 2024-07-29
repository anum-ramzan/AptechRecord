using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AptechRecord.Models;
using System.Dynamic;

namespace AptechRecord.Controllers
{
    [Authorize]
    public class AttendancesController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();


        public ActionResult Customized()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }


                ViewBag.Slots = db.TimeSlots.ToList();
                ViewBag.Days = db.Days.ToList();
                ViewBag.Batch = db.Batches.Where(b => b.BatchStatus == "In Progress").ToList();

                DateTime? date1 = Convert.ToDateTime(DateTime.Now.ToString()).AddHours(9.00000);
                DateTime? date2 = Convert.ToDateTime(DateTime.Now.ToString()).AddHours(9.00000);

                var list = db.usp_select_customized_attendance(date1, date2).ToList();


                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        [HttpPost]
        public ActionResult Customized(DateTime FromCDate, DateTime ToCDate)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }

                if (FromCDate == null)
                {
                    ViewBag.Mesg = "Select from date";
                }
                else if (ToCDate == null)
                {
                    ViewBag.Mesg = "Select to date";
                }
                else if (FromCDate == null || ToCDate == null)
                {
                    ViewBag.Mesg = "Select date";
                }

                ViewBag.Slots = db.TimeSlots.ToList();
                ViewBag.Days = db.Days.ToList();
                ViewBag.Batch = db.Batches.Where(b => b.BatchStatus == "In Progress").ToList();

                DateTime? date1 = Convert.ToDateTime(FromCDate).AddHours(9.00000);
                DateTime? date2 = Convert.ToDateTime(ToCDate).AddHours(9.00000);

                var list = db.usp_select_customized_attendance(date1, date2).ToList();

                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        // GET: Attendances
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var attendances = db.Attendances.Include(a => a.User).Include(a => a.Student).Include(b => b.Student.StudentBatches);
            return View(attendances.ToList());
        }

        public ActionResult SlotWiseAttendance()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var attendances = db.TimeSlots.ToList();
            ViewBag.SlotDropDown = new SelectList(attendances, "Id", "Slot");

            return View();
        }

        [HttpPost]
        public ActionResult SlotWiseAttendance(int? id)
        {
            var attendances = db.TimeSlots.ToList();
            ViewBag.SlotDropDown = new SelectList(attendances, "Id", "Slot");
            int? did = Convert.ToInt32(Request.Form["SlotDropDown"].ToString());
            return RedirectToAction("GetSlotBatches", "Attendances", new { id = did });
        }

        public ActionResult GetSlotBatches(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                return RedirectToAction("SlotWiseAttendance", "Attendances");
            }

            Session["sid"] = id;
            ViewBag.Timing = db.TimeSlots.Where(s => s.Id == id).Select(s => s.Slot).Single();
            var batch = db.Batches.Where(t => t.BatchTiming == id && t.BatchStatus == "In Progress").ToList();
            return View(batch);
        }

        public ActionResult StudentAttendance(string batch)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (batch == null)
            {

                return RedirectToAction("GetSlotBatches", "Attendances", new { id = Session["sid"].ToString() });
            }

            ViewBag.Batch = db.Batches.Where(s => s.BatchCode == batch).Select(s => s.BatchCode).Single();
            var students = db.usp_select_student_attendance(batch).ToList();

            return View(students);
        }

        public ActionResult TodaysReport()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }

                DateTime? datetime = Convert.ToDateTime(DateTime.Now.ToString()).AddHours(9.00000);
                ViewBag.date = datetime;

                return View(db.usp_select_todays_attendance(datetime).ToList());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult TwoDaysReport()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }

                #region switch Day

                string days = DateTime.Now.AddHours(9.00000).ToString("dddd");
                DateTime date = DateTime.Now.AddHours(9.00000);

                DateTime? datetime = null;
                DateTime? datetime1 = null;

                switch (days)
                {
                    case "Monday":
                        datetime = date.AddDays(-2.000);//saturday
                        datetime1 = date.AddDays(-3.000);//friday
                        break;
                    case "Tuesday":
                        datetime = date.AddDays(-1.000); //monday
                        datetime1 = date.AddDays(-3.000); //saturday
                        break;
                    case "Wednesday":
                        datetime = date.AddDays(-1.000); //tuesday
                        datetime1 = date.AddDays(-2.000); //monday
                        break;
                    case "Thursday":
                        datetime = date.AddDays(-1.000); //wednesday
                        datetime1 = date.AddDays(-2.000); //tuesday
                        break;
                    case "Friday":
                        datetime = date.AddDays(-1.000); //thursday
                        datetime1 = date.AddDays(-2.000); //wednesday
                        break;
                    case "Saturday":
                        datetime = date.AddDays(-1.000); //friday
                        datetime1 = date.AddDays(-2.000); //thursday
                        break;
                }

                var twoDays = db.usp_select_two_attendance(datetime1, datetime).ToList();
                return View(twoDays);
                #endregion
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult WeeklyReport()
        {
            return View();
        }

        public ActionResult MonthlyReport()
        {
            return View();
        }

        public ActionResult UnMarkBatches()
        {
            try
            {

                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }

                #region switch Day

                string days = DateTime.Now.AddHours(9.00000).ToString("dddd");
                string passStringToDD = null;
                switch (days)
                {
                    case "Monday":
                        passStringToDD = "MWF";
                        break;
                    case "Wednesday":
                        passStringToDD = "MWF";
                        break;
                    case "Friday":
                        passStringToDD = "MWF";
                        break;
                    case "Tuesday":
                        passStringToDD = "TTS";
                        break;
                    case "Thursday":
                        passStringToDD = "TTS";
                        break;
                    case "Saturday":
                        passStringToDD = "TTS";
                        break;
                }

                #endregion

                ViewBag.GetDate = DateTime.Now.AddHours(9.00000).ToString("d");

                DateTime date = DateTime.Now.AddHours(9.00000);
                var list = db.usp_select_unmarked_attendance(passStringToDD, date).ToList();

                return View(list);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        [HttpPost]
        public ActionResult UnMarkBatches(DateTime CustomDate)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (CustomDate == null)
                {
                    ViewBag.Mesg = "Select date";
                    return View();
                }

                #region switch Day

                string days = CustomDate.ToString("dddd");
                string passStringToDD = null;
                switch (days)
                {
                    case "Monday":
                        passStringToDD = "MWF";
                        break;
                    case "Wednesday":
                        passStringToDD = "MWF";
                        break;
                    case "Friday":
                        passStringToDD = "MWF";
                        break;
                    case "Tuesday":
                        passStringToDD = "TTS";
                        break;
                    case "Thursday":
                        passStringToDD = "TTS";
                        break;
                    case "Saturday":
                        passStringToDD = "TTS";
                        break;
                }
                #endregion

                ViewBag.GetDate = CustomDate;

                DateTime date = DateTime.Now.AddHours(9.00000);
                var list = db.usp_select_unmarked_attendance(passStringToDD, CustomDate).ToList();

                return View(list);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult DateWise()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DateWise(DateTime Date1, DateTime Date2)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (Date1 == null || Date2 == null)
                {
                    ViewBag.Mesg = "Please select date";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
            return View();
        }

        public ActionResult FacultyWiseAttendance()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                #region switch Day

                string days = DateTime.Now.AddHours(9.00000).ToString("dddd");
                switch (days)
                {
                    case "Monday":
                        @ViewBag.Days = "MWF";
                        break;
                    case "Wednesday":
                        @ViewBag.Days = "MWF";
                        break;
                    case "Friday":
                        @ViewBag.Days = "MWF";
                        break;
                    case "Tuesday":
                        @ViewBag.Days = "TTS";
                        break;
                    case "Thursday":
                        @ViewBag.Days = "TTS";
                        break;
                    case "Saturday":
                        @ViewBag.Days = "TTS";
                        break;
                }
                #endregion


                var list = db.usp_select_faculty_wise(Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString())).ToList();
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult ConsolidatedAttendance()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                #region switch Day

                string days = DateTime.Now.AddHours(9.00000).ToString("dddd");
                switch (days)
                {
                    case "Monday":
                        @ViewBag.Days = "MWF";
                        break;
                    case "Wednesday":
                        @ViewBag.Days = "MWF";
                        break;
                    case "Friday":
                        @ViewBag.Days = "MWF";
                        break;
                    case "Tuesday":
                        @ViewBag.Days = "TTS";
                        break;
                    case "Thursday":
                        @ViewBag.Days = "TTS";
                        break;
                    case "Saturday":
                        @ViewBag.Days = "TTS";
                        break;
                }
                #endregion

                var list = db.usp_select_consolidated_attendance(Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString())).ToList();
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult DateWiseAttendance()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                DateTime date1 = Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString());
                DateTime date2 = Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString());


                ViewBag.DateF = date1;
                ViewBag.DateT = date2;

                var list = db.usp_select_date_wise_attendance(date1, date2).ToList();
                ViewBag.ShowDate = "Attendance for " + date1.ToString("D");
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        [HttpPost]
        public ActionResult DateWiseAttendance(DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                DateTime date1 = Convert.ToDateTime(DateFrom);
                DateTime date2 = Convert.ToDateTime(DateTo);

                ViewBag.DateF = date1;
                ViewBag.DateT = date2;

                var list = db.usp_select_date_wise_attendance(date1, date2).ToList();
                ViewBag.ShowDate = "Attendance between " + date1.ToString("D") + " till " + date2.ToString("D");
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult ViewBatchAttendance(string BatchCode, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (BatchCode == null)
                {
                    DateTime date1 = Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString());
                    DateTime date2 = Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString());
                    var list = db.usp_select_date_wise_attendance(date1, date2).ToList();
                    ViewBag.ShowDate = "Attendance between " + date1.ToString("D") + " till " + date2.ToString("D");
                    return View("DateWiseAttendance", list);
                }
                if (BatchCode == null && DateFrom == null && DateTo == null)
                {
                    return RedirectToAction("DateWiseAttendance");
                }
                else
                {
                    DateTime date1 = Convert.ToDateTime(DateFrom);
                    DateTime date2 = Convert.ToDateTime(DateTo);
                    var list = db.usp_select_date_wise_batch_detail(BatchCode, DateFrom, DateTo).ToList();
                    ViewBag.BatchCode = BatchCode;
                    ViewBag.ShowDate = "Batch attendance between " + date1.ToString("D") + " till " + date2.ToString("D");
                    return View(list);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult StudentWiseAttendance()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                var list = db.usp_select_student_wise_attendance().ToList();
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult ViewStudentAttendance(int? StudentId)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (StudentId == null)
                {
                    return RedirectToAction("StudentWiseAttendance", "Attendances");
                }
                ViewBag.StudentId = StudentId;
                ViewBag.StudentName = db.Students.Where(s => s.StudentId == StudentId).Select(s => s.StudentName).Single();
                ViewBag.BatchCode = db.StudentBatches.Where(b => b.StudentId == StudentId).Select(b => b.BatchCode).Single();

                var list = db.usp_select_student_wise_attendance_detail(StudentId).ToList();
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult SessionWiseAttendance()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }

                DateTime? date = Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString());
                var list = db.usp_select_session_wise_attendance(date).ToList();
                return View(list);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult TimingWiseAttendance()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }

                DateTime? date = Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString());
                #region switch Day

                string days = DateTime.Now.AddHours(9.00000).ToString("dddd");

                switch (days)
                {
                    case "Monday":
                        days = "MWF";
                        break;
                    case "Tuesday":
                        days = "TTS";
                        break;
                    case "Wednesday":
                        days = "MWF";
                        break;
                    case "Thursday":
                        days = "TTS";
                        break;
                    case "Friday":
                        days = "MWF";
                        break;
                    case "Saturday":
                        days = "TTS";
                        break;
                }
                #endregion

                var list = db.usp_select_timing_wise_attendance(days, date).ToList();
                return View(list);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult ZeroOrNoAttendance()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }

                DateTime? date = Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString());
                #region switch Day

                string days = DateTime.Now.AddHours(9.00000).ToString("dddd");

                switch (days)
                {
                    case "Monday":
                        days = "MWF";
                        break;
                    case "Tuesday":
                        days = "TTS";
                        break;
                    case "Wednesday":
                        days = "MWF";
                        break;
                    case "Thursday":
                        days = "TTS";
                        break;
                    case "Friday":
                        days = "MWF";
                        break;
                    case "Saturday":
                        days = "TTS";
                        break;
                }
                #endregion

                ViewBag.DayName = days;

                var list = db.usp_select_zero_attendance(date, days).Where(a => a.Attendance <= 0).ToList();
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult LowAttendance()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }

                DateTime? date = Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString());
                #region switch Day

                string days = DateTime.Now.AddHours(9.00000).ToString("dddd");

                switch (days)
                {
                    case "Monday":
                        days = "MWF";
                        break;
                    case "Tuesday":
                        days = "TTS";
                        break;
                    case "Wednesday":
                        days = "MWF";
                        break;
                    case "Thursday":
                        days = "TTS";
                        break;
                    case "Friday":
                        days = "MWF";
                        break;
                    case "Saturday":
                        days = "TTS";
                        break;
                }
                #endregion

                ViewBag.DayName = days;

                var list = db.usp_select_zero_attendance(date, days).Where(a => a.Attendance > 0 && a.Attendance < 75).ToList();
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
