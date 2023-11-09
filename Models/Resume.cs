using System.ComponentModel.DataAnnotations;

namespace HeadHunter.Models
{
    public class Resume
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        public string? Title { get; set; }
        [Display(Name = "Категория")]
        public string? Category { get; set; }

        [Display(Name = "Ожидаемая Зарплата")]
        [Required(ErrorMessage = "Поле 'Ожидаемая Зарплата' обязательно для заполнения")]
        [Range(0, double.MaxValue, ErrorMessage = "Ожидаемая Зарплата должна быть положительным числом")]
        public decimal ExpectedSalary { get; set; }
        [Display(Name = "Телеграм")]
        [Required(ErrorMessage = "Поле 'Телеграм' обязательно для заполнения")]
        public string? Telegram { get; set; }
        [Display(Name = "Электронная почта")]
        [Required(ErrorMessage = "Поле 'Электронная почта' обязательно для заполнения")]
        [EmailAddress(ErrorMessage = "Неверный адрес электронной почты")]
        public string? Email { get; set; }
        [Display(Name = "Номер телефона")]
        [Required(ErrorMessage = "Поле 'Номер телефона' обязательно для заполнения")]
        [Phone(ErrorMessage = "Неверный номер телефона")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Ссылка на Facebook")]
        public string? FacebookLink { get; set; }
        [Display(Name = "Ссылка на Linkedin")]
        public string? LinkedinLink { get; set; }
        public bool IsPublished { get; set; }
        [Display(Name = "Последнее обновление")]
        public DateTime LastUpdated { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<WorkExperience>? WorkExperiences { get; set; }
        public List<Education>? Educations { get; set; }
    }
}
