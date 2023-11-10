using HeadHunter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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
            Resume resume = await _context.Resumes.Include(r => r.Educations).Include(r => r.WorkExperiences).FirstOrDefaultAsync(r => r.Id == id);
            return View(resume);
        }
        public async Task<IActionResult> Publish(int id)
        {
            Resume resume = await _context.Resumes.FirstOrDefaultAsync(r => r.Id == id);
            if (resume != null)
            {
                resume.IsPublished = true;
                _context.Resumes.Update(resume);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
    }
}
