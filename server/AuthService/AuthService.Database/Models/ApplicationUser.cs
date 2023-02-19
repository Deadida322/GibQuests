using Microsoft.AspNetCore.Identity;

namespace AuthService.Database.Models
{
    /// <summary>
    /// расширение пользователя 
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string Img { get; set; }
    }
}
