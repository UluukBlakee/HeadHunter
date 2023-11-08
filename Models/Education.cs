using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HeadHunter.Models
{
    public class Education
    {
        public int Id { get; set; }
        public int ResumeId { get; set; }
        [Display(Name = "Степень обучения")]
        [Required(ErrorMessage = "Поле 'Степень обучения' обязательно для заполнения")]
        public string Degree { get; set; }
        [Display(Name = "Учебное заведение")]
        [Required(ErrorMessage = "Поле 'Учебное заведение' обязательно для заполнения")]
        public string Institution { get; set; }
        [Display(Name = "Специализация")]
        [Required(ErrorMessage = "Поле 'Специализация' обязательно для заполнения")]
        public string FieldOfStudy { get; set; }
        [Display(Name = "Год начало")]
        [Required(ErrorMessage = "Поле 'Год начало' обязательно для заполнения")]
        public int StartYear { get; set; }
        [Display(Name = "Год окончания")]
        [Required(ErrorMessage = "Поле 'Год окончания' обязательно для заполнения")]
        public int EndYear { get; set; } 
        public Resume Resume { get; set; }
    }
}
