using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618
#pragma warning disable S101

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
