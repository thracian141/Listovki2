using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Listovki.Models {
    public class AppUser : IdentityUser<string> {
        [Key]
        public override string Email { get; set; }
        [Required]
        public override string UserName { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string EGN { get; set; }
        public string? ProfilePictureURL { get; set; }
        [Required]
        public int PassedExams { get; set; } = 0;
        [Required]
        public int FailedExams { get; set; } = 0;
        [Required]
        public bool IsAdmin { get; set; } = false;
    }
}
