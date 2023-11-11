using HeadHunter.Enums;
using HeadHunter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace HeadHunter.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly HhContext _context;
        private readonly UserManager<User> _userManager;
        public VacanciesController(HhContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int? page, string? filterCategory, string? search, VacancySortState sortState = VacancySortState.SalaryAsc)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            User user = await _context.Users.Include(u => u.Resumes).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user.Resumes.Any())
            {
                List <Vacancy> vacancies = await _context.Vacancies.Include(v => v.User).OrderByDescending(v => v.LastUpdated).ToListAsync();
                if (search != null)
                    vacancies = vacancies.Where(v => v.Title.Contains(search)).ToList();
                if (filterCategory != null)
                    vacancies = vacancies.Where(v => v.Category == filterCategory).ToList();
                ViewBag.SalarySort = sortState == VacancySortState.SalaryAsc ? VacancySortState.SalaryDesc : VacancySortState.SalaryAsc;
                switch (sortState)
                {
                    case VacancySortState.SalaryAsc: vacancies = vacancies.OrderBy(v => v.Salary).ToList(); break;
                    case VacancySortState.SalaryDesc: vacancies = vacancies.OrderByDescending(v => v.Salary).ToList(); break;
                }
                ViewBag.User = user;
                List<Vacancy> filteredVacancies = new List<Vacancy>();
                foreach (var vacancy in vacancies)
                {
                    bool userApplied = await HasUserApplied(vacancy.Id, user.Id);
                    if (userApplied == false)
                    {
                        filteredVacancies.Add(vacancy);
                    }
                }
                return View(filteredVacancies.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Create", "Resumes");
            }
        }

        private async Task<bool> HasUserApplied(int vacancyId, int userId)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            bool isInRole = await _userManager.IsInRoleAsync(user, "employer");
            if (isInRole)
                return _context.Responses.Any(r => r.VacancyId == vacancyId && r.EmployerId == userId);
            return _context.Responses.Any(r => r.VacancyId == vacancyId && r.ApplicantId == userId);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vacancy vacancy)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (vacancy != null)
            {
                vacancy.UserId = user.Id;
                vacancy.LastUpdated = DateTime.UtcNow;
                vacancy.IsPublished = false;
                await _context.Vacancies.AddAsync(vacancy);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Vacancy vacancy = await _context.Vacancies.FirstOrDefaultAsync(v => v.Id == id);
            return View(vacancy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Vacancy vacancy)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (vacancy != null)
            {
                vacancy.UserId = user.Id;
                vacancy.LastUpdated = DateTime.UtcNow;
                _context.Vacancies.Update(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
        public async Task<IActionResult> Update(int id)
        {
            Vacancy vacancy = await _context.Vacancies.FirstOrDefaultAsync(v => v.Id == id);
            if (vacancy != null)
            {
                vacancy.LastUpdated = DateTime.UtcNow;
                _context.Vacancies.Update(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
        public async Task<IActionResult> Details(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            ViewBag.User = user;
            Vacancy vacancy = await _context.Vacancies.Include(v => v.User).ThenInclude(u => u.Resumes).FirstOrDefaultAsync(v => v.Id == id);
            return View(vacancy);
        }
        public async Task<IActionResult> Publish(int id)
        {
            Vacancy vacancy = await _context.Vacancies.FirstOrDefaultAsync(v => v.Id == id);
            if (vacancy != null)
            {
                vacancy.IsPublished = true;
                _context.Vacancies.Update(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
    }
}
