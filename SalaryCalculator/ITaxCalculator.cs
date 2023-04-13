namespace SalaryCalculator
{
    public interface ITaxCalculator
    {
        decimal CalculateTaxAmount(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? deductions = null);
        Task<decimal> CalculateTaxAmountAsync(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? deductions = null, CancellationToken cancellationToken = default);
    }
}
