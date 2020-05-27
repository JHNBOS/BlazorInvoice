using Microsoft.AspNetCore.Identity;

using System;

namespace BlazorInvoice.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser<int>
	{
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ApplicationUser()
        {
        }

        public ApplicationUser(string gebruikersNaam) : base(gebruikersNaam)
        {
        }
    }
}
