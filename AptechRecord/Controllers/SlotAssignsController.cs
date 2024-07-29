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
    public class SlotAssignsController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: SlotAssigns
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var slotAssigns = db.SlotAssigns.Include(s => s.Day).Include(s => s.TimeSlot).Include(s => s.User);
            return View(slotAssigns.ToList());
        }

        // GET: SlotAssigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlotAssign slotAssign = db.SlotAssigns.Find(id);
            if (slotAssign == null)
            {
                return HttpNotFound();
            }
            return View(slotAssign);
        }

        // GET: SlotAssigns/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass { Status = "Active" });
            list.Add(new StatusClass { Status = "Deactive" });

            ViewBag.SlotDays = new SelectList(db.Days, "Id", "Name");
            ViewBag.SlotId = new SelectList(db.TimeSlots, "Id", "Slot");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username");
            ViewBag.TaskStatus = new SelectList(list, "Status", "Status");
            return View();
        }

        // POST: SlotAssigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,SlotId,SlotDays,AssignDate,TaskStatus")] SlotAssign slotAssign)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                slotAssign.Id = Convert.ToInt32(db.auto_sa_id().SingleOrDefault());
                db.SlotAssigns.Add(slotAssign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass { Status = "Active" });
            list.Add(new StatusClass { Status = "Deactive" });

            ViewBag.SlotDays = new SelectList(db.Days, "Id", "Name", slotAssign.SlotDays);
            ViewBag.SlotId = new SelectList(db.TimeSlots, "Id", "Slot", slotAssign.SlotId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username", slotAssign.UserId);
            ViewBag.TaskStatus = new SelectList(list, "Status", "Status", slotAssign.TaskStatus);
            return View(slotAssign);
        }

        // GET: SlotAssigns/Edit/5
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
            SlotAssign slotAssign = db.SlotAssigns.Find(id);
            if (slotAssign == null)
            {
                return HttpNotFound();
            }
            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass { Status = "Active" });
            list.Add(new StatusClass { Status = "Deactive" });
            ViewBag.SlotDays = new SelectList(db.Days, "Id", "Name", slotAssign.SlotDays);
            ViewBag.SlotId = new SelectList(db.TimeSlots, "Id", "Slot", slotAssign.SlotId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username", slotAssign.UserId);
            ViewBag.TaskStatus = new SelectList(list, "Status", "Status", slotAssign.TaskStatus);
            return View(slotAssign);
        }

        // POST: SlotAssigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,SlotId,SlotDays,AssignDate,TaskStatus")] SlotAssign slotAssign)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (ModelState.IsValid)
            {
                db.Entry(slotAssign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass { Status = "Active" });
            list.Add(new StatusClass { Status = "Deactive" });
            ViewBag.SlotDays = new SelectList(db.Days, "Id", "Name", slotAssign.SlotDays);
            ViewBag.SlotId = new SelectList(db.TimeSlots, "Id", "Slot", slotAssign.SlotId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Username", slotAssign.UserId);
            ViewBag.TaskStatus = new SelectList(list, "Status", "Status", slotAssign.TaskStatus);
            return View(slotAssign);
        }

        // GET: SlotAssigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SlotAssign slotAssign = db.SlotAssigns.Find(id);
            if (slotAssign == null)
            {
                return HttpNotFound();
            }
            return View(slotAssign);
        }

        // POST: SlotAssigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SlotAssign slotAssign = db.SlotAssigns.Find(id);
            db.SlotAssigns.Remove(slotAssign);
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
