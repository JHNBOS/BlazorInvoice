using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoice.Infrastructure.Entities
{
	public class User
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

		public string ConfirmPassword { get; set; }
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public virtual Role Role { get; set; }

		public User()
		{
		}

		public User(string username, string email)
		{
			Username = username;
			Email = email;
		}

		public User(string username, string email, string password)
		{
			Username = username;
			Email = email;
			Password = password;
		}
	}
}
