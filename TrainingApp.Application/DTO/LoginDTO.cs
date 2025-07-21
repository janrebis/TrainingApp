using System.ComponentModel.DataAnnotations;

namespace TrainingApp.Application.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage ="Email should be in proper format")]
        [Display(Name = "Adres e-mail: ")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password cannot be blank")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło: ")]
        public string? Password { get; set; }
    }
}
