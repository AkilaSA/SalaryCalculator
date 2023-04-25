namespace SalaryCalculator
{
    internal class APITaxCalculator : DefaultTaxCalulator
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

        public override decimal CalculateTaxAmount(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? deductions = null)
        {
            var monthlyProfit = CalculateTaxableTotal(basicSalary, allowances, deductions);
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

        public override Task<decimal> CalculateTaxAmountAsync(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? deductions = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CalculateTaxAmount(basicSalary, allowances, deductions));
        }
    }
}
