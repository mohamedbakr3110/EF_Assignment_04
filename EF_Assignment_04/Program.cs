using System.Globalization;
using Microsoft.EntityFrameworkCore;
using NationalBankSystem.Data;
using NationalBankSystem.Entities;
using NationalBankSystem.Enums;

namespace NationalBankSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new BankDbContext();

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.Title = "National Bank Management";
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║       NATIONAL BANK MANAGEMENT         ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("  [1]  Add New Customer");
                Console.WriteLine("  [2]  Open New Account");
                Console.WriteLine("  [3]  Update Account Status");
                Console.WriteLine("  [4]  Remove Account Ownership");
                Console.WriteLine("  [5]  View Customers & Accounts");
                Console.WriteLine("  [0]  Exit");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("──────────────────────────────────────────");
                Console.ResetColor();

                int choice = ReadIntInRange("Enter choice: ", 0, 5);
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        AddCustomer(context);
                        Pause();
                        break;
                    case 2:
                        OpenAccount(context);
                        Pause();
                        break;
                    case 3:
                        UpdateAccountStatus(context);
                        Pause();
                        break;
                    case 4:
                        RemoveAccountFromCustomer(context);
                        Pause();
                        break;
                    case 5:
                        ListAllCustomers(context);
                        Pause();
                        break;
                    case 0:
                        running = false;
                        Console.WriteLine("Closing connection and exiting... Goodbye!");
                        break;
                }
            }
        }

        static void AddCustomer(BankDbContext context)
        {
            Console.WriteLine("--- Add New Customer ---");
            string fullName = ReadNonEmptyString("Full Name               : ");
            string nationalId = ReadNonEmptyString("National ID             : ");

            if (context.Customers.Any(c => c.NationalId == nationalId))
            {
                Console.WriteLine("Error: A customer with this National ID already exists.");
                return;
            }

            DateOnly dob = ReadDate("Date of Birth (yyyy-MM-dd): ");
            string email = ReadNonEmptyString("Email                   : ");
            string phone = ReadNonEmptyString("Phone                   : ");
            string address = ReadNonEmptyString("Address                 : ");

            Console.WriteLine("Customer Type:");
            Console.WriteLine("  1) Individual");
            Console.WriteLine("  2) Business");
            int typeChoice = ReadIntInRange("  Choice: ", 1, 2);
            CustomerType customerType = (CustomerType)typeChoice;

            var customer = new Customer
            {
                FullName = fullName,
                NationalId = nationalId,
                DateOfBirth = dob,
                Email = email,
                PhoneNumber = phone,
                Address = address,
                CustomerType = customerType
            };

            context.Customers.Add(customer);
            context.SaveChanges();

            Console.WriteLine($"\nCustomer created successfully. CustomerId = {customer.Id}");
        }

        static void OpenAccount(BankDbContext context)
        {
            Console.WriteLine("--- Open New Account ---");
            string accountNumber = ReadNonEmptyString("Account Number: ");

            Console.WriteLine("Account Type:");
            Console.WriteLine("  1) Savings");
            Console.WriteLine("  2) Current");
            Console.WriteLine("  3) Business");
            int accTypeChoice = ReadIntInRange("  Choice: ", 1, 3);
            AccountType accountType = (AccountType)accTypeChoice;

            string branchCode = ReadNonEmptyString("Branch Code   : ");
            int customerId = ReadPositiveInt("Customer Id   : ");

            Console.WriteLine("Ownership Role:");
            Console.WriteLine("  1) Primary");
            Console.WriteLine("  2) CoHolder");
            int roleChoice = ReadIntInRange("  Choice: ", 1, 2);
            OwnershipType ownershipRole = (OwnershipType)roleChoice;

            Console.WriteLine($"\nValidating branch '{branchCode}' and customer #{customerId}...");

            var branch = context.Branches.Find(branchCode);
            if (branch == null)
            {
                Console.WriteLine($"Error: Branch '{branchCode}' does not exist.");
                return;
            }

            var customer = context.Customers.Find(customerId);
            if (customer == null)
            {
                Console.WriteLine($"Error: Customer #{customerId} does not exist.");
                return;
            }

            var existingAccount = context.Accounts.Find(accountNumber);
            if (existingAccount == null)
            {
                decimal initialDeposit = ReadDecimal("Initial Balance: ");
                existingAccount = new Account
                {
                    AccountNumber = accountNumber,
                    AccountType = accountType,
                    CurrentBalance = initialDeposit,
                    OpeningDate = DateOnly.FromDateTime(DateTime.Now),
                    BranchCode = branchCode
                };
                context.Accounts.Add(existingAccount);
            }
            else
            {
                bool alreadyLinked = context.CustomerAccounts
                    .Any(ca => ca.AccountNumber == accountNumber && ca.CustomerId == customerId);
                if (alreadyLinked)
                {
                    Console.WriteLine($"Error: Customer #{customerId} is already linked to account '{accountNumber}'.");
                    return;
                }
            }

            var customerAccount = new CustomerAccount
            {
                AccountNumber = accountNumber,
                CustomerId = customerId,
                OwnershipType = ownershipRole,
                OwnershipStartDate = DateOnly.FromDateTime(DateTime.Now),
                AccountStatus = AccountStatus.Active
            };

            context.CustomerAccounts.Add(customerAccount);
            context.SaveChanges();

            Console.WriteLine($"Account '{accountNumber}' created and linked to customer {customerId} as {ownershipRole} owner.");
        }

        static void UpdateAccountStatus(BankDbContext context)
        {
            Console.WriteLine("--- Update Account Status ---");
            string accountNumber = ReadNonEmptyString("Account Number: ");
            int customerId = ReadPositiveInt("Customer Id   : ");

            var link = context.CustomerAccounts
                .FirstOrDefault(ca => ca.AccountNumber == accountNumber && ca.CustomerId == customerId);

            if (link == null)
            {
                Console.WriteLine($"Error: No link found between Account '{accountNumber}' and Customer #{customerId}.");
                return;
            }

            Console.WriteLine($"Current Status: {link.AccountStatus}");
            Console.WriteLine("New Status:");
            Console.WriteLine("  1) Active");
            Console.WriteLine("  2) Closed");
            int statusChoice = ReadIntInRange("  Choice: ", 1, 2);

            link.AccountStatus = (AccountStatus)statusChoice;
            context.SaveChanges();

            Console.WriteLine($"\nStatus updated to {link.AccountStatus}.");
        }

        static void RemoveAccountFromCustomer(BankDbContext context)
        {
            Console.WriteLine("--- Remove Account From Customer ---");
            string accountNumber = ReadNonEmptyString("Account Number: ");
            int customerId = ReadPositiveInt("Customer Id   : ");

            var link = context.CustomerAccounts
                .FirstOrDefault(ca => ca.AccountNumber == accountNumber && ca.CustomerId == customerId);

            if (link == null)
            {
                Console.WriteLine($"Error: No link found between Account '{accountNumber}' and Customer #{customerId}.");
                return;
            }

            context.CustomerAccounts.Remove(link);
            context.SaveChanges();
            Console.WriteLine("\nOwnership link deleted.");

            bool hasOtherOwners = context.CustomerAccounts.Any(ca => ca.AccountNumber == accountNumber);
            if (!hasOtherOwners)
            {
                var account = context.Accounts.Find(accountNumber);
                if (account != null)
                {
                    context.Accounts.Remove(account);
                    context.SaveChanges();
                    Console.WriteLine($"That was the last owner — account '{accountNumber}' was also removed.");
                }
            }
        }

        static void ListAllCustomers(BankDbContext context)
        {
            Console.WriteLine("--- All Customers ---");

            var customers = context.Customers
                .Include(c => c.CustomerAccounts)
                    .ThenInclude(ca => ca.Account)
                        .ThenInclude(a => a.Branch)
                .AsNoTracking()
                .ToList();

            if (!customers.Any())
            {
                Console.WriteLine("No customers found in the system.");
                return;
            }

            foreach (var c in customers)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Customer #{c.Id} — {c.FullName} ({c.CustomerType})");
                Console.ResetColor();

                if (!c.CustomerAccounts.Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("   No accounts assigned");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("   ┌────────────┬──────────┬─────────────┬───────────┬────────┬──────────────────┐");
                    Console.WriteLine("   │ Account    │ Type     │ Balance     │ Ownership │ Status │ Branch           │");
                    Console.WriteLine("   ├────────────┼──────────┼─────────────┼───────────┼────────┼──────────────────┤");
                    foreach (var ca in c.CustomerAccounts)
                    {
                        var acc = ca.Account;
                        Console.WriteLine($"   │ {acc.AccountNumber,-10} │ {acc.AccountType,-8} │ {acc.CurrentBalance,11:N2} │ {ca.OwnershipType,-9} │ {ca.AccountStatus,-6} │ {acc.Branch.Name,-16} │");
                    }
                    Console.WriteLine("   └────────────┴──────────┴─────────────┴───────────┴────────┴──────────────────┘");
                }
            }
        }

        static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.WriteLine("Invalid input. Value cannot be empty. Please try again.");
            }
        }

        static int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                    return value;

                Console.WriteLine($"Invalid input. Please enter a number between {min} and {max}.");
            }
        }

        static int ReadPositiveInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
                    return value;

                Console.WriteLine("Invalid input. Please enter a valid positive number.");
            }
        }

        static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal value) && value >= 0)
                    return value;

                Console.WriteLine("Invalid input. Please enter a valid non-negative amount.");
            }
        }

        static DateOnly ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (DateOnly.TryParseExact(Console.ReadLine()?.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
                    return date;

                Console.WriteLine("Invalid date format. Please use yyyy-MM-dd (e.g., 2000-01-25).");
            }
        }

        static void Pause()
        {
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey(true);
        }
    }
}