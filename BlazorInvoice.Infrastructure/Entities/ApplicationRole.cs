using Microsoft.AspNetCore.Identity;

namespace BlazorInvoice.Infrastructure.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole() { }

        public ApplicationRole(string name)
        {
            Name = name;
        }
    }
}
