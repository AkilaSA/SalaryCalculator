using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator
{
    public class SalaryLineItem
    {
        public SalaryLineItem(string name, decimal amount, bool isPercentage = false)
        {
            Name = name;
            IsPercentage = isPercentage;
            Amount = amount;
        }

        public string Name { get; }
        public decimal Amount { get; }
        public bool IsPercentage { get; }

        public decimal GetValue(decimal baseValue) => IsPercentage ? baseValue * Amount / 100 : Amount;
    }
}
