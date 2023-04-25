namespace SalaryCalculator
{
    public interface ITaxCalculator
    {
        decimal CalculateTaxableTotal(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? deductions = null);
        Task<decimal> CalculateTaxableTotalAsync(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? deductions = null, CancellationToken cancellationToken = default);
        decimal CalculateTaxAmount(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? deductions = null);
        Task<decimal> CalculateTaxAmountAsync(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? deductions = null, CancellationToken cancellationToken = default);
    }

    public class DefaultTaxCalulator : ITaxCalculator
    {
        public virtual decimal CalculateTaxableTotal(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? deductions = null)
        {
            return basicSalary + (allowances?.Where(a => a.IsTaxable).Sum(x => x.Value) ?? 0) - (deductions?.Where(a => a.IsTaxable).Sum(x => x.Value) ?? 0);
        }

        public virtual Task<decimal> CalculateTaxableTotalAsync(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? deductions = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CalculateTaxableTotal(basicSalary, allowances, deductions));
        }

        public virtual decimal CalculateTaxAmount(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? dductions = null)
        {
            return 0m;
        }

        public virtual Task<decimal> CalculateTaxAmountAsync(decimal basicSalary, IEnumerable<Allowance>? allowances = null, IEnumerable<Deduction>? deductions = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0m);
        }
    }
}
