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
    
    public partial class usp_select_unmarked_attendance_Result
    {
        public string BatchCode { get; set; }
        public Nullable<int> BatchTiming { get; set; }
        public Nullable<System.DateTime> BatchStartDate { get; set; }
        public string BatchStatus { get; set; }
        public Nullable<int> BatchBy { get; set; }
        public string Notes { get; set; }
        public string BatchDays { get; set; }
        public int SAId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> SlotId { get; set; }
        public string SlotDays { get; set; }
        public Nullable<System.DateTime> AssignDate { get; set; }
        public string TaskStatus { get; set; }
        public int UId { get; set; }
        public string Name { get; set; }
        public int TId { get; set; }
        public string Slot { get; set; }
    }
}