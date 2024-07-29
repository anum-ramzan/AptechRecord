using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;//y312aa
using System.Net;
using System.Web;
using System.Web.Mvc;
using AptechRecord.Models;

namespace AptechRecord.Controllers
{
    [Authorize]
    public class SemestersController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: Semesters
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var semesters = db.Semesters.Include(s => s.CourseDetail).Include(c=>c.CourseDetail.Cours);
            return View(semesters.ToList());
        }

        // GET: Semesters/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // GET: Semesters/AddSemester
        public ActionResult AddSemester()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Course = new SelectList(db.Courses, "Id", "CourseName");
            return View();
        }

        // POST: Semesters/AddSemester
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSemester([Bind(Include = "Id,SemesterName,CourseId,SemesterSession")] Semester semester)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                db.Semesters.Add(semester);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Course = new SelectList(db.Courses, "Id", "CourseName"); 
            return View(semester);
        }

        // GET: Semesters/EditSemester/5
        public ActionResult EditSemester(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.CourseDetails, "CourseDetail1", "CourseDetailName", semester.CourseId);
            return View(semester);
        }

        // POST: Semesters/EditSemester/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSemester([Bind(Include = "Id,SemesterName,CourseId,SemesterSession")] Semester semester)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                db.Entry(semester).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.CourseDetails, "CourseDetail1", "CourseDetailName", semester.CourseId);
            return View(semester);
        }

        // GET: Semesters/DeleteSemester/5
        public ActionResult DeleteSemester(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // POST: Semesters/DeleteSemester/5
        [HttpPost, ActionName("DeleteSemester")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSemesterConfirmed(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            Semester semester = db.Semesters.Find(id);
            db.Semesters.Remove(semester);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        //Semester drop down proper
        public JsonResult GeCourseDetailList(int id)
        {
            try
            {
                var cd = db.CourseDetails.Where(s => s.Cours.Id == id).Select(c => new { c.CourseDetail1, c.CourseDetailName }).ToList();
                return Json(cd, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return Json("Something went wrong. Try later.", JsonRequestBehavior.AllowGet);
            }
        }

        //Semester drop down proper
        public JsonResult GeCourseSemester(int id)
        {
            try
            {
                var semester = db.Semesters.Where(s => s.CourseId == id);
                return Json(semester, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return Json("Something went wrong. Try later.", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
