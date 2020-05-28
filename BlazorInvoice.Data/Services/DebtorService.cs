using BlazorInvoice.Data.Services.Interfaces;
using BlazorInvoice.Infrastructure.Entities;

using System;
using System.Threading.Tasks;

namespace BlazorInvoice.Data.Services
{
	public class DebtorService : IDebtorService
	{
		public Task<Debtor> Create(Debtor debtor) => throw new NotImplementedException();
		public Task<Debtor> Delete(Debtor debtor) => throw new NotImplementedException();
		public Task<Debtor> Update(Debtor debtor) => throw new NotImplementedException();
	}
}
