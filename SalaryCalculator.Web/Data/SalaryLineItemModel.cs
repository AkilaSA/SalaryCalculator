namespace SalaryCalculator.Web.Data
{
    public class SalaryLineItemModel
    {
        public SalaryLineItemModel(string name, decimal amount, bool isPercentageAmount, bool isTaxable)
        {
            Name = name;
            Amount = amount;
            IsPercentageAmount = isPercentageAmount;
            IsTaxable = isTaxable;
        }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsPercentageAmount { get; set; }
        public bool IsTaxable { get; set; }

        public Allowance GetAllowance(decimal basicSalary)
        {
            if (IsPercentageAmount)
            {
                return new(Name, Amount, basicSalary, IsTaxable);
            }
            return new(Name, Amount, IsTaxable);
        }
        public Deduction GetDeduction(decimal basicSalary)
        {
            if (IsPercentageAmount)
            {
                return new(Name, Amount, basicSalary, IsTaxable);
            }
            return new(Name, Amount, IsTaxable);
        }
    }
}
