using BlazorInvoice.Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorInvoice.Infrastructure.Repositories
{
	public class DebtorRepository
	{
		private readonly ApplicationDbContext _context;

		public DebtorRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Debtor>> GetDebtors()
		{
			return await _context.Debtors.ToListAsync();
		}

		public async Task<Debtor> GetDebtor(int id)
		{
			return await _context.Debtors.FirstOrDefaultAsync(q => q.Id == id);
		}

		public async Task<Debtor> GetDebtorByEmail(string email)
		{
			return await _context.Debtors.FirstOrDefaultAsync(q => q.Email == email);
		}

		public async Task<Debtor> Create(Debtor debtor)
		{
			await _context.Debtors.AddAsync(debtor);
			var result = await _context.SaveChangesAsync();

			return result > 0 ? debtor : null;
		}

		public async Task<Debtor> Update(Debtor debtor)
		{
			_context.Debtors.Update(debtor);
			var result = await _context.SaveChangesAsync();

			return result > 0 ? debtor : null;
		}

		public async Task<bool> Delete(Debtor debtor)
		{
			_context.Debtors.Remove(debtor);
			var result = await _context.SaveChangesAsync();

			return result > 0 ? true : false;
		}
	}
}
