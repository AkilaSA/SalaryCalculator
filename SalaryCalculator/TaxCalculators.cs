using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator
{
    internal class APITaxCalculator : ITaxCalculator
    {
        public decimal TaxReliefAmount { get; }
        public decimal TaxTierAmount { get; }
        public decimal TaxRateFactor { get; }
        public decimal MaxTier { get; }

        internal APITaxCalculator(decimal taxReliefAmount, decimal taxTierAmount, decimal taxRateFactor, decimal maxTier)
        {
            TaxReliefAmount = taxReliefAmount;
            TaxTierAmount = taxTierAmount;
            TaxRateFactor = taxRateFactor;
            MaxTier = maxTier;
        }

        public decimal CalculateTaxAmount(decimal basicSalary, List<SalaryLineItem>? taxableAllowances = null, List<SalaryLineItem>? taxableDeductions = null)
        {
            var monthlyProfit = basicSalary + taxableAllowances?.Sum(x => x.GetValue(basicSalary)) ?? 0;
            var tax = 0m;

            var currentTaxableAmount = monthlyProfit - TaxReliefAmount;
            var tier = 1;
            while (currentTaxableAmount > 0)
            {
                if (currentTaxableAmount >= TaxTierAmount && tier < MaxTier)
                {
                    tax += TaxTierAmount * TaxRateFactor * tier;
                }
                else
                {
                    tax += currentTaxableAmount * TaxRateFactor * tier;
                    break;
                }
                tier++;
                currentTaxableAmount -= TaxTierAmount;
            }
            return tax;
        }

        public Task<decimal> CalculateTaxAmountAsync(decimal basicSalary, List<SalaryLineItem>? taxableAllowances = null, List<SalaryLineItem>? taxableDeductions = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CalculateTaxAmount(basicSalary, taxableAllowances, taxableDeductions));
        }
    }

    //public class APITaxCalculator : ITaxCalculator
    //{
    //    public decimal CalculateTaxAmount(decimal basicSalary, List<SalaryLineItem>? taxableAllowances = null, List<SalaryLineItem>? taxableDeductions = null)
    //    {
    //        var monthlyProfit = basicSalary + taxableAllowances?.Sum(x => x.GetValue(basicSalary)) ?? 0;
    //        var tax = 0m;
    //        if (monthlyProfit <= 250000)
    //            return tax;

    //        if (monthlyProfit <= 500000)
    //        {
    //            tax = monthlyProfit * 0.06m - 15000;
    //        }
    //        else if (monthlyProfit <= 750000)
    //        {
    //            tax = monthlyProfit * 0.12m - 45000;
    //        }
    //        else
    //        {
    //            tax = monthlyProfit * 0.18m - 90000;
    //        }
    //        return tax;
    //    }

    //    public Task<decimal> CalculateTaxAmountAsync(decimal basicSalary, List<SalaryLineItem>? taxableAllowances = null, List<SalaryLineItem>? taxableDeductions = null, CancellationToken cancellationToken = default)
    //    {
    //        return Task.FromResult(CalculateTaxAmount(basicSalary, taxableAllowances, taxableDeductions));
    //    }
    //}

    //public class NewAPITaxCalculator_Initial : ITaxCalculator
    //{
    //    public decimal CalculateTaxAmount(decimal basicSalary, List<SalaryLineItem>? taxableAllowances = null, List<SalaryLineItem>? taxableDeductions = null)
    //    {
    //        const decimal taxReliefAmount = 150000;
    //        const decimal taxTierAmount = 100000;
    //        const decimal taxRateFactor = 0.04m;
    //        const decimal maxTier = 8;

    //        var monthlyProfit = basicSalary + taxableAllowances?.Sum(x => x.GetValue(basicSalary)) ?? 0;
    //        var tax = 0m;

    //        var currentTaxableAmount = monthlyProfit - taxReliefAmount;
    //        var tier = 1;
    //        while (currentTaxableAmount > 0)
    //        {
    //            if (currentTaxableAmount >= taxTierAmount && tier < maxTier)
    //            {
    //                tax += taxTierAmount * taxRateFactor * tier;
    //            }
    //            else
    //            {
    //                tax += currentTaxableAmount * taxRateFactor * tier;
    //                break;
    //            }
    //            tier++;
    //            currentTaxableAmount -= taxTierAmount;
    //        }
    //        return tax;

    //        //if (monthlyProfit <= 150000)
    //        //    return tax;

    //        //if (monthlyProfit <= 250000)
    //        //{
    //        //    tax = monthlyProfit * 0.04m - 6000;
    //        //}
    //        //else if (monthlyProfit <= 350000)
    //        //{
    //        //    tax = monthlyProfit * 0.08m - 16000;
    //        //}
    //        //else if (monthlyProfit <= 450000)
    //        //{
    //        //    tax = monthlyProfit * 0.12m - 30000;
    //        //}
    //        //else if (monthlyProfit <= 550000)
    //        //{
    //        //    tax = monthlyProfit * 0.16m - 48000;
    //        //}
    //        //else if (monthlyProfit <= 650000)
    //        //{
    //        //    tax = monthlyProfit * 0.2m - 70000;
    //        //}
    //        //else if (monthlyProfit <= 750000)
    //        //{
    //        //    tax = monthlyProfit * 0.24m - 96000;
    //        //}
    //        //else if (monthlyProfit <= 850000)
    //        //{
    //        //    tax = monthlyProfit * 0.28m - 126000;
    //        //}
    //        //else
    //        //{
    //        //    tax = monthlyProfit * 0.32m - 160000;
    //        //}
    //        //return tax;
    //    }

    //    public Task<decimal> CalculateTaxAmountAsync(decimal basicSalary, List<SalaryLineItem>? taxableAllowances = null, List<SalaryLineItem>? taxableDeductions = null, CancellationToken cancellationToken = default)
    //    {
    //        return Task.FromResult(CalculateTaxAmount(basicSalary, taxableAllowances, taxableDeductions));
    //    }
    //}
}
