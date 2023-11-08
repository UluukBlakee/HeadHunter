using HeadHunter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeadHunter.Controllers
{
    public class ResumesController : Controller
    {
        private readonly HhContext _context;
        public ResumesController(HhContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Resume resume)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (resume != null)
            {
                resume.UserId = user.Id;
                resume.LastUpdated = DateTime.UtcNow;
                resume.IsPublished = false;
                await _context.Resumes.AddAsync(resume);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(int? id, string? name)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == name);
            if (user == null)
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            return View(user);
        }
        public async Task<IActionResult> Update(int? id, string? name)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == name);
            if (user == null)
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            return View(user);
        }
        public async Task<IActionResult> Details(int? id, string? name)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == name);
            if (user == null)
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            return View(user);
        }
        public async Task<IActionResult> Publish(int? id, string? name)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == name);
            if (user == null)
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            return View(user);
        }
    }
}
