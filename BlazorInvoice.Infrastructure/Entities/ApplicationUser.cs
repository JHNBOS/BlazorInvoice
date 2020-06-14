using Microsoft.AspNetCore.Identity;

using System.Linq;

namespace BlazorInvoice.Infrastructure.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName { get { return $"{FirstName} {LastName}"; } }
		public string Initials { get { return $"{FirstName.First()}{LastName.First()}"; } }
	}
}
