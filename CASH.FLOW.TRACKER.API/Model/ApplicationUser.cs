using Microsoft.AspNetCore.Identity;

namespace CASH.FLOW.TRACKER.API.Model
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
