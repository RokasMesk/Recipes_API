using System.ComponentModel.DataAnnotations;

namespace Recipe.Models.DTO
{
    public class ChangePasswordDTO
    {
        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
