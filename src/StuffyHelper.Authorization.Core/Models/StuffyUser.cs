using EnsureThat;
using Microsoft.AspNetCore.Identity;

namespace StuffyHelper.Authorization.Core.Models
{
    public class StuffyUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        public void PatchFrom(UpdateModel model)
        {
            EnsureArg.IsNotNull(model, nameof(model));

            FirstName = model.FirstName;
            MiddleName = model.MiddleName;
            LastName = model.LastName;
            UserName = model.Username;
            Email = model.Email;
            PhoneNumber = model.Phone;
        }
    }
}
