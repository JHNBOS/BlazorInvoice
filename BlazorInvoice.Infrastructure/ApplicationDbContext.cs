using BlazorInvoice.Infrastructure.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlazorInvoice.Infrastructure
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		protected ApplicationDbContext()
		{
		}

		public DbSet<Address> Addresses { get; set; }
		public DbSet<Debtor> Debtors { get; set; }
		public DbSet<InvoiceItem> InvoiceItems { get; set; }
		public DbSet<Invoice> Invoices { get; set; }
		public DbSet<Settings> Settings { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Unique properties
			modelBuilder.Entity<Debtor>().HasIndex(u => u.Email).IsUnique();

			// Composite keys
			modelBuilder.Entity<Address>().HasKey(e => new { e.Number, e.PostalCode });
			modelBuilder.Entity<InvoiceItem>().HasKey(e => new { e.InvoiceNumber, e.ItemNumber });

			// Many to many relationships
			modelBuilder.Entity<Address>().HasMany(e => e.Debtors).WithOne(e => e.Address);
			modelBuilder.Entity<Debtor>().HasOne(e => e.Address).WithMany(e => e.Debtors);
			modelBuilder.Entity<InvoiceItem>().HasOne(e => e.Invoice).WithMany(e => e.Items);

			#region Identity

			modelBuilder.Entity<ApplicationUser>().HasKey(e => e.Email);

			#endregion
		}
	}
}
