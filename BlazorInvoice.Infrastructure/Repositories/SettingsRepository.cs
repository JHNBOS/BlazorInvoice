using BlazorInvoice.Infrastructure.Entities;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

namespace BlazorInvoice.Infrastructure.Repositories
{
	public class SettingsRepository
	{
		private readonly ApplicationDbContext _context;

		public SettingsRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Settings> GetSettings(int id)
		{
			return await _context.Settings.FirstOrDefaultAsync(q => q.Id == id);
		}

		public async Task<Settings> GetSettingsByCompany(string name)
		{
			return await _context.Settings.FirstOrDefaultAsync(q => q.CompanyName == name);
		}

		public async Task<Settings> Create(Settings settings)
		{
			await _context.Settings.AddAsync(settings);
			var result = await _context.SaveChangesAsync();

			return result > 0 ? settings : null;
		}

		public async Task<Settings> Update(Settings settings)
		{
			_context.Settings.Update(settings);
			var result = await _context.SaveChangesAsync();

			return result > 0 ? settings : null;
		}

		public async Task<bool> Delete(Settings settings)
		{
			_context.Settings.Remove(settings);
			var result = await _context.SaveChangesAsync();

			return result > 0 ? true : false;
		}
	}
}
