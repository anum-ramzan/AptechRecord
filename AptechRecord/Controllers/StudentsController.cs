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
    public class StudentsController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: Students
        public ActionResult Index(string Status)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            IQueryable<Student> student;
            if (Status == "Enrolled")
            {
                student = db.Students.Include(s => s.CourseDetail).Include(s => s.User).Where(s => s.Status == "Enrolled");
            }
            else if (Status == "Drop out")
            {
                student = db.Students.Include(s => s.CourseDetail).Include(s => s.User).Where(s => s.Status == "Drop Out");
            }
            else if (Status == "Course Completed")
            {
                student = db.Students.Include(s => s.CourseDetail).Include(s => s.User).Where(s => s.Status == "Course Completed");
            }
            else
            {
                student = db.Students.Include(s => s.CourseDetail).Include(s => s.User);
            }

            return View(student.ToList());
        }

        // GET: Students/Details/5
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
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/AddStudent
        public ActionResult AddStudent()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Course = new SelectList(db.Courses, "Id", "CourseName");
            ViewBag.ChangesDoneBy = new SelectList(db.Users, "Id", "Username");
            return View();
        }

        // POST: Students/AddStudent
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent([Bind(Include = "StudentId,StudentName,FatherName,NICNumber,StudentCourse,Contact1,Contact2,Email,Address,Status,ChangesDoneBy,Notes")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.ChangesDoneBy = Convert.ToInt32(Session["UserId"].ToString());
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Course = new SelectList(db.Courses, "Id", "CourseName");
            //ViewBag.ChangesDoneBy = new SelectList(db.Users, "Id", "Username", student.ChangesDoneBy);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                TempData["NoIdFound"] = "No selection made";
                return RedirectToAction("Index", "Students");
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass() { Status = "Select Status" });
            list.Add(new StatusClass() { Status = "Course Completed" });
            list.Add(new StatusClass() { Status = "Center Transfer" });
            list.Add(new StatusClass() { Status = "Drop Out" });

            ViewBag.Status = new SelectList(list, "Status", "Status");
            ViewBag.Course = new SelectList(db.Courses, "Id", "CourseName");
            ViewBag.ChangesDoneBy = new SelectList(db.Users, "Id", "Username", student.ChangesDoneBy);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,StudentName,FatherName,NICNumber,StudentCourse,Contact1,Contact2,Email,Address,Status,ChangesDoneBy,Notes")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.ChangesDoneBy = Convert.ToInt32(Session["UserId"].ToString());
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass() { Status = "Select Status" });
            list.Add(new StatusClass() { Status = "Course Completed" });
            list.Add(new StatusClass() { Status = "Center Transfer" });
            list.Add(new StatusClass() { Status = "Drop Out" });

            ViewBag.Status = new SelectList(list, "Status", "Status");
            ViewBag.StudentCourse = new SelectList(db.CourseDetails, "CourseDetail1", "CourseDetailName", student.StudentCourse);
            ViewBag.ChangesDoneBy = new SelectList(db.Users, "Id", "Username", student.ChangesDoneBy);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
    }
}
