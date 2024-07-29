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
    public class BatchTeacherDetailsController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: BatchTeacherDetails
        public ActionResult Index()
        {

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var batchTeacherDetails = db.BatchTeacherDetails.Include(b => b.Batch).Include(b => b.Employee).Include(b => b.User);
            return View(batchTeacherDetails.ToList());
        }
        // GET: BatchTeacherDetails/ChangeTeacher
        public ActionResult ChangeTeacher()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            List<StatusClass> status = new List<StatusClass>();
            status.Add(new StatusClass() { Status = "New" });
            status.Add(new StatusClass() { Status = "Alternative" });

            ViewBag.TeacherStatus = new SelectList(status, "Status", "Status");
            ViewBag.BatchCode = new SelectList(db.Batches, "BatchCode", "BatchCode");
            ViewBag.TeacherCode = new SelectList(db.Employees, "Id", "EmployeeName");
            return View();
        }

        // POST: BatchTeacherDetails/ChangeTeacher
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeTeacher([Bind(Include = "Id,BatchCode,TeacherCode,AssignDate,TeacherStatus,ChangesDoneBy,Notes")] BatchTeacherDetail batchTeacherDetail)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                batchTeacherDetail.ChangesDoneBy = Convert.ToInt32(Session["UserId"].ToString());
                batchTeacherDetail.AssignDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f")).AddHours(9.00000);
                db.BatchTeacherDetails.Add(batchTeacherDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<StatusClass> status = new List<StatusClass>();
            status.Add(new StatusClass() { Status = "New" });
            status.Add(new StatusClass() { Status = "Alternative" });

            ViewBag.TeacherStatus = new SelectList(status, "Status", "Status");
            ViewBag.BatchCode = new SelectList(db.Batches, "BatchCode", "BatchCode", batchTeacherDetail.BatchCode);
            ViewBag.TeacherCode = new SelectList(db.Employees, "Id", "EmployeeName", batchTeacherDetail.TeacherCode);
            return View(batchTeacherDetail);
        }

        // GET: BatchTeacherDetails/Edit/5
        public ActionResult Edit(int? id)
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
            BatchTeacherDetail batchTeacherDetail = db.BatchTeacherDetails.Find(id);
            if (batchTeacherDetail == null)
            {
                return HttpNotFound();
            }
            List<StatusClass> status = new List<StatusClass>();
            status.Add(new StatusClass() { Status = "New" });
            status.Add(new StatusClass() { Status = "Alternative" });

            ViewBag.TeacherStatus = new SelectList(status, "Status", "Status", batchTeacherDetail.TeacherStatus);
            ViewBag.BatchCode = new SelectList(db.Batches, "BatchCode", "BatchCode", batchTeacherDetail.BatchCode);
            ViewBag.TeacherCode = new SelectList(db.Employees, "Id", "EmployeeName", batchTeacherDetail.TeacherCode);
            return View(batchTeacherDetail);
        }

        // POST: BatchTeacherDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BatchCode,TeacherCode,AssignDate,TeacherStatus,ChangesDoneBy,Notes")] BatchTeacherDetail batchTeacherDetail)
        {
            if (ModelState.IsValid)
            {
                batchTeacherDetail.ChangesDoneBy = Convert.ToInt32(Session["UserId"].ToString());
                batchTeacherDetail.AssignDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f")).AddHours(9.00000);
                db.Entry(batchTeacherDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<StatusClass> status = new List<StatusClass>();
            status.Add(new StatusClass() { Status = "New" });
            status.Add(new StatusClass() { Status = "Alternative" });

            ViewBag.TeacherStatus = new SelectList(status, "Status", "Status", batchTeacherDetail.TeacherStatus);
            ViewBag.BatchCode = new SelectList(db.Batches, "BatchCode", "BatchCode", batchTeacherDetail.BatchCode);
            ViewBag.TeacherCode = new SelectList(db.Employees, "Id", "EmployeeName", batchTeacherDetail.TeacherCode); return View(batchTeacherDetail);
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
