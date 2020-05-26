using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoice.Infrastructure.Entities
{
	public class Address
	{
		public string Street { get; set; }

		[Key, Column(Order = 0)]
		public string Number { get; set; }

		[Key, Column(Order = 0)]
		public string PostalCode { get; set; }

		public string City { get; set; }

		public string Country { get; set; }

		public Address()
		{
			Debtors = new List<Debtor>();
		}

		public Address(string street, string number, string postalCode, string city, string country)
		{
			this.Street = street;
			this.Number = number;
			this.PostalCode = postalCode;
			this.City = city;
			this.Country = country;
		}

		public virtual ICollection<Debtor> Debtors { get; set; }
	}
}
