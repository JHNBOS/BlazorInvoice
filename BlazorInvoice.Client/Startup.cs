using Blazored.LocalStorage;

using BlazorInvoice.Client.Areas.Identity;
using BlazorInvoice.Data.Services;
using BlazorInvoice.Infrastructure;
using BlazorInvoice.Infrastructure.Entities;
using BlazorInvoice.Infrastructure.Repositories;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.IO;
using System.Threading.Tasks;

namespace BlazorInvoice.Client
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("BlazorInvoiceDatabase"),
					b => b.MigrationsAssembly("BlazorInvoice.Infrastructure")));
			services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
					.AddRoles<IdentityRole>()
					.AddEntityFrameworkStores<ApplicationDbContext>()
					.AddDefaultTokenProviders();
			services.AddRazorPages();
			services.AddServerSideBlazor().AddCircuitOptions(options =>
			{
				options.DetailedErrors = true;
			}).AddHubOptions(o =>
			{
				o.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
			});
			services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

			//services.Configure<IdentityOptions>(options =>
			//{
			//	// Default Lockout settings.
			//	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			//	options.Lockout.MaxFailedAccessAttempts = 5;
			//	options.Lockout.AllowedForNewUsers = false;
			//});

			//services.ConfigureApplicationCookie(options =>
			//{
			//	// Cookie settings
			//	options.Cookie.HttpOnly = false;
			//	options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

			//	options.LoginPath = "/Account/Login";
			//	options.AccessDeniedPath = "/Account/AccessDenied";
			//	options.SlidingExpiration = true;
			//});

			services.AddHttpContextAccessor();
			services.AddControllersWithViews();
			services.AddBlazoredLocalStorage();
			services.AddHttpClient();

			services.AddTransient<DebtorRepository>();
			services.AddTransient<InvoiceRepository>();
			services.AddTransient<SettingsRepository>();

			services.AddTransient<DebtorService>();
			services.AddTransient<InvoiceService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILogger<Startup> logger)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			// Seed user and roles
			app.Seed(serviceProvider, logger).Wait();

			app.UseExceptionHandler(errorApp =>
			{
				errorApp.Run(async context =>
				{
					context.Response.StatusCode = 500;
					context.Response.ContentType = "text/html";

					await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
					await context.Response.WriteAsync("ERROR!<br><br>\r\n");

					var exceptionHandlerPathFeature =
						context.Features.Get<IExceptionHandlerPathFeature>();

					await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.Message + "<br/><br/>");
					await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.StackTrace);

					logger.LogError("Error" + exceptionHandlerPathFeature.Error.Message);

					if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
					{
						await context.Response.WriteAsync("File error thrown!<br><br>\r\n");
					}

					await context.Response.WriteAsync("<a href=\"/\">Home</a><br>\r\n");
					await context.Response.WriteAsync("</body></html>\r\n");
					await context.Response.WriteAsync(new string(' ', 512)); // IE padding
				});
			});

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});

			logger.LogInformation("Done configuring application...");
		}
	}

	public static class DatabaseSeeder
	{
		public static async Task Seed(this IApplicationBuilder app, IServiceProvider serviceProvider, ILogger logger)
		{
			logger.LogInformation("Checking if database needs updating...");

			var context = serviceProvider.GetService<ApplicationDbContext>();
			var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
			var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

			var userExists = await userManager.FindByEmailAsync("bosbosjohan@gmail.com");
			if (userExists == null)
			{
				await SeedRoles(context, roleManager, logger);
				await SeedUsers(context, userManager, logger);
			}

			logger.LogInformation("Done updating database...");
		}

		public static async Task SeedUsers(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger logger)
		{
			var userExists = await userManager.FindByEmailAsync("bosbosjohan@gmail.com");
			if (userExists == null)
			{
				logger.LogInformation("Updating database...");

				var user = new ApplicationUser()
				{
					Email = "bosbosjohan@gmail.com",
					UserName = "JohanBos",
					FirstName = "Johan",
					LastName = "Bos",
					NormalizedEmail = ("bosbosjohan@gmail.com").ToUpper(),
					NormalizedUserName = ("JohanBos").ToUpper(),
					EmailConfirmed = true,
					LockoutEnabled = false,
					AccessFailedCount = 0,
					PhoneNumber = "XXX",
					PhoneNumberConfirmed = true,
					ConcurrencyStamp = new Guid().ToString("D"),
					SecurityStamp = new Guid().ToString("D") + DateTime.Now.ToString("ddMMYYYYHHss"),
				};

				var userResult = await userManager.CreateAsync(user, "Welkom@2020");
				await context.SaveChangesAsync();

				if (userResult.Succeeded)
				{
					await userManager.AddToRoleAsync(user, "Administrator");
					await context.SaveChangesAsync();

					logger.LogInformation($"Set password 'Welkom@2020' for default user '{user.Email}' successfully");
				}
				else
				{
					logger.LogError($"Password for default user '{user.Email}' couldn't be set");
				}
			}
		}

		public static async Task SeedRoles(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, ILogger logger)
		{
			string[] roles = new string[] { "Debtor", "Administrator", "Employee" };

			foreach (string role in roles)
			{
				var roleExists = await roleManager.RoleExistsAsync(role);
				if (!roleExists)
				{
					var appRole = await roleManager.CreateAsync(new IdentityRole(role));

					if (appRole.Succeeded)
					{
						logger.LogTrace($"Created role for '{role}' successfully");
					}
					else
					{
						logger.LogError($"Role for '{role}' couldn't be created!");
					}
				}
			}

			await context.SaveChangesAsync();
		}
	}
}
