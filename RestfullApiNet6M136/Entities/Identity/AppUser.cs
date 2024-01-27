using Microsoft.AspNetCore.Identity;

namespace RestfullApiNet6M136.Entities.Identity
{
    public class AppUser: IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndTime { get; set; }
    }

    public class AppRole : IdentityRole<string>
    {

    }

    public class AppUserRoles : IdentityUserRole<string>
    {

    }
}
