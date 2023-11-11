namespace HeadHunter.Models
{
    public class Response
    {
        public int Id { get; set; }
        public int EmployerId { get; set; }
        public User? Employer { get; set; }
        public int ApplicantId { get; set; }
        public User? Applicant { get; set; }
        public int VacancyId { get; set; }
        public Vacancy? Vacancy { get; set; }
        public int ResumeId { get; set; }
        public Resume? Resume { get; set; }
        public List<Message>? Messages { get; set; }
    }
}
