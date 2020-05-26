using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoice.Infrastructure.Entities
{
    public class InvoiceItem
	{
        [Key, Column(Order = 0)]
        public string InvoiceNumber { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 1)]
        public int ItemNumber { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
        
        public int Tax { get; set; }
        
        public int Quantity { get; set; }

        public InvoiceItem()
        {

        }

        public InvoiceItem(string invoiceNumber, string name, string description, decimal price, int tax, int quantity, Invoice invoice)
        {
            InvoiceNumber = invoiceNumber;
            Name = name;
            Description = description;
            Price = price;
            Tax = tax;
            Quantity = quantity;
            Invoice = invoice;
        }

        public virtual Invoice Invoice { get; set; }
    }
}
