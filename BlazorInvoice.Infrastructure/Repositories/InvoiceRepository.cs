using BlazorInvoice.Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorInvoice.Infrastructure.Repositories
{
	public class InvoiceRepository
	{
		private readonly ApplicationDbContext _context;

		public InvoiceRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Invoice>> GetInvoices()
		{
			return await _context.Invoices.ToListAsync();
		}

		public async Task<List<Invoice>> GetInvoicesByDebtor(int debtorId)
		{
			return await _context.Invoices.Where(q => q.Debtor.Id == debtorId).ToListAsync();
		}

		public async Task<List<Invoice>> GetInvoicesByDebtorEmail(string email)
		{
			return await _context.Invoices.Where(q => q.Debtor.Email == email).ToListAsync();
		}

		public async Task<Invoice> GetInvoice(string invoiceNumber)
		{
			return await _context.Invoices.FirstOrDefaultAsync(q => q.InvoiceNumber == invoiceNumber);
		}

		public async Task<Invoice> Create(Invoice invoice)
		{
			await _context.Invoices.AddAsync(invoice);
			var result = await _context.SaveChangesAsync();

			return result > 0 ? invoice : null;
		}

		public async Task<Invoice> Update(Invoice invoice)
		{
			_context.Invoices.Update(invoice);
			var result = await _context.SaveChangesAsync();

			return result > 0 ? invoice : null;
		}

		public async Task<bool> Delete(Invoice invoice)
		{
			_context.Invoices.Remove(invoice);
			var result = await _context.SaveChangesAsync();

			return result > 0 ? true : false;
		}
	}
}
