using System.ComponentModel.DataAnnotations;

namespace TrainingApp.Api.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Email cannot be blank")]
        [EmailAddress]
        [Display(Name = "Adres e-mail: ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password cannot be blank")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło: ")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password cannot be blank")]
        [Display(Name = "Powtórz hasło: ")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords not matching")]
        public string ConfirmPassword { get; set; }
    }
}
