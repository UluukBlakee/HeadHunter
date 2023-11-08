namespace HeadHunter.Models
{
    public class Resume
    {
        public int Id { get; set; }
        public string TItle { get; set; }
        public string Category { get; set; }
        public decimal ExpectedSalary { get; set; }
        public string Telegram { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookLink { get; set; }
        public string LinkedinLink { get; set; }
        public bool IsPublished { get; set; }
        public DateTime LastUpdated { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<WorkExperience> WorkExperiences { get; set; }
        public List<Education> Educations { get; set; }
    }
}
