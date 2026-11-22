using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalBankSystem.Enums;

namespace NationalBankSystem.Entities
{
    public class CustomerAccount
    {
        public string AccountNumber { get; set; } = null!;
        public Account Account { get; set; } = null!;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public OwnershipType OwnershipType { get; set; }
        public DateOnly OwnershipStartDate { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
