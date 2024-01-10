using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace johnWk4
{
    public class Transaction
    {

        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
    }

    public enum AccountType
    {
        Savings = 1,
        Current = 2
    }
}

