using HeadHunter.Enums;
using HeadHunter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using X.PagedList;

namespace HeadHunter.Controllers
{
    public class ResumesController : Controller
    {
        private readonly HhContext _context;
        private readonly UserManager<User> _userManager;
        public ResumesController(HhContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int? page, string? filterCategory)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            User user = await _context.Users.Include(u => u.Vacancies).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (user.Vacancies.Any())
            {
                List<Resume> resumes = await _context.Resumes.Include(v => v.User).OrderByDescending(v => v.LastUpdated).ToListAsync();
                if (filterCategory != null)
                    resumes = resumes.Where(v => v.Category == filterCategory).ToList();
                ViewBag.User = user;
                List<Resume> filteredResumes = new List<Resume>();
                foreach (var resume in resumes)
                {
                    bool userApplied = await HasUserApplied(resume.Id, user.Id);
                    if (userApplied == false)
                    {
                        filteredResumes.Add(resume);
                    }
                }
                return View(filteredResumes.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Create", "Vacancies");
            }
        }

        private async Task<bool> HasUserApplied(int resumeId, int userId)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            bool isInRole = await _userManager.IsInRoleAsync(user, "employer");
            if (isInRole)
                return _context.Responses.Any(r => r.ResumeId == resumeId && r.EmployerId == userId);
            return _context.Responses.Any(r => r.ResumeId == resumeId && r.ApplicantId == userId);
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

                if (resume.WorkExperiences != null)
                {
                    foreach (WorkExperience work in resume.WorkExperiences)
                    {
                        work.ResumeId = resume.Id;
                        await _context.Works.AddAsync(work);
                    }
                }
                if (resume.Educations != null)
                {
                    foreach (Education education in resume.Educations)
                    {
                        education.ResumeId = resume.Id;
                        await _context.Educations.AddAsync(education);
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Resume resume = await _context.Resumes.Include(r => r.Educations).Include(r => r.WorkExperiences).FirstOrDefaultAsync(r => r.Id == id);
            return View(resume);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Resume resume)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (resume != null)
            {
                resume.UserId = user.Id;
                resume.LastUpdated = DateTime.UtcNow;
                _context.Resumes.Update(resume);

                if (resume.WorkExperiences != null)
                {
                    foreach (WorkExperience work in resume.WorkExperiences)
                    {
                        work.ResumeId = resume.Id;
                        await _context.Works.AddAsync(work);
                    }
                }
                if (resume.Educations != null)
                {
                    foreach (Education education in resume.Educations)
                    {
                        education.ResumeId = resume.Id;
                        await _context.Educations.AddAsync(education);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
        public async Task<IActionResult> Update(int id)
        {
            Resume resume = await _context.Resumes.FirstOrDefaultAsync(r => r.Id == id);
            if (resume != null)
            {
                resume.LastUpdated = DateTime.UtcNow;
                _context.Resumes.Update(resume);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
        public async Task<IActionResult> Details(int id)
        {
            Resume resume = await _context.Resumes.Include(r => r.User).Include(r => r.Educations).Include(r => r.WorkExperiences).FirstOrDefaultAsync(r => r.Id == id);
            return View(resume);
        }
        public async Task<IActionResult> Publish(int id)
        {
            Resume resume = await _context.Resumes.FirstOrDefaultAsync(r => r.Id == id);
            if (resume != null)
            {
                resume.IsPublished = !resume.IsPublished;
                _context.Resumes.Update(resume);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
    }
}
