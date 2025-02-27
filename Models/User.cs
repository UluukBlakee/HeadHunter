﻿using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace HeadHunter.Models
{
    public class User : IdentityUser<int>
    {
        public string? Name { get; set; }
        public string? Avatar { get; set; }
        public List<Resume>? Resumes { get; set; }
        public List<Vacancy>? Vacancies { get; set; }
    }
}
