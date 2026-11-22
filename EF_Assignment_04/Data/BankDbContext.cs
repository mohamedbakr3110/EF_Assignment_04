using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NationalBankSystem.Entities;

namespace NationalBankSystem.Data
{
    public class BankDbContext : DbContext
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=NationalBankDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankDbContext).Assembly);

            
            modelBuilder.Entity<Manager>().HasData(
                new Manager
                {
                    Id = 1,
                    FullName = "Alex Morgan",
                    Email = "alex.morgan@nationalbank.example",
                    PhoneNumber = "+1-202-555-0101",
                    HireDate = new DateOnly(2018, 5, 1)
                },
                new Manager
                {
                    Id = 2,
                    FullName = "Jamie Lee",
                    Email = "jamie.lee@nationalbank.example",
                    PhoneNumber = "+1-202-555-0102",
                    HireDate = new DateOnly(2019, 9, 15)
                }
            );

            modelBuilder.Entity<Branch>().HasData(
                new Branch
                {
                    Code = "CTR-01",
                    Name = "Central Branch",
                    Address = "100 Market Street",
                    PhoneNumber = "+1-202-555-0201",
                    ManagerId = 1
                },
                new Branch
                {
                    Code = "HBR-01",
                    Name = "Harbor Branch",
                    Address = "250 Harbor Avenue",
                    PhoneNumber = "+1-202-555-0202",
                    ManagerId = 2
                }
            );
        }
    }
}
