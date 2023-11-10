using System.ComponentModel.DataAnnotations;

namespace HeadHunter.Models
{
    public class Vacancy
    {
        public int Id { get; set; }
        [Display(Name = "Название вакансии")]
        [Required(ErrorMessage = "Поле 'Название вакансии' обязательно для заполнения")]
        public string? Title { get; set; }
        [Display(Name = "Заработная плата")]
        [Required(ErrorMessage = "Поле 'Заработная плата' обязательно для заполнения")]
        [Range(0, double.MaxValue, ErrorMessage = "Ожидаемая Зарплата должна быть положительным числом")]
        public int Salary { get; set; }
        [Display(Name = "Детальное описание вакансии")]
        [Required(ErrorMessage = "Поле 'Детальное описание вакансии' обязательно для заполнения")]
        public string? Description { get; set; }
        [Display(Name = "Требуемый опыт работы")]
        [Required(ErrorMessage = "Поле 'Требуемый опыт работы' обязательно для заполнения")]
        public int MinExperience { get; set; }
        [Display(Name = "Категория вакансии")]
        [Required(ErrorMessage = "Поле 'Категория вакансии' обязательно для заполнения")]
        public string? Category { get; set; }
        public bool IsPublished { get; set; }
        [Display(Name = "Последнее обновление")]
        public DateTime LastUpdated { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
