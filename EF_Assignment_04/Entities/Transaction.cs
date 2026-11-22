using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalBankSystem.Enums;

namespace NationalBankSystem.Entities
{
    public class Transaction
    {
        public string TransactionNumber { get; set; } = null!;
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public string? Note { get; set; }

        public string AccountNumber { get; set; } = null!;
        public Account Account { get; set; } = null!;
    }
}
