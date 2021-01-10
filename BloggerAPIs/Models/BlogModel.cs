using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BloggerAPIs.Models
{
    public class BlogModel
    {
        [Required] public int Id { get; set; }
        [Required] public string Subject { get; set; }
        [Required] public string Body { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}