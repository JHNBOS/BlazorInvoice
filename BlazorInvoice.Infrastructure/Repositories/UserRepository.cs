using BlazorInvoice.Infrastructure.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorInvoice.Infrastructure.Repositories
{
	public class UserRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<List<ApplicationUser>> GetUsers()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<ApplicationUser> GetUser(int id)
		{
			return await _context.Users.FirstOrDefaultAsync(q => q.Id == id);
		}

		public async Task<ApplicationUser> GetUserByEmail(string email)
		{
			return await _context.Users.FirstOrDefaultAsync(q => q.Email == email);
		}

		public async Task<ApplicationUser> Create(ApplicationUser user)
		{
			var result = await _userManager.CreateAsync(user, user.PasswordHash);

			if (result.Succeeded)
			{
				return user;
			}

			return null;
		}

		public async Task<ApplicationUser> Update(ApplicationUser user)
		{
			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				return user;
			}

			return null;
		}

		public async Task<bool> Delete(ApplicationUser user)
		{
			await _userManager.DeleteAsync(user);
			var result = await _context.SaveChangesAsync();

			return result > 0 ? true : false;
		}
	}
}
