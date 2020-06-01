using BlazorInvoice.Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorInvoice.Infrastructure.Repositories
{
	public class UserRepository
	{
		private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<User>> GetUsers()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<User> GetUser(int id)
		{
			return await _context.Users.FirstOrDefaultAsync(q => q.Id == id);
		}

		public async Task<User> GetUserByEmail(string email)
		{
			return await _context.Users.FirstOrDefaultAsync(q => q.Email == email);
		}

		public async Task<User> Create(User user)
		{
			var result = await _context.Users.AddAsync(user);
			if(result.IsKeySet)
			{
				return user;
			}

			return null;
		}

		public async Task<User> Update(User user)
		{
			_context.Users.Update(user);
			await _context.SaveChangesAsync();

			return user;
		}

		public async Task<bool> Delete(User user)
		{
			_context.Users.Remove(user);
			var result = await _context.SaveChangesAsync();

			return result > 0 ? true : false;
		}
	}
}
