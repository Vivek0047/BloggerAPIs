using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BloggerAPIs.DDL
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Blogs = new HashSet<Blog>();
        }

        [Column(TypeName = "nvarchar(150)")] public string FullName { get; set; }
        public ICollection<Blog> Blogs { get; set; }
    }
}