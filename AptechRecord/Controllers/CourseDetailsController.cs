using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AptechRecord.Models;

namespace AptechRecord.Controllers
{
    [Authorize]
    public class CourseDetailsController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: CourseDetails
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var courseDetails = db.CourseDetails.Include(c => c.Cours).OrderBy(x => x.Cours.CourseName).ThenBy(x=>x.CourseDetail1);
            return View(courseDetails.ToList());
        }

        // GET: CourseDetails/Create
        public ActionResult AddCourseDetails()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.CourseDetailTree = new SelectList(db.Courses, "Id", "CourseName");
            return View();
        }

        // POST: CourseDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCourseDetails([Bind(Include = "CourseDetail1,CourseDetailName,CourseDetailTree")] CourseDetail courseDetail)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                db.CourseDetails.Add(courseDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseDetailTree = new SelectList(db.Courses, "Id", "CourseName", courseDetail.CourseDetailTree);
            return View(courseDetail);
        }

        // GET: CourseDetails/Edit/5
        public ActionResult EditCourseDetails(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            if (courseDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseDetailTree = new SelectList(db.Courses, "Id", "CourseName", courseDetail.CourseDetailTree);
            return View(courseDetail);
        }

        // POST: CourseDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourseDetails([Bind(Include = "CourseDetail1,CourseDetailName,CourseDetailTree")] CourseDetail courseDetail)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                db.Entry(courseDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseDetailTree = new SelectList(db.Courses, "Id", "CourseName", courseDetail.CourseDetailTree);
            return View(courseDetail);
        }

        // GET: CourseDetails/Delete/5
        public ActionResult DeleteCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            if (courseDetail == null)
            {
                return HttpNotFound();
            }
            return View(courseDetail);
        }

        // POST: CourseDetails/Delete/5
        [HttpPost, ActionName("DeleteCourse")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            db.CourseDetails.Remove(courseDetail);
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
    }
}
