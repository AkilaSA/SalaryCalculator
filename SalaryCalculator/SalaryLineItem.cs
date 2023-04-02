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

        public decimal GetValue(decimal baseValue) => IsPercentage ? baseValue / 100 * Amount : Amount;
    }
}
