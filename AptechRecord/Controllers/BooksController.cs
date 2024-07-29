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
    public class BooksController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: Books
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var books = db.Books.Include(b => b.Semester);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/AddBook
        public ActionResult AddBook()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            ViewBag.Course = new SelectList(db.Courses, "Id", "CourseName");
            //ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "SemesterName");
            return View();
        }

        // POST: Books/AddBook
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBook([Bind(Include = "Id,BookName,SemesterId")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        
            ViewBag.Course = new SelectList(db.Courses, "Id", "CourseName");
            //ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "SemesterName", book.SemesterId);
            return View(book);
        }

        // GET: Books/EditBook/5
        public ActionResult EditBook(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                TempData["NoIdFound"] = "No book selected to edit";
                return RedirectToAction("Index", "Books");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            // ViewBag.Course = new SelectList(db.usp_select_edit_book(id), "CID", "CName", db.usp_select_edit_book(id).Select(x=>x.CID));
            ViewBag.Course = new SelectList(db.Courses, "Id", "CourseName");
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "SemesterName", "CourseId", book.SemesterId);
            return View(book);
        }

        // POST: Books/EditBook/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook([Bind(Include = "Id,BookName,SemesterId")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           ViewBag.Course = new SelectList(db.Courses, "Id", "CourseName");
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "SemesterName", "CourseId", book.SemesterId);
            return View(book);
        }

        // GET: Books/DeleteBook/5
        public ActionResult DeleteBook(int? id)
        {
            if (id == null)
            {
                TempData["NoIdFound"] = "No book selected to delete";
                return RedirectToAction("Index", "Books");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/DeleteBook/5
        [HttpPost, ActionName("DeleteBook")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBookConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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

        [HttpPost]
        //Semester drop down proper
        public JsonResult GetCourseSemester(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var semester = db.Semesters.Where(s => s.CourseId == id).ToList();
                return Json(semester, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return Json("Something went wrong. Try later.", JsonRequestBehavior.AllowGet);
            }
        }

        //books drop down proper
        public JsonResult GetSemesterBooks(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var books = db.Books.Where(s => s.SemesterId == id).ToList();
                return Json(books, JsonRequestBehavior.AllowGet);
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
