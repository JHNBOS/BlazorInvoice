using BlazorInvoice.Infrastructure.Entities;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorInvoice.Infrastructure
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
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

			//modelBuilder.Entity<ApplicationUser>(entity =>
			//{
			//    entity.ToTable(name: "AspNetUser", schema: "Security");
			//    entity.Property(e => e.Id).HasColumnName("AspNetUserId");
			//});

			//modelBuilder.Entity<ApplicationRole>(entity =>
			//{
			//    entity.ToTable(name: "AspNetRole", schema: "Security");
			//    entity.Property(e => e.Id).HasColumnName("AspNetRoleId");
			//});

			//modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
			//{
			//    entity.ToTable("AspNetUserClaim", "Security");
			//    entity.Property(e => e.UserId).HasColumnName("AspNetUserId");
			//    entity.Property(e => e.Id).HasColumnName("AspNetUserClaimId");
			//});

			//modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
			//{
			//    entity.ToTable("AspNetUserLogin", "Security");
			//    entity.Property(e => e.UserId).HasColumnName("AspNetUserId");
			//});

			//modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
			//{
			//    entity.ToTable("AspNetRoleClaim", "Security");
			//    entity.Property(e => e.Id).HasColumnName("AspNetRoleClaimId");
			//    entity.Property(e => e.RoleId).HasColumnName("AspNetRoleId");
			//});

			//modelBuilder.Entity<IdentityUserRole<int>>(entity =>
			//{
			//    entity.ToTable("AspNetUserRole", "Security");
			//    entity.Property(e => e.UserId).HasColumnName("AspNetUserId");
			//    entity.Property(e => e.RoleId).HasColumnName("AspNetRoleId");
			//});

			//modelBuilder.Entity<IdentityUserToken<int>>(entity =>
			//{
			//    entity.ToTable("AspNetUserToken", "Security");
			//    entity.Property(e => e.UserId).HasColumnName("AspNetUserId");
			//});


			#endregion
		}
	}
}
