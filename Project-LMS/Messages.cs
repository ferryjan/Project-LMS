//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_LMS
{
    using System;
    using System.Collections.Generic;
    
    public partial class Messages
    {
        public int Id { get; set; }
        public string SentFrom { get; set; }
        public string SentTo { get; set; }
        public System.DateTime SentDate { get; set; }
        public bool isRead { get; set; }
        public string Topic { get; set; }
        public string Msg { get; set; }
        public string MessageBoxNumber { get; set; }
        public string FirstPersonLeft { get; set; }
        public string SecondPersonLeft { get; set; }
        public bool isPublic { get; set; }
        public string SentFromFullName { get; set; }
        public string SentToFullName { get; set; }
    }
}
