using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;



namespace AptechRecord.Models
{
    public class StatusClass
    {
        public string Status { get; set; }
    }

    public class BatchStatus
    {
        public string Status { get; set; }
    }

    public partial class usp_select_attendance_student_Result
    {
        public DateTime AttendanceDate { get; set; }
        public int AttendanceMarkBy { get; set; }
        public DateTime AttendanceMarkAt { get; set; }
        public string Notes { get; set; }
        [Required(ErrorMessage = "Select attendance status")]
        public bool AttendanceStatus { get; set; }
    }

    public partial class usp_select_absent_student_Result
    {
        public DateTime AttendanceDate { get; set; }
        public int AttendanceMarkBy { get; set; }
        public DateTime AttendanceMarkAt { get; set; }
        public string Notes { get; set; }
        [Required(ErrorMessage = "Select attendance status")]
        public bool AttendanceStatus { get; set; }
    }

    public partial class usp_select_voucher_student_Result
    {
        public DateTime VoucherDate { get; set; }
        public int VoucherDistributedBy { get; set; }
        public DateTime VoucherDistributedAt { get; set; }
        public string Notes { get; set; }
        [Required(ErrorMessage = "Select voucher status")]
        public bool VoucherStatus { get; set; }
    }

    public partial class usp_select_unreceived_student_Result
    {
        public DateTime VoucherDate { get; set; }
        public int VoucherDistributedBy { get; set; }
        public DateTime VoucherDistributedAt { get; set; }
        public string Notes { get; set; }
        [Required(ErrorMessage = "Select voucher status")]
        public bool VoucherStatus { get; set; }
    }

    //User Region
    #region User Annotation
    [MetadataType(typeof(UserAnnotation))]
    public partial class User
    {
        // Note this class has nothing in it.  It's just here to add the class-level attribute.
        public string RePassword { get; set; }
    }

