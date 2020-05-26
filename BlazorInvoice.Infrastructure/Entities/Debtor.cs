using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoice.Infrastructure.Entities
{
    public class Debtor
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Email { get; set; }
        
        public string BankAccount { get; set; }
        
        public string Phone { get; set; }
        
        public DebtorType Type { get; set; }

        public Debtor()
        {
            Invoices = new List<Invoice>();
        }

        public Debtor(string name, string email, string bankAccount, string phone, DebtorType type, Address address, ICollection<Invoice> invoices)
        {
            Name = name;
            Email = email;
            BankAccount = bankAccount;
            Phone = phone;
            Type = type;
            Address = address;
            Invoices = invoices;
        }

        public virtual Address Address { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }

    public enum DebtorType
    {
        Private,
        Company
    }
}
