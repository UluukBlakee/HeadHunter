using HeadHunter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeadHunter.Controllers
{
    public class MessagesController : Controller
    {
        private readonly HhContext _context;
        public MessagesController(HhContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetMessage(int id)
        {
            List<Message> messages = await _context.Messages.Include(m => m.Sender).OrderByDescending(m => m.DepartureDate).Where(m => m.ResponseId == id).Take(30).ToListAsync();
            return Json(messages);
        }
        [HttpPost]
        public async Task<IActionResult> AddMessage(string userName, string text, int responseId)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            Message message = new Message
            {
                Text = text,
                SenderId = user.Id,
                ResponseId = responseId,
                DepartureDate = DateTime.UtcNow
            };
            if (message != null)
            {
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
            }
            return Json(message);
        }
    }
}
