using BlazorInvoice.Data.Services.Interfaces;
using BlazorInvoice.Infrastructure.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorInvoice.Data.Services
{
	public class InvoiceService : IInvoiceService
	{
		public Task<Invoice> Create(Invoice invoice, List<InvoiceItem> invoiceItems) => throw new NotImplementedException();
		public Task<Invoice> Delete(Invoice invoice, List<InvoiceItem> invoiceItems) => throw new NotImplementedException();
		public Task<Invoice> Update(Invoice invoice, List<InvoiceItem> invoiceItems) => throw new NotImplementedException();
	}
}
