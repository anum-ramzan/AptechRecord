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
    public class StudentBatchesController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: StudentBatches
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var studentBatches = db.StudentBatches.Include(s => s.Batch).Include(s => s.Student).Include(s => s.User).Where(b => b.BatchStatus == "Enrolled");
            return View(studentBatches.ToList());
        }

        // GET: StudentBatches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentBatch studentBatch = db.StudentBatches.Find(id);
            if (studentBatch == null)
            {
                return HttpNotFound();
            }
            return View(studentBatch);
        }

        // GET: StudentBatches/AssignBatch
        public ActionResult AssignBatch()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.BatchCode = new SelectList(db.Batches.Where(b => b.BatchStatus == "In Progress" || b.BatchStatus == "Not Yet Started"), "BatchCode", "BatchCode");
            ViewBag.StudentId = new SelectList(db.Students.Where(s => s.Status == "Enrolled"), "StudentId", "StudentId");
            return View();
        }

        // POST: StudentBatches/AssignBatch
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignBatch([Bind(Include = "Id,StudentId,BatchCode,AssignDate,BatchStatus,ChangesDoneBy,Notes")] StudentBatch studentBatch)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                studentBatch.Id = Convert.ToInt32(db.usp_select_sb());
                studentBatch.ChangesDoneBy = Convert.ToInt32(Session["UserId"].ToString());
                studentBatch.AssignDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f")).AddHours(9.00000);
                db.StudentBatches.Add(studentBatch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BatchCode = new SelectList(db.Batches.Where(b=>b.BatchStatus == "In Progress" || b.BatchStatus == "Not Yet Started"), "BatchCode", "BatchCode");
            ViewBag.StudentId = new SelectList(db.Students.Where(s=>s.Status == "Enrolled"), "StudentId", "StudentId");
            return View(studentBatch);
        }

        // GET: StudentBatches/ChangeBatch/5
        public ActionResult ChangeBatch(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                TempData["NoIdFound"] = "No selection made";
                return RedirectToAction("Index", "BatchTeacherDetails");
            }
            StudentBatch studentBatch = db.StudentBatches.Find(id);
            if (studentBatch == null)
            {
                return HttpNotFound();
            }
            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass() { Status = "Select Status" });
            list.Add(new StatusClass() { Status = "Course Completed" });
            list.Add(new StatusClass() { Status = "Batch Transfer" });
            list.Add(new StatusClass() { Status = "Drop Out" });

            ViewBag.BatchStatus = new SelectList(list, "Status", "Status");
            ViewBag.BatchCode = new SelectList(db.Batches.Where(b=>b.BatchStatus == "In Progress" || b.BatchStatus == "Not Yet Started"), "BatchCode", "BatchCode", studentBatch.BatchCode);
            ViewBag.StudentId = new SelectList(db.Students.Where(s=>s.Status == "Enrolled"), "StudentId", "StudentId", studentBatch.StudentId);
            return View(studentBatch);
        }

        // POST: StudentBatches/ChangeBatch/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeBatch([Bind(Include = "Id,StudentId,BatchCode,AssignDate,BatchStatus,ChangesDoneBy,Notes")] StudentBatch studentBatch)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                studentBatch.ChangesDoneBy = Convert.ToInt32(Session["UserId"].ToString());
                studentBatch.AssignDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f")).AddHours(9.00000);
                db.Entry(studentBatch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass() { Status = "Select Status" });
            list.Add(new StatusClass() { Status = "Course Completed" });
            list.Add(new StatusClass() { Status = "Batch Transfer" });
            list.Add(new StatusClass() { Status = "Drop Out" });

            ViewBag.BatchStatus = new SelectList(list, "Status", "Status");
            ViewBag.BatchCode = new SelectList(db.Batches.Where(b => b.BatchStatus == "In Progress" || b.BatchStatus == "Not Yet Started"), "BatchCode", "BatchCode", studentBatch.BatchCode);
            ViewBag.StudentId = new SelectList(db.Students.Where(s => s.Status == "Enrolled"), "StudentId", "StudentId", studentBatch.StudentId);
            return View(studentBatch);
        }

        // GET: StudentBatches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentBatch studentBatch = db.StudentBatches.Find(id);
            if (studentBatch == null)
            {
                return HttpNotFound();
            }
            return View(studentBatch);
        }

        // POST: StudentBatches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentBatch studentBatch = db.StudentBatches.Find(id);
            db.StudentBatches.Remove(studentBatch);
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
