using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_LMS.Models
{
    public class DocumentType
    {
        [Key]
        public int DocumentTypeId { get; set; }

        public string Type { get; set; }

        //Navigational properties
        public virtual ICollection<Document> Documents { get; set; }
    }
}