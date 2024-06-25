using Microsoft.AspNetCore.Identity;

namespace NGOCXMHT_Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; } = string.Empty;
    }
}