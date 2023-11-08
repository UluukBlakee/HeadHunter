using HeadHunter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace HeadHunter.Controllers
{
    public class UsersController : Controller
    {
        private readonly HhContext _context;
        private readonly IWebHostEnvironment appEnvironment;
        public UsersController(HhContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            this.appEnvironment = appEnvironment;
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

        [HttpPut]
        public async Task<IActionResult> Edit(int id, string email, string phoneNumber, string name)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            user.Email = email;
            user.PhoneNumber = phoneNumber;
            user.Name = name;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return Json(user);
        }
        [HttpPut]
        public async Task<IActionResult> EditPhoto(int id, IFormFile photo)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (photo != null && photo.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                string filePath = Path.Combine(appEnvironment.WebRootPath, "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }
                string imagePath = $"/images/{fileName}";
                user.Avatar = imagePath;
            }
            _context.Update(user);
            await _context.SaveChangesAsync();
            return Json(user);
        }
    }
}
