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
    public class VouchersController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        public ActionResult VoucherReport()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                } 
                DateTime? date = Convert.ToDateTime(DateTime.Now.ToString());    
                return View(db.usp_select_voucher(date).ToList());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }



        // GET: Vouchers
        public ActionResult Index()
        {

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            } var vouchers = db.Vouchers.Include(v => v.Student).Include(v => v.User);
            return View(vouchers.ToList());
        }

        // GET: Vouchers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        // GET: Vouchers/Create
        public ActionResult Create()
        {

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            } ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentName");
            ViewBag.VoucherDistributedBy = new SelectList(db.Users, "Id", "Username");
            return View();
        }

        // POST: Vouchers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,VoucherDate,VoucherStatus,VoucherDistributedAt,VoucherDistributedBy,Notes")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.Vouchers.Add(voucher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentName", voucher.StudentId);
            ViewBag.VoucherDistributedBy = new SelectList(db.Users, "Id", "Username", voucher.VoucherDistributedBy);
            return View(voucher);
        }

        // GET: Vouchers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentName", voucher.StudentId);
            ViewBag.VoucherDistributedBy = new SelectList(db.Users, "Id", "Username", voucher.VoucherDistributedBy);
            return View(voucher);
        }

        // POST: Vouchers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,VoucherDate,VoucherStatus,VoucherDistributedAt,VoucherDistributedBy,Notes")] Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(voucher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "StudentName", voucher.StudentId);
            ViewBag.VoucherDistributedBy = new SelectList(db.Users, "Id", "Username", voucher.VoucherDistributedBy);
            return View(voucher);
        }

        // GET: Vouchers/Delete/5
        public ActionResult Delete(int? id)
        {

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            } if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voucher voucher = db.Vouchers.Find(id);
            if (voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        // POST: Vouchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Voucher voucher = db.Vouchers.Find(id);
            db.Vouchers.Remove(voucher);
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
