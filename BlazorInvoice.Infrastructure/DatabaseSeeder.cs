using BlazorInvoice.Infrastructure.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlazorInvoice.Infrastructure
{
	public class DatabaseSeeder
	{
		public static async Task SeedUsers(UserManager<ApplicationUser> userManager, ILogger<DatabaseSeeder> logger)
		{
			if (userManager.FindByEmailAsync("bosbosjohan@gmail.com").Result == null)
			{
				ApplicationUser user = new ApplicationUser()
				{
					Email = "bosbosjohan@gmail.com",
					UserName = "Admin",
					NormalizedEmail = ("bosbosjohan@gmail.com").ToUpper(),
					NormalizedUserName = ("Admin").ToUpper(),
					PhoneNumber = "123",
					Password = "welkom2020",
					PasswordHash = string.Empty,
					FirstName = "Johan",
					LastName = "Bos"
                };

				user.PasswordHash = GenerateHash(user);

			IdentityResult result = await userManager.CreateAsync(user, "welkom2020");

			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(user, "Administrator");
				logger.LogTrace($"Set password welkom2020 for default user bosbosjohan@gmail.com successfully");
			}
			else
			{
				logger.LogError($"Password for the user `{user.Email}` couldn't be set");
			}
		}
	}

	public static async Task SeedRoles(RoleManager<IdentityRole> roleManager, ILogger<DatabaseSeeder> logger)
	{
		string[] roles = new string[] { "Debtor", "Administrator", "Employee" };

		foreach (string role in roles)
		{
			var result = await roleManager.CreateAsync(new IdentityRole(role));
			if (result.Succeeded)
			{
				logger.LogTrace($"Created role {role} successfully");
			}
			else
			{
				logger.LogError($"Role {role} couldn't be created!");
			}
		}
	}

	private static string GenerateHash(ApplicationUser user)
	{
		var passHash = new PasswordHasher<ApplicationUser>();
		return passHash.HashPassword(user, "welkom2020");
	}
}
}
