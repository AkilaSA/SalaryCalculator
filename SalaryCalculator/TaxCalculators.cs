using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator
{
    public class APITaxCalculator : ITaxCalculator
    {
        public decimal CalculateTaxAmount(decimal basicSalary, List<SalaryLineItem>? fixedAllowances = null, List<SalaryLineItem>? otherFixedDeductions = null)
        {
            var monthlyProfit = basicSalary + fixedAllowances?.Sum(x => x.GetValue(basicSalary)) ?? 0;
            var tax = 0m;
            if (monthlyProfit <= 250000)
                return tax;

            if (monthlyProfit <= 500000)
            {
                tax = monthlyProfit * 0.06m - 15000;
            }
            else if (monthlyProfit <= 750000)
            {
                tax = monthlyProfit * 0.12m - 45000;
            }
            else
            {
                tax = monthlyProfit * 0.18m - 90000;
            }
            return tax;
        }

        public Task<decimal> CalculateTaxAmountAsync(decimal basicSalary, List<SalaryLineItem>? fixedAllowances = null, List<SalaryLineItem>? otherFixedDeductions = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CalculateTaxAmount(basicSalary, fixedAllowances, otherFixedDeductions));
        }
    }

    public class NewAPITaxCalculator : ITaxCalculator
    {
        public decimal CalculateTaxAmount(decimal basicSalary, List<SalaryLineItem>? fixedAllowances = null, List<SalaryLineItem>? otherFixedDeductions = null)
        {
            var monthlyProfit = basicSalary + fixedAllowances?.Sum(x => x.GetValue(basicSalary)) ?? 0;
            var tax = 0m;
            if (monthlyProfit <= 150000)
                return tax;

            if (monthlyProfit <= 200000)
            {
                tax = monthlyProfit * 0.04m - 6000;
            }
            else if (monthlyProfit <= 250000)
            {
                tax = monthlyProfit * 0.08m - 12000;
            }
            else if (monthlyProfit <= 300000)
            {
                tax = monthlyProfit * 0.12m - 22000;
            }
            else if (monthlyProfit <= 350000)
            {
                tax = monthlyProfit * 0.16m - 34000;
            }
            else if (monthlyProfit <= 400000)
            {
                tax = monthlyProfit * 0.2m - 48000;
            }
            else if (monthlyProfit <= 450000)
            {
                tax = monthlyProfit * 0.24m - 64000;
            }
            else if (monthlyProfit <= 500000)
            {
                tax = monthlyProfit * 0.28m - 82000;
            }
            else
            {
                tax = monthlyProfit * 0.32m - 102000;
            }
            return tax;
        }

        public Task<decimal> CalculateTaxAmountAsync(decimal basicSalary, List<SalaryLineItem>? fixedAllowances = null, List<SalaryLineItem>? otherFixedDeductions = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CalculateTaxAmount(basicSalary, fixedAllowances, otherFixedDeductions));
        }
    }
}
