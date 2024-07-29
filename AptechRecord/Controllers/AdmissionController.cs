using AptechRecord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechRecord.Controllers
{
    [Authorize]
    public class AdmissionController : Controller
    {
        AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: Admission
        public ActionResult Index()
        {
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
            }

            return View();
        }
    }
}