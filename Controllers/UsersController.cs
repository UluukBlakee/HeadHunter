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
        public UsersController(HhContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
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
            user.UserName = name;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return Json(user);
        }

    }
}
