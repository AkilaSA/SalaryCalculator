using System.Text;
using System.Text.Json;

namespace SalaryCalculator
{
    public class SalaryDetails
    {
        public SalaryDetails(decimal basicSalary, List<SalaryLineItem>? fixedAllowances = null, List<SalaryLineItem>? otherFixedDeductions = null)
        {
            BasicSalary = basicSalary;
            FixedAllowances = fixedAllowances ?? new List<SalaryLineItem>();
            OtherFixedDeductions = otherFixedDeductions ?? new List<SalaryLineItem>();
        }

        public decimal BasicSalary { get; }
        public decimal TotalFixedAllowances => FixedAllowances?.Sum(x => x.GetValue(BasicSalary)) ?? 0;
        public List<SalaryLineItem> FixedAllowances { get; }
        public List<SalaryLineItem> OtherFixedDeductions { get; }
        public decimal PayeeTaxAmount
        {
            get
            {
                return GetPayeeTaxAmount(BasicSalary + TotalFixedAllowances);
            }
        }
        public decimal TotalDeductions => PayeeTaxAmount + EPFETFContributions.TotalEmployeeContribution + OtherFixedDeductions?.Sum(x => x.GetValue(BasicSalary)) ?? 0;
        public decimal NetSalary => BasicSalary + TotalFixedAllowances - TotalDeductions;
        public decimal GrossSalary => BasicSalary + TotalFixedAllowances;

        public EPFETFContributions EPFETFContributions => new(BasicSalary);


        private static decimal GetPayeeTaxAmount(decimal monthlyProfit)
        {
            var tax = 0m;
            if (monthlyProfit <= 250000)
                return tax;

            if (monthlyProfit <= 500000)
            {
                tax = monthlyProfit * 0.06m - 15000;
            }
            else if (monthlyProfit <= 750000)
            {
                tax = monthlyProfit * 0.12m - 45000;
            }
            else
            {
                tax = monthlyProfit * 0.18m - 90000;
            }
            return tax;
        }
        public override string ToString()
        {
            StringBuilder @string = new();
            //@string.AppendLine($"Basic Salary: {BasicSalary}");
            @string.AppendLine($"Gross Salary: {GrossSalary}");
            @string.AppendLine($"Net Salary: {NetSalary}");

            if (FixedAllowances?.Count > 0)
            {
                @string.AppendLine($"{Environment.NewLine}Fixed Allowances");
                FixedAllowances.ForEach(x => @string.AppendLine($"\t{x.Name}: {x.Amount}"));

                @string.AppendLine($"Total Fixed Allowances: {TotalFixedAllowances}");
            }

            @string.AppendLine($"{Environment.NewLine}Deductions");
            if (OtherFixedDeductions?.Count > 0)
            {
                OtherFixedDeductions.ForEach(x => @string.AppendLine($"\t{x.Name}: {x.Amount}"));
            }
            @string.AppendLine($"\tEPF: {EPFETFContributions.TotalEmployeeContribution}");
            if (PayeeTaxAmount > 0)
                @string.AppendLine($"\tPayee Tax: {PayeeTaxAmount}");
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

