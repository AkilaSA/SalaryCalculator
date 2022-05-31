using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator
{
    public interface ITaxCalculator
    {
        decimal CalculateTaxAmount(decimal basicSalary, List<SalaryLineItem>? fixedAllowances = null, List<SalaryLineItem>? otherFixedDeductions = null);
        Task<decimal> CalculateTaxAmountAsync(decimal basicSalary, List<SalaryLineItem>? fixedAllowances = null, List<SalaryLineItem>? otherFixedDeductions = null, CancellationToken cancellationToken = default);
    }
}
