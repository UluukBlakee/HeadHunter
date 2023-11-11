namespace HeadHunter.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ResponseId { get; set; }
        public Response? Response { get; set; } 
        public int SenderId { get; set; }
        public User? Sender { get; set; }
        public string? Text { get; set; }
        public DateTime? DepartureDate { get; set; }
    }
}
