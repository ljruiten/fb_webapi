using System.ComponentModel.DataAnnotations;

namespace fb_webapi.ViewModels {
    public class CreateAccountModel {
        [Required]
        [StringLengthAttribute(20, MinimumLength = 6)]
        public string Username { get; set; }
        [Required]
        [StringLengthAttribute(50, MinimumLength = 6)]
        public string Email { get; set;}
        [Required]
        [StringLengthAttribute(30, MinimumLength = 6)]
        public string Password { get; set; }
    }
}