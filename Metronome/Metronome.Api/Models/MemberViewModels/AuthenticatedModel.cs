using System.ComponentModel.DataAnnotations;
using Metronome.Api.Authentication.Jwt;

namespace Metronome.Api.Models.MemberViewModels
{
    public class AuthenticatedModel
    {
        [Required]
        [DataType(DataType.Url)]
        public string SpaHost { get; set; }

        [Required]
        [EmailAddress]
        public string MemberEmail { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Provider { get; set; }

        [Required]
        public AuthToken Token { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string BreackPadding { get; set; }


    }
}
