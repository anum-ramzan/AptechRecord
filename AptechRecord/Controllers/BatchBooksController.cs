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
    public class BatchBooksController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: BatchBooks
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var batchBooks = db.BatchBooks.Include(b => b.Batch).Include(b => b.Book).Include(b => b.User).Where(b=>b.Batch.BatchStatus == "In Progress");
            return View(batchBooks.ToList());
        }

        // GET: BatchBooks/Details/5
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
            BatchBook batchBook = db.BatchBooks.Find(id);
            if (batchBook == null)
            {
                return HttpNotFound();
            }
            return View(batchBook);
        }

        // GET: BatchBooks/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.BatchCode = new SelectList(db.Batches.Where(b=>b.BatchStatus == "In Progress"), "BatchCode", "BatchCode");
            ViewBag.Course = new SelectList(db.Courses, "Id", "CourseName");
            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass() { Status = "Finished" });
            list.Add(new StatusClass() { Status = "Going On" });
            ViewBag.BookStatus = new SelectList(list, "Status", "Status");

            return View();
        }

        // POST: BatchBooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BatchCode,BatchBook1,AssignDate,BookStatus,ChangesDoneBy,Notes")] BatchBook batchBook)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                batchBook.ChangesDoneBy = Convert.ToInt32(Session["UserId"].ToString());
                batchBook.AssignDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f")).AddHours(9.00000);
                db.BatchBooks.Add(batchBook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BatchCode = new SelectList(db.Batches.Where(b=>b.BatchStatus == "In Progress"), "BatchCode", "BatchStatus", batchBook.BatchCode);
            ViewBag.Course = new SelectList(db.Courses, "Id", "CourseName");
            return View(batchBook);
        }

        // GET: BatchBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                TempData["NoIdFound"] = "No selection made";
                return RedirectToAction("Index", "BatchBooks");
            }
            BatchBook batchBook = db.BatchBooks.Find(id);
            if (batchBook == null)
            {
                return HttpNotFound();
            }
            ViewBag.BatchCode = new SelectList(db.Batches, "BatchCode", "BatchCode", batchBook.BatchCode);
            ViewBag.BatchBook1 = new SelectList(db.Books, "Id", "BookName", batchBook.BatchBook1);
            return View(batchBook);
        }

        // POST: BatchBooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BatchCode,BatchBook1,AssignDate,BookStatus,ChangesDoneBy,Notes")] BatchBook batchBook)
        {
            if (ModelState.IsValid)
            {
                batchBook.ChangesDoneBy = Convert.ToInt32(Session["UserId"].ToString());
                batchBook.AssignDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f")).AddHours(9.00000);
                db.Entry(batchBook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BatchCode = new SelectList(db.Batches, "BatchCode", "BatchCode", batchBook.BatchCode);
            ViewBag.BatchBook1 = new SelectList(db.Books, "Id", "BookName", batchBook.BatchBook1);
            return View(batchBook);
        }

        // GET: BatchBooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BatchBook batchBook = db.BatchBooks.Find(id);
            if (batchBook == null)
            {
                return HttpNotFound();
            }
            return View(batchBook);
        }

        // POST: BatchBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BatchBook batchBook = db.BatchBooks.Find(id);
            db.BatchBooks.Remove(batchBook);
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