    public class UserAnnotation
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [Display(Name = "Re-enter Password")]
        public string RePassword { get; set; }
    }
    #endregion

    //Attendance Region
    #region Attendance Annotation
    [MetadataType(typeof(AttendanceAnnotation))]
    public partial class Attendance
    {
        // Note this class has nothing in it.  It's just here to add the class-level attribute.
    }


    public class AttendanceAnnotation
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMMM-yyyy}")]
        public DateTime AttendanceDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMMM-yyyy hh: mm: ss}")]
        public DateTime AttendanceMarkAt { get; set; }
    }
    #endregion

    //Batch Region
    #region Batch Annotation
    [MetadataType(typeof(BatchAnnotation))]
    public partial class Batch
    {
        // Note this class has nothing in it.  It's just here to add the class-level attribute.
    }

    public class BatchAnnotation
    {
        [Key]
        public string BatchCode { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMMM-yyyy}")]

        public Nullable<System.DateTime> BatchStartDate { get; set; }
    }
    #endregion

    //Batch Teacher Annotation
    #region Attendance Annotation
    [MetadataType(typeof(BatchTeacherDetailAnnotation))]
    public partial class BatchTeacherDetail
    {
        // Note this class has nothing in it.  It's just here to add the class-level attribute.
    }

    public class BatchTeacherDetailAnnotation
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMMM-yyyy}")]

        public Nullable<System.DateTime> AssignDate { get; set; }
    }
    #endregion

    //Attendance last 2 days Annotation
    #region Attendance Annotation
    [MetadataType(typeof(TwoDaysAttendanceResultAnnotation))]
    public partial class usp_select_two_attendance_Result
    {
        // Note this class has nothing in it.  It's just here to add the class-level attribute.
    }

    public class TwoDaysAttendanceResultAnnotation
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMMM-yyyy}")]

        public Nullable<System.DateTime> AttendanceDate { get; set; }
    }
    #endregion

    //Course Detail Annotation
    #region Course Detail
    [MetadataType(typeof(CourseDetailId))]
    public partial class CourseDetail
    {

    }

    public partial class CourseDetailId
    {
        [Key]
        public int CourseDetail1 { get; set; }
    }
    #endregion

    //Error Log Annotation
    #region error log
    [MetadataType(typeof(ErrorLogId))]
    public partial class ErrorLog
    {
    }
    public partial class ErrorLogId
    {
        [Key]
        public int ErrorId { get; set; }
    }
    #endregion

    //usp_select_unmarked_attendance_Result Annotation
    #region Un-marked Attendance
    [MetadataType(typeof(UnMarkedAttendance))]
    public partial class usp_select_unmarked_attendance_Result
    { }
    public class UnMarkedAttendance
    {
        [Key]
        public string BatchCode { get; set; }
    }
    #endregion

    /********usp_select_voucher_Result*******/
    #region Voucher Attendance
    [MetadataType(typeof(VoucherAttendance))]
    public partial class usp_select_voucher_Result
    { }
    public class VoucherAttendance
    {
        [Key]
        public int StudentId { get; set; }
    }
    #endregion

    //Customized Attendance
    #region Customized Attendance
    [MetadataType(typeof(CustomizedAttendance))]
    public partial class usp_select_customized_attendance_Result
    { }
    public class CustomizedAttendance
    {
        [Key]
        public int Id { get; set; }
    }
    #endregion

    //Unreceived Voucher Entry
    #region Unreceived Voucher Entry

    [MetadataType(typeof(UnreceivedVoucher))]
    public partial class usp_select_unreceived_student_Result
    { }
    public class UnreceivedVoucher
    {
        [Key]
        public int Studentid { get; set; }
    }
    #endregion

    //AbsenteesFollowUp
    #region Absentees Follow Up
    [MetadataType(typeof(AbsenteesFollowUp))]
    public partial class usp_select_absentees_followup_Result
    { }
    public class AbsenteesFollowUp
    {
        [Key]
        public int StudentId { get; set; }
    }
    #endregion

    //Faculty Wise Attendance
    #region Faculty Wise Attendance
    [MetadataType(typeof(FacultyWise))]
    public partial class usp_select_faculty_wise_Result
    {
    }
    public class FacultyWise
    {
        [Key]
        public string EmployeeName { get; set; }
    }
    #endregion

    //Consolidated Attendance
    #region Consolidated Attendance
    [MetadataType(typeof(Consolidated))]
    public partial class usp_select_consolidated_attendance_Result
    {
    }
    public class Consolidated
    {
        [Key]
        public string EmployeeName { get; set; }
    }
    #endregion

    //Date Wise Attendance
    #region Date Wise Attendance
    [MetadataType(typeof(DateWiseAttendance))]
    public partial class usp_select_date_wise_attendance_Result
    { }

    public class DateWiseAttendance
    {
        [Key]
        public string BatchCode { get; set; }
    }
    #endregion

    //Stduent Wise Details
    #region Student Wise Details
    [MetadataType(typeof(StudentWiseBatchDetails))]
    public partial class usp_select_student_wise_attendance_Result
    { }

    public class StudentWiseBatchDetails
    {
        [Key]
        public int StudentId { get; set; }
    }
    #endregion

    //Single Stduent Wise Details
    #region Single Student Wise Details
    [MetadataType(typeof(SingleStudentWiseBatchDetails))]
    public partial class usp_select_student_wise_attendance_detail_Result
    { }

    public class SingleStudentWiseBatchDetails
    {
        [Key]
        public int AttendanceDate { get; set; }
    }
    #endregion
    
    //Timing Wise Attendance
    #region Timing Wise Attendance
    [MetadataType(typeof(TimingWiseAttendance))]
    public partial class usp_select_timing_wise_attendance_Result
    { }

    public class TimingWiseAttendance
    {
        [Key]
        public int TSId { get; set; }
    }
    #endregion

    //Zero /No Attendance
    #region Zero /No Attendance
    [MetadataType(typeof(ZeroAttendance))]
    public partial class usp_select_zero_attendance_Result
    { }

    public class ZeroAttendance
    {
        [Key]
        public Nullable<int> StudentId { get; set; }
    }
    #endregion

    //Session Wise Attendance
    #region Session Wise Attendance
    [MetadataType(typeof(SessionWiseAttendance))]
    public partial class usp_select_session_wise_attendance_Result
    { }

    public class SessionWiseAttendance
    {
        [Key]
        public Nullable<long> OrderBy { get; set; }
    }
    #endregion

}