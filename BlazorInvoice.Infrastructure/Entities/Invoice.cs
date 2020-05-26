using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoice.Infrastructure.Entities
{
    public class Invoice
	{
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string InvoiceNumber { get; set; }

        public string CustomerId { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public DateTime ExpiredOn { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Total { get; set; }
        
        public int Discount { get; set; }
        
        public string Comment { get; set; }
        
        public bool IsPaid { get; set; }
        
        public bool Concept { get; set; }

        public Invoice()
        {
            Items = new List<InvoiceItem>();
        }

        public Invoice(string invoiceNumber, string customerId, DateTime createdOn, DateTime expiredOn, decimal total, int discount, string comment, bool isPaid, bool concept, ICollection<InvoiceItem> items, Debtor debtor)
        {
            InvoiceNumber = invoiceNumber;
            CustomerId = customerId;
            CreatedOn = createdOn;
            ExpiredOn = expiredOn;
            Total = total;
            Discount = discount;
            Comment = comment;
            IsPaid = isPaid;
            Concept = concept;
            Items = items;
            Debtor = debtor;
        }

        public virtual ICollection<InvoiceItem> Items { get; set; }
        public virtual Debtor Debtor { get; set; }
    }
}
