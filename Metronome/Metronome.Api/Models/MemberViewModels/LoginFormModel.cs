using System.ComponentModel.DataAnnotations;

namespace Metronome.Api.Models.MemberViewModels
{
    public class LoginFormModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
