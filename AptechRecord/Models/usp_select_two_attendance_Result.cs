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
    
    public partial class usp_select_two_attendance_Result
    {
        public int Id { get; set; }
        public Nullable<int> AStudentId { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        public string AttenddanceStatus { get; set; }
        public Nullable<System.DateTime> AttendanceMarkAt { get; set; }
        public Nullable<int> AttendanceMarkBy { get; set; }
        public string ANotes { get; set; }
        public int SBId { get; set; }
        public Nullable<int> SBStudentId { get; set; }
        public string SBBatchCode { get; set; }
        public Nullable<System.DateTime> AssignDate { get; set; }
        public string SBBatchStatus { get; set; }
        public Nullable<int> SBChangesDoneBy { get; set; }
        public string SBNotes { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string NICNumber { get; set; }
        public Nullable<int> StudentCourse { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public Nullable<int> SChangesDoneBy { get; set; }
        public string SNotes { get; set; }
        public string BBatchCode { get; set; }
        public Nullable<int> BatchTiming { get; set; }
        public Nullable<System.DateTime> BatchStartDate { get; set; }
        public string BBatchStatus { get; set; }
        public Nullable<int> BatchBy { get; set; }
        public string BNotes { get; set; }
        public string BatchDays { get; set; }
        public string Slot { get; set; }
        public int UId { get; set; }
        public string Name { get; set; }
    }
}
