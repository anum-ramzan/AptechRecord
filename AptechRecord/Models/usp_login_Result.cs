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
    
    public partial class usp_login_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Nullable<int> UserRole { get; set; }
        public string Userstatus { get; set; }
        public Nullable<System.DateTime> UserCreatedOn { get; set; }
    }
}