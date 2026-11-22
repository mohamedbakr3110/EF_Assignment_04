using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalBankSystem.Enums;

namespace NationalBankSystem.Entities
{
    public class Account
    {
        public string AccountNumber { get; set; } = null!;
        public decimal CurrentBalance { get; set; }
        public AccountType AccountType { get; set; }
        public DateOnly OpeningDate { get; set; }

        public string BranchCode { get; set; } = null!;
        public Branch Branch { get; set; } = null!;

        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new HashSet<CustomerAccount>();
        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    }
}