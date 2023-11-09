using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HeadHunter.Models
{
    public class WorkExperience
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        [Display(Name = "Год начало работы")]
        [Required(ErrorMessage = "Поле 'Год начало работы' обязательно для заполнения")]
        public int StartYear { get; set; }
        [Display(Name = "Год окончания работы")]
        [Required(ErrorMessage = "Поле 'Год окончания работы' обязательно для заполнения")]
        public int EndYear { get; set; }
        [Display(Name = "Названия компании")]
        [Required(ErrorMessage = "Поле 'Название компании' обязательно для заполнения")]
        public string?  CompanyName { get; set; }
        [Display(Name = "Должность")]
        [Required(ErrorMessage = "Поле 'Должность' обязательно для заполнения")]
        public string? Position { get; set; }
        [Display(Name = "Обязанности")]
        [Required(ErrorMessage = "Поле 'Обязанности' обязательно для заполнения")]
        public string? Responsibilities { get; set; }
        public Resume? Resume { get; set; }
    }
}
