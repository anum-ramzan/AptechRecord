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
    using System.Collections.Generic;
    
    public partial class Day
    {
        public Day()
        {
            this.Batches = new HashSet<Batch>();
            this.Batches1 = new HashSet<Batch>();
            this.SlotAssigns = new HashSet<SlotAssign>();
            this.SlotAssigns1 = new HashSet<SlotAssign>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Batch> Batches { get; set; }
        public virtual ICollection<Batch> Batches1 { get; set; }
        public virtual ICollection<SlotAssign> SlotAssigns { get; set; }
        public virtual ICollection<SlotAssign> SlotAssigns1 { get; set; }
    }
}