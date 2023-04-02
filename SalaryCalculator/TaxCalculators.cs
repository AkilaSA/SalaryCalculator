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
}
