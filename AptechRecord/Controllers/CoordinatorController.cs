using AptechRecord.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AptechRecord.Controllers
{
    [Authorize]
    public class CoordinatorController : Controller
    {
        AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: Coordinator
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                ViewBag.CoursesCount = db.Courses.ToList().Count;
                ViewBag.BatchesCount = db.Batches.ToList().Count;
                ViewBag.StudentsCount = db.Students.ToList().Count;
                ViewBag.FacultiesCount = db.Employees.ToList().Count;

            }

            return View();
        }
    }
}