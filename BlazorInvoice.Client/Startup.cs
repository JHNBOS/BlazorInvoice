using BlazorInvoice.Client.Areas.Identity;
using BlazorInvoice.Client.Data;
using BlazorInvoice.Infrastructure;
using BlazorInvoice.Infrastructure.Entities;
using BlazorInvoice.Infrastructure.Repositories;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PreRenderComponent;

using System;
using System.IO;

namespace BlazorInvoice.Client
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;

			var builder = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json");

			Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("BlazorInvoiceDatabase"),
				   b => b.MigrationsAssembly("BlazorInvoice.Infrastructure"))
				.UseLazyLoadingProxies()
			);

			services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
			{
				options.SignIn.RequireConfirmedAccount = false;
			})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddHttpContextAccessor();
			services.AddControllersWithViews();
			services.AddRazorPages();
			services.AddServerSideBlazor();

			services.AddSingleton<WeatherForecastService>();

			services.AddTransient<UserRepository>();
			services.AddTransient<DebtorRepository>();
			services.AddTransient<InvoiceRepository>();
			services.AddTransient<SettingsRepository>();

			services.AddScoped<IPreRenderFlag, PreRenderFlag>();
			services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
			services.AddScoped<IHostEnvironmentAuthenticationStateProvider>(sp => {
				// this is safe because 
				//     the `RevalidatingIdentityAuthenticationStateProvider` extends the `ServerAuthenticationStateProvider`
				var provider = (ServerAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>();
				return provider;
			});

			services.Configure<IdentityOptions>(options =>
			{
				// Default Lockout settings.
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = false;
			});

			services.AddServerSideBlazor().AddCircuitOptions(options =>
			{
				options.DetailedErrors = true;
			}).AddHubOptions(o =>
			{
				o.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
			});

			services.ConfigureApplicationCookie(options =>
			{
				// Cookie settings
				options.Cookie.HttpOnly = false;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

				options.LoginPath = "/Account/Login";
				options.AccessDeniedPath = "/Account/AccessDenied";
				options.SlidingExpiration = true;
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
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

			app.Seed(logger);

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
		public static void Seed(this IApplicationBuilder app, ILogger logger)
		{
			logger.LogInformation("Updating database...");
			using var serviceScope = app.ApplicationServices.CreateScope();

			var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
			var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
			var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

			SeedRoles(context, roleManager, logger);
			SeedUsers(context, userManager, logger);

			logger.LogInformation("Done updating database...");
		}

		public static void SeedUsers(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger logger)
		{
			if (userManager.FindByEmailAsync("bosbosjohan@gmail.com").Result == null)
			{
				ApplicationUser user = new ApplicationUser()
				{
					Email = "bosbosjohan@gmail.com",
					UserName = "Admin",
					NormalizedEmail = ("bosbosjohan@gmail.com").ToUpper(),
					NormalizedUserName = ("Admin").ToUpper(),
					FirstName = "Johan",
					LastName = "Bos",
					EmailConfirmed = true,
					LockoutEnabled = false,
					AccessFailedCount = 0,
					PhoneNumber = "XXX",
					PhoneNumberConfirmed = true,
					ConcurrencyStamp = new Guid().ToString("D"),
					SecurityStamp = new Guid().ToString("D") + DateTime.Now.ToString("ddMMYYYYHHss"),
				};

				var userResult = userManager.CreateAsync(user, "Welkom@2020").Result;
				context.SaveChanges();

				if (userResult.Succeeded)
				{
					userManager.AddToRoleAsync(user, "Administrator").Wait();
					context.SaveChanges();

					logger.LogInformation($"Set password 'Welkom@2020' for default user 'bosbosjohan@gmail.com' successfully");
				}
				else
				{
					logger.LogError($"Password for the user `{user.Email}` couldn't be set");
				}
			}
		}

		public static void SeedRoles(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, ILogger logger)
		{
			string[] roles = new string[] { "Debtor", "Administrator", "Employee" };

			foreach (string role in roles)
			{
				if (roleManager.FindByNameAsync(role).Result == null)
				{
					var appRole = new ApplicationRole(role);
					var appRoleResult = roleManager.CreateAsync(appRole).Result;

					if (appRoleResult.Succeeded)
					{
						logger.LogTrace($"Created role {role} successfully");
					}
					else
					{
						logger.LogError($"Role {role} couldn't be created!");
					}
				}
			}

			context.SaveChanges();
		}

		private static string GenerateHash(ApplicationUser user)
		{
			var passHash = new PasswordHasher<ApplicationUser>();
			return passHash.HashPassword(user, "welkom2020");
		}
	}
}
