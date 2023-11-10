﻿using HeadHunter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeadHunter.Controllers
{
    public class VacanciesController : Controller
    {
        private readonly HhContext _context;
        public VacanciesController(HhContext context)
        {
            _context = context;
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
            Vacancy vacancy = await _context.Vacancies.FirstOrDefaultAsync(v => v.Id == id);
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
