using BlazorInvoice.Infrastructure.Entities;

using System.Threading.Tasks;

namespace BlazorInvoice.Data.Services.Interfaces
{
	public interface IDebtorService
	{
		Task<Debtor> Create(Debtor debtor);
		Task<Debtor> Update(Debtor debtor);
		Task<Debtor> Delete(Debtor debtor);
	}
}
