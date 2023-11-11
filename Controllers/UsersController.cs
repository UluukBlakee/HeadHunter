﻿using HeadHunter.Models;
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
        private readonly UserManager<User> _userManager;
        public UsersController(HhContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
        {
            _context = context;
            this.appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Details(int? id, string? name)
        {
            User user = await _context.Users.Include(u => u.Resumes).ThenInclude(r => r.WorkExperiences).Include(u => u.Resumes).ThenInclude(r => r.Educations).Include(u => u.Vacancies).FirstOrDefaultAsync(u => u.UserName == name);
            if (user == null)
            {
                user = await _context.Users.Include(u => u.Resumes).ThenInclude(r => r.WorkExperiences).Include(u => u.Resumes).ThenInclude(r => r.Educations).Include(u => u.Vacancies).FirstOrDefaultAsync(u => u.Id == id);
            }
            bool isInRole = await _userManager.IsInRoleAsync(user, "employer");
            ViewBag.Role = isInRole;

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
