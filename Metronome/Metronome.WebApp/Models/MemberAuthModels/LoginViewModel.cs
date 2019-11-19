// SYSTEM

using System.ComponentModel.DataAnnotations;

namespace Metronome.WebApp.Models.MemberAuthModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
