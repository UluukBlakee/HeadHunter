using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HeadHunter.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Display(Name = "Аватар")]
        public IFormFile? Avatar { get; set; }
        [Display(Name = "Номер телефона")]
        public string? PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Соискатель или работодатель")]
        public string Role { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
