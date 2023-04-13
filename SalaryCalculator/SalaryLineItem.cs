namespace SalaryCalculator
{
    public abstract class SalaryLineItem
    {
        public SalaryLineItem(string name, decimal percentage, decimal baseAmount, bool isTaxable)
        {
            Name = name;
            IsPercentage = true;
            Amount = percentage;
            IsTaxable = isTaxable;
            _baseAmount = baseAmount;
        }

        public SalaryLineItem(string name, decimal amount, bool isTaxable)
        {
            Name = name;
            IsPercentage = false;
            Amount = amount;
            IsTaxable = isTaxable;
        }
        private readonly decimal _baseAmount;

        public string Name { get; }
        public decimal Amount { get; }
        public decimal BaseAmount => IsPercentage ? _baseAmount : Amount;
        public bool IsPercentage { get; }
        public bool IsTaxable { get; }

        public decimal Value => IsPercentage ? _baseAmount / 100 * Amount : Amount;
    }

    public class Allowance : SalaryLineItem
    {
        public Allowance(string name, decimal percentage, decimal baseAmount, bool isTaxable = true) : base(name, percentage, baseAmount, isTaxable) { }
        public Allowance(string name, decimal amount, bool isTaxable) : base(name, amount, isTaxable) { }
    }

    public class Deduction : SalaryLineItem
    {
        public Deduction(string name, decimal percentage, decimal baseAmount, bool isTaxable = false) : base(name, percentage, baseAmount, isTaxable) { }
        public Deduction(string name, decimal amount, bool isTaxable = false) : base(name, amount, isTaxable) { }
    }
}
