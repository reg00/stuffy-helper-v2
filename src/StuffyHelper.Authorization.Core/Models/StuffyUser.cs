using Microsoft.AspNetCore.Identity;

namespace StuffyHelper.Authorization.Core.Models
{
    public class StuffyUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
    }
}
