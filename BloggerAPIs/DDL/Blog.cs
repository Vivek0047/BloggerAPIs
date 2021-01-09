using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BloggerAPIs.DDL
{
    public class Blog
    {
        [Key] public int Id { get; set; }

        [Required] public string Subject { get; set; }
        [Required] public string Body { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}