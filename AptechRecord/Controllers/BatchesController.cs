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
    public class BatchesController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();


        //[HttpGet]
        //// GET: Batches
        //public ActionResult Index()
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Login", "Accounts");
        //    }
        //    var batches = db.Batches.Include(b => b.Day).Include(b => b.User).Include(b => b.TimeSlot).OrderByDescending(d => d.BatchStartDate);
        //    return View(batches.ToList());
        //}


        //[ActionName("Index")]
        //GET: Batches/Course Completed
        public ActionResult Index(string Status)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            IQueryable<Batch> batches;
            if (Status == "In Progress")
            {
                batches = db.Batches.Include(b => b.Day).Include(b => b.User).Include(b => b.TimeSlot).OrderByDescending(d => d.BatchStartDate).Where(s => s.BatchStatus == "In Progress");
            }
            else if (Status == "Not Yet Started")
            {
                batches = db.Batches.Include(b => b.Day).Include(b => b.User).Include(b => b.TimeSlot).OrderByDescending(d => d.BatchStartDate).Where(s => s.BatchStatus == "Not Yet Started");
            }
            else if (Status == "Course Completed")
            {
                batches = db.Batches.Include(b => b.Day).Include(b => b.User).Include(b => b.TimeSlot).OrderByDescending(d => d.BatchStartDate).Where(s => s.BatchStatus == "Course Completed");
            }
            else
            {
                batches = db.Batches.Include(b => b.Day).Include(b => b.User).Include(b => b.TimeSlot).OrderByDescending(d => d.BatchStartDate);
            }
            return View(batches.ToList());
        }

        // GET: Batches/Details/5
        public ActionResult Details(string id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Batch batch = db.Batches.Find(id);
            if (batch == null)
            {
                return HttpNotFound();
            }
            return View(batch);
        }

        // GET: Batches/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.BatchDays = new SelectList(db.Days, "Id", "Name");
            ViewBag.BatchBy = new SelectList(db.Users, "Id", "Username");
            ViewBag.BatchTiming = new SelectList(db.TimeSlots, "Id", "Slot");

            #region Batch Status Region
            List<BatchStatus> batchStatus = new List<BatchStatus>();
            batchStatus.Add(new BatchStatus() { Status = "Select Status" });
            batchStatus.Add(new BatchStatus() { Status = "Course Completed" });
            batchStatus.Add(new BatchStatus() { Status = "In Progress" });
            batchStatus.Add(new BatchStatus() { Status = "Not Yet Started" });
            #endregion

            ViewBag.BatchStatus = new SelectList(batchStatus, "Status", "Status");

            return View();
        }

        // POST: Batches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BatchCode,BatchTiming,BatchStartDate,BatchStatus,BatchBy,Notes,BatchDays")] Batch batch)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                db.Batches.Add(batch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BatchDays = new SelectList(db.Days, "Id", "Name", batch.BatchDays);
            ViewBag.BatchBy = new SelectList(db.Users, "Id", "Username", batch.BatchBy);
            ViewBag.BatchTiming = new SelectList(db.TimeSlots, "Id", "Slot", batch.BatchTiming);

            #region Batch Status Region
            List<BatchStatus> batchStatus = new List<BatchStatus>();
            batchStatus.Add(new BatchStatus() { Status = "Select Status" });
            batchStatus.Add(new BatchStatus() { Status = "Course Completed" });
            batchStatus.Add(new BatchStatus() { Status = "In Progress" });
            batchStatus.Add(new BatchStatus() { Status = "Not Yet Started" });
            #endregion

            ViewBag.BatchStatus = new SelectList(batchStatus, "Status", "Status");

            return View(batch);
        }

        // GET: Batches/EditBatch/5
        public ActionResult EditBatch(string id)
        {
            if (id == null)
            {
                TempData["NoBIdFound"] = "Select batch to edit";
                return RedirectToAction("Index");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Batch batch = db.Batches.Find(id);
            if (batch == null)
            {
                return HttpNotFound();
            }
            ViewBag.BatchDays = new SelectList(db.Days, "Id", "Name", batch.BatchDays);
            ViewBag.BatchBy = new SelectList(db.Users, "Id", "Username", batch.BatchBy);
            ViewBag.BatchTiming = new SelectList(db.TimeSlots, "Id", "Slot", batch.BatchTiming);

            #region Batch Status Region
            List<BatchStatus> batchStatus = new List<BatchStatus>();
            batchStatus.Add(new BatchStatus() { Status = "Select Status" });
            batchStatus.Add(new BatchStatus() { Status = "Course Completed" });
            batchStatus.Add(new BatchStatus() { Status = "In Progress" });
            batchStatus.Add(new BatchStatus() { Status = "Not Yet Started" });
            #endregion

            ViewBag.BatchStatus = new SelectList(batchStatus, "Status", "Status", batch.BatchStatus);

            return View(batch);
        }

        // POST: Batches/EditBatch/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBatch([Bind(Include = "BatchCode,BatchTiming,BatchStartDate,BatchStatus,BatchBy,Notes,BatchDays")] Batch batch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(batch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BatchDays = new SelectList(db.Days, "Id", "Name", batch.BatchDays);
            ViewBag.BatchBy = new SelectList(db.Users, "Id", "Username", batch.BatchBy);
            ViewBag.BatchTiming = new SelectList(db.TimeSlots, "Id", "Slot", batch.BatchTiming);

            #region Batch Status Region
            List<BatchStatus> batchStatus = new List<BatchStatus>();
            batchStatus.Add(new BatchStatus() { Status = "Select Status" });
            batchStatus.Add(new BatchStatus() { Status = "Course Completed" });
            batchStatus.Add(new BatchStatus() { Status = "In Progress" });
            batchStatus.Add(new BatchStatus() { Status = "Not Yet Started" });
            #endregion


            ViewBag.BatchStatus = new SelectList(batchStatus, "Status", "Status", batch.BatchStatus);

            return View(batch);
        }

        // GET: Batches/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Batch batch = db.Batches.Find(id);
            if (batch == null)
            {
                return HttpNotFound();
            }
            return View(batch);
        }

        // POST: Batches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Batch batch = db.Batches.Find(id);
            db.Batches.Remove(batch);
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
