using Microsoft.AspNetCore.Identity;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoice.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Guid AccountGuid { get; set; }
        
        public string Salt { get; set; }
        
        public bool Verified { get; set; }
        
        public string Checksum { get; set; }
        
        public string Password { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Updated { get; set; }

        public ApplicationUser()
        {
            AccountGuid = new Guid();
            ConcurrencyStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            SecurityStamp = new Guid().ToString("D");
        }
    }
}
