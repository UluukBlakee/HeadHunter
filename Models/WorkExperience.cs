namespace HeadHunter.Models
{
    public class WorkExperience
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string Responsibilities { get; set; }
        public Resume Resume { get; set; }
    }
}
