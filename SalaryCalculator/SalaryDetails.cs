using System.Text;
using System.Text.Json;

namespace SalaryCalculator
{
    public class SalaryDetails
    {
        protected ITaxCalculator TaxCalculator;
        protected ITierdAllowanceScheme TiredAllowanceScheme;

        public SalaryDetails(ITaxCalculator taxCalculator, decimal basicSalary, List<Allowance>? allowances = null, List<Deduction>? deductions = null)
        {
            BasicSalary = basicSalary;
            Allowances = allowances ?? new List<Allowance>();
            Deductions = deductions ?? new List<Deduction>();
            TaxCalculator = taxCalculator;
        }
        public SalaryDetails(decimal basicSalary, List<Allowance>? allowances = null, List<Deduction>? deductions = null)
        {
            BasicSalary = basicSalary;
            Allowances = allowances ?? new List<Allowance>();
            Deductions = deductions ?? new List<Deduction>();
            TaxCalculator = new DefaultTaxCalulator();
        }

        public SalaryDetails(ITaxCalculator taxCalculator, decimal basicSalary, ITierdAllowanceScheme tiredAllowanceScheme, List<Allowance>? allowances = null, List<Deduction>? deductions = null) : this(taxCalculator, basicSalary, allowances, deductions)
        {
            TiredAllowanceScheme = tiredAllowanceScheme;
        }
        public SalaryDetails(decimal basicSalary, ITierdAllowanceScheme tiredAllowanceScheme, List<Allowance>? allowances = null, List<Deduction>? deductions = null) : this(basicSalary, allowances, deductions)
        {
            TiredAllowanceScheme = tiredAllowanceScheme;
        }


        public decimal BasicSalary { get; }
        public decimal TotalAllowances => (Allowances?.Sum(x => x.Value) ?? 0) + (TiredAllowanceScheme?.GetTotalAllowance(BasicSalary) ?? 0);
        public List<Allowance> Allowances { get; }
        public List<Deduction> Deductions { get; }
        public decimal TotalForTax => TaxCalculator.CalculateTaxableTotal(BasicSalary, Allowances.Concat(TiredAllowanceScheme?.GetAllowances(BasicSalary) ?? new List<Allowance>()), Deductions);
        public decimal TaxAmount => TaxCalculator.CalculateTaxAmount(BasicSalary, Allowances.Concat(TiredAllowanceScheme?.GetAllowances(BasicSalary) ?? new List<Allowance>()), Deductions);
        public decimal TotalDeductions => TaxAmount + EPFETFContributions.TotalEmployeeContribution + Deductions?.Sum(x => x.Value) ?? 0;
        public decimal NetSalary => BasicSalary + TotalAllowances - TotalDeductions;
        public decimal GrossSalary => BasicSalary + TotalAllowances;

        public EPFETFContributions EPFETFContributions => new(BasicSalary);

        public override string ToString()
        {
            StringBuilder @string = new();
            //@string.AppendLine($"Basic Salary: {BasicSalary}");
            @string.AppendLine($"Gross Salary: {GrossSalary}");
            @string.AppendLine($"Net Salary: {NetSalary}");

            if (Allowances?.Count > 0)
            {
                @string.AppendLine($"{Environment.NewLine}Fixed Allowances");
                Allowances.ForEach(x => @string.AppendLine($"\t{x.Name}: {x.Amount}"));

                @string.AppendLine($"Total Fixed Allowances: {TotalAllowances}");
            }

            @string.AppendLine($"{Environment.NewLine}Deductions");
            if (Deductions?.Count > 0)
            {
                Deductions.ForEach(x => @string.AppendLine($"\t{x.Name}: {x.Amount}"));
            }
            @string.AppendLine($"\tEPF: {EPFETFContributions.TotalEmployeeContribution}");
            if (TaxAmount > 0)
                @string.AppendLine($"\tTax: {TaxAmount}");
            @string.AppendLine($"Total Deductions: {TotalDeductions}");

            @string.AppendLine($"{Environment.NewLine}Employer Contributions");
            @string.AppendLine($"\tEPF: {EPFETFContributions.EPFEmployer}");
            @string.AppendLine($"\tETF: {EPFETFContributions.ETFEmployer}");
            @string.AppendLine($"Total Employer Contribution: {EPFETFContributions.TotalEmployerContribution}");


            @string.AppendLine($"{Environment.NewLine}Total EPF ETF Contribution: {EPFETFContributions.TotalContribution}");

            return @string.ToString();

        }

        public string ToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }



    public class EPFETFContributions
    {
        private const decimal EPFRateEmployee = 0.08m;
        private const decimal EPFRateEmployer = 0.12m;
        private const decimal ETFRateEmployer = 0.03m;
        internal EPFETFContributions(decimal basicSalary)
        {
            EPFEmployee = basicSalary * EPFRateEmployee;
            EPFEmployer = basicSalary * EPFRateEmployer;
            ETFEmployer = basicSalary * ETFRateEmployer;
        }
        public decimal EPFEmployee { get; private set; }
        public decimal EPFEmployer { get; private set; }
        public decimal ETFEmployer { get; private set; }
        public decimal TotalEmployerContribution => EPFEmployer + ETFEmployer;
        public decimal TotalEmployeeContribution => EPFEmployee;
        public decimal TotalEPFContribution => EPFEmployee + EPFEmployer;
        public decimal TotalContribution => TotalEmployerContribution + TotalEmployeeContribution;
    }

}

