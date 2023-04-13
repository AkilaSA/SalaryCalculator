using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SalaryCalculator.TierdAllowanceScheme;

namespace SalaryCalculator
{
    public interface ITierdAllowanceScheme
    {
        public Guid SchemeId { get; }
        public string SchemeName { get; }
        List<AllowanceTier> Tiers { get; }
        public List<Allowance> GetAllowances(decimal basicSalary);
        public List<TierdAllowance> GetTierdAllowances(decimal basicSalary);
        public decimal GetTotalAllowance(decimal basicSalary);
        public TierdllowanceResponse GetTierdAllowance(decimal basicSalary);
    }
}
