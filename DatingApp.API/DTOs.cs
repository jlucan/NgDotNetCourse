using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength (8, MinimumLength=4, ErrorMessage = "Password must be between 4 and 8 characters.")]
        public string Password { get; set; }        
    }

    public class UserLoginDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }        
    }

} 