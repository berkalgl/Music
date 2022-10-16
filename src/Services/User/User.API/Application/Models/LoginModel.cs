using System.ComponentModel.DataAnnotations;

namespace User.API.Application.Models
{
    public record LoginRequest
    {
        [Required(ErrorMessage = "Email cannot be empty")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
