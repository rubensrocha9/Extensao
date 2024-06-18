using GestorPay.Models;
using Microsoft.EntityFrameworkCore;

namespace GestorPay.Context
{
    public class GestorPayContext : DbContext
    {
        public DbSet<AuthCompany> CompanyAuthentications { get; set; }
        public DbSet<AuthEmployee> EmployeeAuthentications { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbSet<CompanyAddress> CompanyAddresses { get; set; }
        public DbSet<EmployeeAddress> Addresses { get; set; }
        public DbSet<SpendingManager> SpendingManagers { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<EmployeeFeedback> EmployeeFeedback { get; set; }

        public GestorPayContext(DbContextOptions<GestorPayContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(e =>
            {
                e.Property(p => p.Amount).HasPrecision(18,2);

                e.HasOne(p => p.Company)
                    .WithMany(c => c.Employees)
                    .HasForeignKey(p => p.CompanyId)
                    .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(p => p.EmployeePosition)
                    .WithMany()
                    .HasForeignKey(p => p.PositionId)
                    .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(p => p.Address)
                    .WithOne()
                    .HasForeignKey<EmployeeAddress>(a => a.EmployeeId)
                    .OnDelete(DeleteBehavior.NoAction);

                e.HasMany(p => p.EmployeeFeedback)
                    .WithOne(p => p.Employee)
                    .HasForeignKey(p => p.EmployeeId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<AuthEmployee>(e =>
            {

                e.HasOne(p => p.Employee)
                    .WithOne(p => p.AuthEmployee)
                    .HasForeignKey<AuthEmployee>(p => p.AuthEmployeeId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<EmployeePosition>(e =>
            {
                e.HasOne(p => p.Company)
                    .WithMany(c => c.EmployeePositions)
                    .HasForeignKey(p => p.CompanyId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Company>(e =>
            {
                e.HasOne(p => p.Auth)
                    .WithOne(p => p.Company)
                    .HasForeignKey<Company>(p => p.AuthId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<CompanyAddress>(e =>
            {
                e.HasOne(p => p.Company)
                    .WithOne(p => p.Address)
                    .HasForeignKey<CompanyAddress>(p => p.CompanyId)
                    .OnDelete(DeleteBehavior.NoAction); ;
            });

            modelBuilder.Entity<SpendingManager>(e =>
            {
                e.Property(p => p.Amount).HasPrecision(18, 2);

                e.HasOne(p => p.Company)
                    .WithMany(c => c.SpendingManagers)
                    .HasForeignKey(p => p.CompanyId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
