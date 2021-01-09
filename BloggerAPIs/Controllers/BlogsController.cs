using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggerAPIs.DDL;
using BloggerAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BloggerAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogsController : ControllerBase
    {
        private readonly BloggerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogsController(BloggerContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Blogs
        [HttpGet]
        public async Task<IEnumerable<BlogModel>> GetBlogs()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var blogs = await _context.Blogs.Where(x => x.ApplicationUser.Id == userId).ToListAsync();
            var model = blogs.Select(x => new BlogModel()
            {
                CreatedDateTime = x.CreatedDateTime, Subject = x.Subject, Body = x.Body, Id = x.Id
            }).AsEnumerable();
            return model;
        }

        // GET: api/Blogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogModel>> GetBlog(int id)
        {
            var blog = await GetUserBlog(id);

            if (blog == null)
            {
                return NotFound();
            }

            var model = new BlogModel()
            {
                CreatedDateTime = blog.CreatedDateTime, Body = blog.Body, Id = blog.Id, Subject = blog.Subject
            };
            return model;
        }

        // PUT: api/Blogs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(int id, BlogModel blog)
        {
            if (id != blog.Id)
            {
                return BadRequest();
            }

            var originalBlog = await GetUserBlog(blog.Id);
            if (originalBlog != null)
            {
                originalBlog.Body = blog.Body;
                originalBlog.Subject = blog.Subject;
                _context.Entry(originalBlog).State = EntityState.Modified;
            }
            else
            {
                return NotFound();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Blogs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBlog(Blog blog)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            blog.ApplicationUser = user;
            blog.CreatedDateTime = DateTime.Now.ToUniversalTime();
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetBlog", new {id = blog.Id});
        }

        // DELETE: api/Blogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Blog>> DeleteBlog(int id)
        {
            var userBlog = await GetUserBlog(id);
            if (userBlog == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(userBlog);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //This can be moved into service
        [NonAction]
        private async Task<Blog> GetUserBlog(int id)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var dbBlog = await _context.Blogs.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.Id == id);
                if (dbBlog == null)
                {
                    return null;
                }

                return dbBlog.ApplicationUser.Id.Equals(userId) ? dbBlog : null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}