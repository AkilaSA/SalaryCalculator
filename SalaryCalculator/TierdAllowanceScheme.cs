using System.Globalization;
using System.Text;

namespace SalaryCalculator
{
    public class TierdAllowanceScheme : ITierdAllowanceScheme
    {
        public Guid SchemeId { get; }
        public string SchemeName { get; }

        public List<AllowanceTier> Tiers { get; private set; }

        public decimal GetTotalAllowance(decimal basicSalary) => GetTierdAllowances(basicSalary).Sum(x => x.Value);

        public TierdAllowanceScheme(string? schemeName = null)
        {
            Tiers = new List<AllowanceTier>();
            SchemeId = Guid.NewGuid();
            if (string.IsNullOrWhiteSpace(schemeName))
            {
                SchemeName = $"Tierd Allowance Scheme - {SchemeId}";
            }
            else
            {
                SchemeName = schemeName;
            }
        }

        public void AddTier(decimal tierFloorAmount, decimal amount, bool isPercentage = true, bool isTaxable = true)
        {
            if (tierFloorAmount < 0)
            {
                throw new ArgumentException("A positive amount is required", nameof(tierFloorAmount));
            }
            if (amount < 0)
            {
                throw new ArgumentException("A positive amount is required", nameof(amount));
            }
            var t = new AllowanceTier
            {
                Amount = amount,
                TierFloorAmount = tierFloorAmount,
                IsPercentage = isPercentage,
                IsTaxable = isTaxable,
            };
            Tiers.Add(t);
            Tiers = Tiers.OrderByDescending(x => x.TierFloorAmount).ToList();
        }

        public List<TierdAllowance> GetTierdAllowances(decimal basicSalary)
        {
            List<TierdAllowance> allowances = new();

            int i = Tiers.Count;
            var currentAmount = basicSalary;

            foreach (var item in Tiers)
            {
                if (currentAmount <= item.TierFloorAmount)
                {
                    --i;
                    continue;
                }
                var baseAmount = currentAmount - item.TierFloorAmount;
                var a = item.IsPercentage ?
                    new TierdAllowance(SchemeId, SchemeName, i, item.Amount, baseAmount, item.IsTaxable) :
                    new TierdAllowance(SchemeId, SchemeName, i, item.Amount, item.IsTaxable);

                allowances.Add(a);
                currentAmount = item.TierFloorAmount;
                --i;
            }
            return allowances;
        }

        public List<Allowance> GetAllowances(decimal basicSalary)
        {
            return GetTierdAllowances(basicSalary).Select(x => x.Allowance).ToList();
        }

        public TierdllowanceResponse GetTierdAllowance(decimal basicSalary)
        {
            TierdllowanceResponse response = new()
            {
                TierdAllowances = GetTierdAllowances(basicSalary),
                SchemeName = SchemeName,
                SchemeId = SchemeId
            };
            response.Total = response.TierdAllowances.Sum(x => x.Value);
            response.Description = GetSchemeString(response.TierdAllowances);

            return response;
        }

        private string GetSchemeString(List<TierdAllowance> data)
        {
            if (data.Count < 1)
            {
                return SchemeName;
            }
            StringBuilder sb = new("(");
            for (int i = data.Count - 1; i >= 0; i--)
            {

                var s = data[i].ToString();
                if (i == data.Count - 1)
                {
                    sb.Append(s);
                }
                else
                {
                    sb.Append($" + {s}");
                }
            }
            sb.Append(')');
            return sb.ToString();
        }

    }
    public class TierdllowanceResponse
    {
        public List<TierdAllowance> TierdAllowances { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public Guid SchemeId { get; set; }
        public string SchemeName { get; set; }
    }
}