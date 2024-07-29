using AptechRecord.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AptechRecord.Controllers
{
    [Authorize]
    public class RecoveryOfficerController : Controller
    {

        AptechSFCRecordEntities db = new AptechSFCRecordEntities();
        public static string taskName = null;
        public static string staticBatchCode = null;

        // GET: RecoveryOfficer
        public ActionResult Dashboard()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                else
                {
                    //ViewBag.CoursesCount = db.Courses.Where(c => c.CourseStatus == "Current").ToList().Count;
                    //ViewBag.BatchesCount = db.Batches.Where(b => b.BatchStatus == "In Progress").ToList().Count;
                    //ViewBag.StudentsCount = db.Students.Where(s => s.Status == "Enrolled").ToList().Count;
                    //ViewBag.FacultiesCount = db.Employees.Where(e => e.EmployeeStatus == "Active").ToList().Count;

                    #region switch Day

                    string days = DateTime.Now.AddHours(9.00000).ToString("dddd");
                    string passStringToDD = null;
                    switch (days)
                    {
                        case "Monday":
                            passStringToDD = "MWF";
                            break;
                        case "Wednesday":
                            passStringToDD = "MWF";
                            break;
                        case "Friday":
                            passStringToDD = "MWF";
                            break;
                        case "Tuesday":
                            passStringToDD = "TTS";
                            break;
                        case "Thursday":
                            passStringToDD = "TTS";
                            break;
                        case "Saturday":
                            passStringToDD = "TTS";
                            break;
                    }

                    #endregion
                    DateTime? date = Convert.ToDateTime(DateTime.Now.AddHours(9.00000).ToString());

                    //counters
                    ViewBag.TotalBatch = db.usp_select_total_batch(passStringToDD).ToList().Count;
                    ViewBag.TotalMarkedBatch = db.usp_select_total_marked_batch(date, passStringToDD).ToList().Count;
                    ViewBag.TotalUnmarkedBatch = db.usp_select_total_unmarked_batch(date, passStringToDD).ToList().Count;

                    ViewBag.TotalStudent = db.usp_select_total_student(passStringToDD).ToList().Count;
                    ViewBag.TotalPresentStudent = db.usp_select_total_student_present(passStringToDD, date).ToList().Count;
                    ViewBag.TotalAbsentStudent = db.usp_select_total_student_absent(passStringToDD, date).ToList().Count;

                    var list = db.usp_recovery_batch_list(date).ToList();
                    return View(list);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        //Get: Task
        public ActionResult Task(string task)
        {

            #region switch Day

            string days = DateTime.Now.AddHours(9.00000).ToString("dddd");
            string passStringToDD = null;
            switch (days)
            {
                case "Monday":
                    passStringToDD = "MWF";
                    break;
                case "Wednesday":
                    passStringToDD = "MWF";
                    break;
                case "Friday":
                    passStringToDD = "MWF";
                    break;
                case "Tuesday":
                    passStringToDD = "TTS";
                    break;
                case "Thursday":
                    passStringToDD = "TTS";
                    break;
                case "Saturday":
                    passStringToDD = "TTS";
                    break;
            }
            #endregion
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (task == null)
                {
                    return RedirectToAction("Dashboard", "RecoveryOfficer");
                }
                else if (task.Equals("attendance"))
                {
                    //mwf or tts days batches on current day
                    ViewBag.TaskTitle = task;
                    taskName = task;

                    var batchesList = db.usp_select_unmarked_batch_list(passStringToDD, Convert.ToDateTime(DateTime.Now.AddHours(9.00000))).ToList();
                    ViewBag.BatchDropDown = new SelectList(batchesList, "BatchCode", "BatchCode");
                }
                else if (task.Equals("absentees"))
                {
                    //mwf or tts days marked batches on current day
                    ViewBag.TaskTitle = task;
                    taskName = task;
                    var batchesList = db.usp_select_absentees_batch(passStringToDD, Convert.ToDateTime(DateTime.Now.AddHours(9.00000))).ToList();
                    ViewBag.BatchDropDown = new SelectList(batchesList, "BatchCode", "BatchCode");
                }
                else if (task.Equals("voucher"))
                {
                    //all batches
                    ViewBag.TaskTitle = task;
                    taskName = task;
                    var batchesList = db.usp_select_undistributed_batch_list(Convert.ToDateTime(DateTime.Now.AddHours(9.00000))).ToList();
                    ViewBag.BatchDropDown = new SelectList(batchesList, "BatchCode", "BatchCode");
                }
                else if (task.Equals("unreceived"))
                {
                    //all batches
                    ViewBag.TaskTitle = task;
                    taskName = task;
                    var batchesList = db.usp_select_unreceived_batch(Convert.ToDateTime(DateTime.Now.AddHours(9.00000))).ToList();
                    ViewBag.BatchDropDown = new SelectList(batchesList, "BatchCode", "BatchCode");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
            }

            return View();
        }

        //POST: 
        [HttpPost]
        public ActionResult Task()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }

                #region switch Day

                string days = DateTime.Now.AddHours(9.00000).ToString("dddd");
                string passStringToDD = null;
                switch (days)
                {
                    case "Monday":
                        passStringToDD = "MWF";
                        break;
                    case "Wednesday":
                        passStringToDD = "MWF";
                        break;
                    case "Friday":
                        passStringToDD = "MWF";
                        break;
                    case "Tuesday":
                        passStringToDD = "TTS";
                        break;
                    case "Thursday":
                        passStringToDD = "TTS";
                        break;
                    case "Saturday":
                        passStringToDD = "TTS";
                        break;
                }
                #endregion

                //selective batches will be shown in the list for attendance
                //selective days batches will be seen
                if (taskName.Equals("attendance"))
                {
                    var batchesList = db.usp_select_unmarked_batch_list(passStringToDD, Convert.ToDateTime(DateTime.Now.AddHours(9.00000))).ToList();
                    ViewBag.BatchDropDown = new SelectList(batchesList, "BatchCode", "BatchCode");
                    string DDLValue = Request.Form["BatchDropDown"].ToString();
                    return RedirectToAction("Attendance", "RecoveryOfficer", new { batch = DDLValue });
                }
                //all batches must have there for voucher distribution
                else if (taskName.Equals("voucher"))
                {
                    var batchesList = db.usp_select_undistributed_batch_list(Convert.ToDateTime(DateTime.Now.AddHours(9.00000))).ToList();
                    ViewBag.BatchDropDown = new SelectList(batchesList, "BatchCode", "BatchCode");
                    string DDLValue = Request.Form["BatchDropDown"].ToString();
                    return RedirectToAction("Voucher", "RecoveryOfficer", new { batch = DDLValue });
                }
                else if (taskName.Equals("absentees"))
                {
                    var batchesList = db.usp_select_absentees_batch(passStringToDD, Convert.ToDateTime(DateTime.Now.AddHours(9.00000))).ToList();
                    ViewBag.BatchDropDown = new SelectList(batchesList, "BatchCode", "BatchCode");
                    string DDLValue = Request.Form["BatchDropDown"].ToString();
                    return RedirectToAction("AbsenteesAttendance", "RecoveryOfficer", new { batch = DDLValue });
                }
                else if (taskName.Equals("unreceived"))
                {
                    var batchesList = db.usp_select_unreceived_batch(Convert.ToDateTime(DateTime.Now.AddHours(9.00000))).ToList();
                    ViewBag.BatchDropDown = new SelectList(batchesList, "BatchCode", "BatchCode");
                    string DDLValue = Request.Form["BatchDropDown"].ToString();
                    return RedirectToAction("UnreceivedVoucher", "RecoveryOfficer", new { batch = DDLValue });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
            }

            return View();
        }

        //GET: Attendance
        public ActionResult Attendance(string batch)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (batch == null)
                {
                    return RedirectToAction("Task", "RecoveryOfficer", new { task = "attendance" });
                }
                else
                {
                    ViewBag.Id = Session["UserId"].ToString();
                    ViewBag.BatchName = batch;
                    staticBatchCode = batch;
                    //var studentList = db.StudentBatches.Include(s => s.Student).Where(b => b.BatchCode == batch).ToList();

                    var studentList = db.usp_select_attendance_student(batch).ToList();

                    ViewBag.CountStudent = studentList.Count;
                    Session["CountStudent"] = studentList.Count;
                    return View(studentList);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }
        //POST: Attendance
        [HttpPost]
        public ActionResult Attendance(string batch, FormCollection collection)
        {
            try
            {

                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                //Check for NULL.
                if (collection == null)
                {
                    return RedirectToAction("Task", "RecoveryOfficer", new { task = "attendance" });
                }

                //Loop and insert records.

                DateTime? date = Convert.ToDateTime(DateTime.Now.AddHours(9.00000));

                int x = collection.Count;
                int insertedRow = 0;

                for (int i = 0; i < int.Parse(Session["CountStudent"].ToString()); i++)
                {
                    int id = Convert.ToInt32(db.usp_auto_attendance().Single());
                    int sid = Convert.ToInt32(collection["[" + i + "].StudentId"]);
                    string status = collection["[" + i + "].AttendanceStatus"];
                    if (status == null)
                    {
                        status = "Absent";
                    }
                    int markBy = int.Parse(Session["UserId"].ToString());
                    string notes = collection["[" + i + "].Notes"].ToString();

                    var check = db.usp_check_student_attendance(sid, date).SingleOrDefault();

                    if (check == 0)
                    {
                        db.usp_insert_attendance(id, sid, date, status, date, markBy, notes);
                        db.SaveChanges();
                        insertedRow++;
                    }
                }

                if (insertedRow == 0) { ViewBag.Message = -1; }
                else
                {
                    ViewBag.Message = 1;
                    ViewBag.TS = db.usp_recovery_ts(batch, date).Single();
                    ViewBag.TP = db.usp_recovery_tp(batch, date).Single();
                    ViewBag.TA = db.usp_recovery_ta(batch, date).Single();
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        //GET: Absentees
        public ActionResult AbsenteesAttendance(string batch)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (batch == null)
                {
                    return RedirectToAction("Task", "RecoveryOfficer", new { task = "absentees" });
                }
                else
                {
                    ViewBag.Id = Session["UserId"].ToString();
                    ViewBag.BatchName = batch;
                    //var studentList = db.StudentBatches.Include(s => s.Student).Where(b => b.BatchCode == batch).ToList();
                    var studentList = db.usp_select_absent_student(batch).ToList();
                    ViewBag.CountStudent = studentList.Count;
                    return View(studentList);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }
        //POST: Absentees
        [HttpPost]
        public ActionResult AbsenteesAttendance(string batch, FormCollection collection)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                //Check for NULL.
                if (collection == null || batch == null)
                {
                    return RedirectToAction("Task", "RecoveryOfficer", new { task = "absentees" });
                }

                //Loop and insert records.

                DateTime? date = Convert.ToDateTime(DateTime.Now.AddHours(9.00000));

                int x = collection.Count;
                int insertedRow = 0;

                for (int i = 0; i < (x / 3); i++)
                {
                    int sid = Convert.ToInt32(collection["[" + i + "].StudentId"]);
                    string status = collection["[" + i + "].AttendanceStatus"];
                    int markBy = int.Parse(Session["UserId"].ToString());
                    string notes = collection["[" + i + "].Notes"].ToString();

                    db.usp_update_student_attendance(status, date, markBy, notes, sid, date);
                    db.SaveChanges();
                    insertedRow++;

                }

                if (insertedRow == 0) { ViewBag.Message = -1; }
                else
                {
                    ViewBag.Message = 1;
                    ViewBag.TS = db.usp_recovery_ts(batch, date).Single();
                    ViewBag.TP = db.usp_recovery_tp(batch, date).Single();
                    ViewBag.TA = db.usp_recovery_ta(batch, date).Single();
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        //GET: Voucher
        public ActionResult Voucher(string batch)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (batch == null)
                {
                    return RedirectToAction("Task", "RecoveryOfficer", new { task = "voucher" });
                }
                else
                {
                    ViewBag.Id = Session["UserId"].ToString();
                    ViewBag.BatchName = batch;

                    var studentList = db.usp_select_voucher_student(batch).ToList();

                    ViewBag.CountStudent = studentList.Count;
                    return View(studentList);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }
        //POSt: Voucher
        [HttpPost]
        public ActionResult Voucher(string batch, FormCollection collection)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                //Check for NULL.
                if (collection == null || batch == null)
                {
                    return RedirectToAction("Task", "RecoveryOfficer", new { task = "absentees" });
                }

                //Loop and insert records.

                DateTime? date = Convert.ToDateTime(DateTime.Now.AddHours(9.00000));

                int x = collection.Count;
                int insertedRow = 0;

                for (int i = 0; i < (x / 3); i++)
                {
                    int id = Convert.ToInt32(db.usp_auto_voucher().Single());
                    int sid = Convert.ToInt32(collection["[" + i + "].StudentId"]);
                    string status = collection["[" + i + "].VoucherStatus"];
                    int markBy = int.Parse(Session["UserId"].ToString());
                    string notes = collection["[" + i + "].Notes"].ToString();

                    var check = db.usp_check_student_voucher(sid, date).SingleOrDefault();

                    if (check == 0)
                    {
                        db.usp_insert_voucher(id, sid, date, status, date, markBy, notes);
                        db.SaveChanges();
                        insertedRow++;
                    }
                }

                if (insertedRow == 0) { ViewBag.Message = -1; }
                else
                {
                    ViewBag.Message = 1;
                    ViewBag.TS = db.usp_recovery_vts(batch, date).Single();
                    ViewBag.TR = db.usp_recovery_vtr(batch, date).Single();
                    ViewBag.TU = db.usp_recovery_vtu(batch, date).Single();
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        //GET: Unreceived
        public ActionResult UnreceivedVoucher(string batch)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (batch == null)
                {
                    return RedirectToAction("Task", "RecoveryOfficer", new { task = "unreceived" });
                }
                else
                {
                    ViewBag.Id = Session["UserId"].ToString();
                    ViewBag.BatchName = batch;
                    var studentList = db.usp_select_unreceived_student(batch, Convert.ToDateTime(DateTime.Now.AddHours(9.00000))).ToList();
                    ViewBag.CountStudent = studentList.Count;
                    return View(studentList);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }
        //POST: Unreceived
        [HttpPost]
        public ActionResult UnreceivedVoucher(string batch, FormCollection collection)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                //Check for NULL.
                if (collection == null || batch == null)
                {
                    return RedirectToAction("Task", "RecoveryOfficer", new { task = "unreceived" });
                }

                //Loop and insert records.

                DateTime? date = Convert.ToDateTime(DateTime.Now.AddHours(9.00000));

                int x = collection.Count;
                int insertedRow = 0;

                for (int i = 0; i < (x / 3); i++)
                {
                    int sid = Convert.ToInt32(collection["[" + i + "].StudentId"]);
                    string status = collection["[" + i + "].AttendanceStatus"];
                    int markBy = int.Parse(Session["UserId"].ToString());
                    string notes = collection["[" + i + "].Notes"].ToString();

                    db.usp_update_student_attendance(status, date, markBy, notes, sid, date);
                    db.SaveChanges();
                    insertedRow++;

                }

                if (insertedRow == 0) { ViewBag.Message = -1; }
                else
                {
                    ViewBag.Message = 1;
                    ViewBag.TS = db.usp_recovery_ts(batch, date).Single();
                    ViewBag.TP = db.usp_recovery_tp(batch, date).Single();
                    ViewBag.TA = db.usp_recovery_ta(batch, date).Single();
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult PasswordChange()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            int id = Convert.ToInt32(Session["UserId"].ToString());
            User user = db.Users.Find(id);

            return View(user);
        }

        [HttpPost]
        public ActionResult PasswordChange([Bind(Exclude = "Id,Username,UserRole,Userstatus,UserAddUserdOn")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.usp_update_user_password(user.Password, Convert.ToInt32(Session["UserId"].ToString()));
                    TempData["SuccessMsg"] = "Password changed successfully.";
                    return RedirectToAction("Dashboard", "RecoveryOfficer");
                }
                else
                {
                    int id = Convert.ToInt32(Session["UserId"].ToString());
                    User users = db.Users.Find(id);
                    return View(users);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }

        }

        public ActionResult AbsenteesFollowUp()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                else
                {
                    var studentList = db.usp_select_absentees_followup(Convert.ToDateTime(DateTime.Now.AddHours(9.00000))).ToList();

                    return View(studentList);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        [HttpPost]
        public ActionResult AbsenteesFollowUp(DateTime AbsenteesDate)
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                if (AbsenteesDate == null)
                {
                    return RedirectToAction("Dashboard", "RecoveryOfficer");
                }
                else
                {
                    var studentList = db.usp_select_absentees_followup(Convert.ToDateTime(AbsenteesDate)).ToList();
                    return View(studentList);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult PastAttendance()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PastAttendance(DateTime CustomDate)
        {
            try
            {
                if (CustomDate == null)
                {
                    ViewBag.Mesg = "Select date";
                    return View();
                }
                #region switch Day

                string days = CustomDate.ToString("dddd");
                string passStringToDD = null;
                switch (days)
                {
                    case "Monday":
                        passStringToDD = "MWF";
                        break;
                    case "Wednesday":
                        passStringToDD = "MWF";
                        break;
                    case "Friday":
                        passStringToDD = "MWF";
                        break;
                    case "Tuesday":
                        passStringToDD = "TTS";
                        break;
                    case "Thursday":
                        passStringToDD = "TTS";
                        break;
                    case "Saturday":
                        passStringToDD = "TTS";
                        break;
                }
                #endregion

                ViewBag.GetDate = CustomDate;

                DateTime date = DateTime.Now.AddHours(9.00000);
                var list = db.usp_select_unmarked_attendance(passStringToDD, CustomDate).ToList();

                return View(list);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        public ActionResult DropOutList()
        {
            try
            {
                if (Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Accounts");
                }
                var list = db.usp_select_dropList().ToList();
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong. Try later.";
                MethodsReuseability.ErrorMessage(ex.Message, ex.ToString());
                return View();
            }
        }

        [HttpPost]
        public ActionResult DropOutStudent(FormCollection collection)
        {
            int coll = collection.Count;

            for (int i = 1; i < coll-1; i++)
            {
                int? sid = Convert.ToInt32(collection["studentId[" + i + "]"]);
                var notes = collection["notes[" + i + "]"];
                db.usp_update_student_drop_out(notes, sid);                
            }

            return RedirectToAction("Attendance", new { batch = RecoveryOfficerController.staticBatchCode });
        }

        [HttpPost]
        public ActionResult BatchTransferStudent(FormCollection collection)
        {
            int coll = collection.Count;

            for (int i = 1; i < coll - 1; i++)
            {
                int? sid = Convert.ToInt32(collection["studentId[" + i + "]"]);
                var batchfrom = collection["batchf[" + i + "]"];
                var batchto = collection["batcht[" + i + "]"]; 
                var notes = collection["notes[" + i + "]"];
                //db.usp_update_student_drop_out(notes, sid);
            }

            return RedirectToAction("Attendance", new { batch = RecoveryOfficerController.staticBatchCode });
        }


        #region MArking attendance with ajax call
        //Json Result for ajax call
        /*public JsonResult MarkAttendance(List<Attendance> attendances)
        {
            using (db)
            {
                //Check for NULL.
                if (attendances == null)
                {
                    attendances = new List<Attendance>();
                }

                //Loop and insert records.
                foreach (Attendance attend in attendances)
                {
                    db.Attendances.Add(attend);
                }

                int insertedRecords = 0;

                if (attendances.Count == int.Parse(Session["CountStudent"].ToString()))
                {
                    insertedRecords = db.SaveChanges();
                }
                else
                {
                    insertedRecords = -1;
                }
                return Json(insertedRecords);
            }
        }*/
        #endregion

        #region JSON MArk VOUCHER
        /*
        public JsonResult MarkVoucher(List<Voucher> voucher)
        {
            using (db)
            {
                //Check for NULL.
                if (voucher == null)
                {
                    voucher = new List<Voucher>();
                }

                //Loop and insert records.
                foreach (Voucher vou in voucher)
                {
                    db.Vouchers.Add(vou);
                }
                int insertedRecords = db.SaveChanges();
                return Json(insertedRecords);
            }
        }*/
        #endregion

        #region Absentees JSON CALL
        /*public JsonResult MarkAbsentees(List<Attendance> attendance)
        {
            using (db)
            {
                //Check for NULL.
                if (attendance == null)
                {
                    attendance = new List<Attendance>();
                }

                DateTime? date = Convert.ToDateTime(DateTime.Now.ToString());

                //Loop and insert records.
                foreach (Attendance att in attendance)
                {
                    db.usp_update_student_attendance(att.AttenddanceStatus, att.AttendanceMarkAt, att.AttendanceMarkBy, att.Notes, att.StudentId, date);
                }
                int insertedRecords = db.SaveChanges();
                return Json(insertedRecords, JsonRequestBehavior.AllowGet);
            }
        }
        */
        #endregion

        #region Student Ids JSON Data

        public ActionResult StudentIdList(string batch)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = db.usp_select_student_id_for_do(batch).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult StudentIdBatchList(string batch)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = db.usp_select_student_id_for_bt(batch).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BatchCodeList()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = db.Batches.Where(x => x.BatchStatus == "In Progress").ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}