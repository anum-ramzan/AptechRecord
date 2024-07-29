using AptechRecord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechRecord.Controllers
{
    [Authorize]
    public class ManagementController : Controller
    {
        AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: Management
        public ActionResult Index()
        {
            try {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                else
                {
                    ViewBag.CoursesCount = db.Courses.Where(c => c.CourseStatus == "Current").ToList().Count;
                    ViewBag.BatchesCount = db.Batches.Where(b => b.BatchStatus == "In Progress").ToList().Count;
                    ViewBag.StudentsCount = db.Students.Where(s => s.Status == "Enrolled").ToList().Count;
                    ViewBag.FacultiesCount = db.Employees.Where(e => e.EmployeeStatus == "Active").ToList().Count;

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
                    DateTime? date = Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString());

                    //counters
                    ViewBag.TotalBatch = db.usp_select_total_batch(passStringToDD).ToList().Count;
                    ViewBag.TotalMarkedBatch = db.usp_select_total_marked_batch(date, passStringToDD).ToList().Count;
                    ViewBag.TotalUnmarkedBatch = db.usp_select_total_unmarked_batch(date, passStringToDD).ToList().Count;

                    ViewBag.TotalStudent = db.usp_select_total_student(passStringToDD).ToList().Count;
                    ViewBag.TotalPresentStudent = db.usp_select_total_student_present(passStringToDD, date).ToList().Count;
                    ViewBag.TotalAbsentStudent = db.usp_select_total_student_absent(passStringToDD, date).ToList().Count;
                }
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
            }
            return View();
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
                    return RedirectToAction("Index", "Management");
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
    }
}