//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AptechRecord.Models
{
    using System;
    
    public partial class usp_select_student_attendance_Result
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        public string AttenddanceStatus { get; set; }
        public string Notes { get; set; }
        public string Username { get; set; }
        public string BatchCode { get; set; }
        public string Slot { get; set; }
        public string BatchDays { get; set; }
        public string contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}