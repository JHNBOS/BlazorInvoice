using BlazorInvoice.Infrastructure.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorInvoice.Data.Services.Interfaces
{
	public interface IInvoiceService
	{
		Task<Invoice> Create(Invoice invoice, List<InvoiceItem> invoiceItems);
		Task<Invoice> Update(Invoice invoice, List<InvoiceItem> invoiceItems);
		Task<Invoice> Delete(Invoice invoice, List<InvoiceItem> invoiceItems);
	}
}
