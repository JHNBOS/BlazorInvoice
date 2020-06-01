using BlazorInvoice.Infrastructure.Entities;

using System.Threading.Tasks;

namespace BlazorInvoice.Data.Services.Interfaces
{
	public interface IUserService
	{
		Task<User> GetUser(int id);
		Task<User> GetUserByEmail(string email);
		Task<User> GetUserByToken(string token);
	}
}
