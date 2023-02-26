using Microsoft.AspNetCore.Identity;

namespace AuthService.Database.Models
{
    public class ApplicationUserRole : IdentityRole<int>
    {
        public ApplicationUserRole() { }

        public ApplicationUserRole(string name)
        {
            Name = name;
        }
    }
}
