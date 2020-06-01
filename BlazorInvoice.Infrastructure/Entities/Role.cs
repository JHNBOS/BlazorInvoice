using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoice.Infrastructure.Entities
{
	public class Role
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		public Role()
		{
			Users = new List<User>();
		}

		public Role(string name)
		{
			Users = new List<User>();
			Name = name;
		}

		public virtual ICollection<User> Users { get; set; }
	}
}
