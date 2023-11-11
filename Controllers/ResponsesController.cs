using HeadHunter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeadHunter.Controllers
{
    public class ResponsesController : Controller
    {
        private readonly HhContext _context;
        private readonly UserManager<User> _userManager;
        public ResponsesController(HhContext context, UserManager<User> userManager)
        {
            _context = context;            _userManager = userManager;

        }
        [HttpPost]
        public async Task<IActionResult> Create(int id, int resumeId)
        {
            Vacancy vacancy = await _context.Vacancies.FirstOrDefaultAsync(v => v.Id == id);
            Resume resume = await _context.Resumes.FirstOrDefaultAsync(r => r.Id == resumeId);
            User applicant = await _context.Users.FirstOrDefaultAsync(u => u.Id == resume.UserId);
            User employer = await _context.Users.FirstOrDefaultAsync(u => u.Id == vacancy.UserId);
            Response response = new Response()
            {
                EmployerId = employer.Id,
                ApplicantId = applicant.Id,
                ResumeId = resume.Id,
                VacancyId = vacancy.Id
            };
            _context.Responses.Add(response);
            await _context.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = response.Id });
        }
        public async Task<IActionResult> Chat(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Response response = await _context.Responses
                .Include(r => r.Employer)
                .Include(r => r.Applicant)
                .Include(r => r.Vacancy)
                .Include(r => r.Resume)
                .Include(r => r.Messages)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (response == null)
            {
                return NotFound();
            }
            User user = await _context.Users.Include(u => u.Vacancies).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            bool isInRole = await _userManager.IsInRoleAsync(user, "employer");
            ViewBag.Role = isInRole;
            return View(response);
        }
        public async Task<IActionResult> MyResponses(int? vacancyId)
        {
            User user = await _context.Users.Include(u => u.Vacancies).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            bool isInRole = await _userManager.IsInRoleAsync(user, "employer");
            ViewBag.Role = isInRole;
            if (isInRole)
            {
                List<Response> responses = await _context.Responses
                .Include(r => r.Employer)
                .ThenInclude(e => e.Vacancies)
                .Include(r => r.Applicant)
                .Include(r => r.Vacancy)
                .Include(r => r.Resume)
                .Include(r => r.Messages).Where(r => r.EmployerId == user.Id).ToListAsync();
                if (vacancyId != null)
                {
                    responses = await _context.Responses
                    .Include(r => r.Employer)
                    .ThenInclude(e => e.Vacancies)
                    .Include(r => r.Applicant)
                    .Include(r => r.Vacancy)
                    .Include(r => r.Resume)
                    .Include(r => r.Messages).Where(r => r.EmployerId == user.Id).Where(r => r.VacancyId == vacancyId).ToListAsync();
                }
                return View(responses);
            }
            else
            {
                List<Response> responses = await _context.Responses
                .Include(r => r.Employer)
                .Include(r => r.Applicant)
                .Include(r => r.Vacancy)
                .Include(r => r.Resume)
                .Include(r => r.Messages).Where(r => r.ApplicantId == user.Id).ToListAsync();
                return View(responses);
            }
        }
    }
}
