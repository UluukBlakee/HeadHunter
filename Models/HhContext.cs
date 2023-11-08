using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HeadHunter.Models
{
    public class HhContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<WorkExperience> Works { get; set; }
        public DbSet<Education> Educations { get; set; }
        public HhContext(DbContextOptions<HhContext> options) : base(options) { }
    }
}
