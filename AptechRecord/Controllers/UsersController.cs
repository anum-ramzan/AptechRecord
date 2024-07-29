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
    public class UsersController : Controller
    {
        private AptechSFCRecordEntities db = new AptechSFCRecordEntities();

        // GET: Users
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            var users = db.Users.Include(u => u.Role);
            return View(users.ToList());
        }

        // GET: Users/AddUser
        public ActionResult AddUser()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass() { Status = "Active" });
            list.Add(new StatusClass() { Status = "Deactive" });
            ViewBag.UserStatus = new SelectList(list, "Status", "Status");
            ViewBag.UserRole = new SelectList(db.Roles, "Id", "Rolename");
            return View();
        }

        // POST: Users/AddUser
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser([Bind(Include = "Id,Name,Username,Password,UserRole,Userstatus,UserAddUserdOn,RePassword")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Convert.ToInt32(db.usp_auto_userid().Single());
                user.UserCreatedOn = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f")).AddHours(9.00000);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass() { Status = "Active" });
            list.Add(new StatusClass() { Status = "Deactive" });
            ViewBag.UserStatus = new SelectList(list, "Status", "Status");
            ViewBag.UserRole = new SelectList(db.Roles, "Id", "Rolename", user.UserRole);
            return View(user);
        }

        // GET: Users/EditUser/5
        public ActionResult EditUser(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass() { Status = "Active" });
            list.Add(new StatusClass() { Status = "Deactive" });
            ViewBag.UserStatus = new SelectList(list, "Status", "Status", user.Userstatus);
            ViewBag.UserRole = new SelectList(db.Roles, "Id", "Rolename", user.UserRole);
            return View(user);
        }

        // POST: Users/EditUser/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "Id,Name,Username,Password,UserRole,Userstatus,UserAddUserdOn")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UserCreatedOn = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.f")).AddHours(9.00000);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<StatusClass> list = new List<StatusClass>();
            list.Add(new StatusClass() { Status = "Active" });
            list.Add(new StatusClass() { Status = "Deactive" });
            ViewBag.UserStatus = new SelectList(list, "Status", "Status", user.Userstatus);
            ViewBag.UserRole = new SelectList(db.Roles, "Id", "Rolename", user.UserRole);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
