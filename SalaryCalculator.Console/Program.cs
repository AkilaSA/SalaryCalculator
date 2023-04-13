// See https://aka.ms/new-console-template for more information



using SalaryCalculator;

//var k = new SampathExchangeRateProvider(new HttpClient());
//var kk=await k.GetExchangeRate(CurrencyCode.USD);

var ka = TierdAllowanceSchemeFactory.GetTiredAllowanceScheme(TierdAllowanceSchemes.IFS_April2023);



var l = ka.GetTierdAllowances(125000);
var kk = ka.GetTotalAllowance(125000);
//return;


while (true)
{
    try
    {
        Console.WriteLine("=================");
        Console.Write("Basic Salary: ");
        //if (decimal.TryParse(Console.ReadLine()?.Trim(), out decimal basic) && basic > 0)
        {
            decimal basic = decimal.Parse(Console.ReadLine()?.Trim());
            var k = TierdAllowanceSchemeFactory.GetTiredAllowanceScheme(TierdAllowanceSchemes.IFS_April2023);
            //var deductions = new List<(string Name, decimal Amount)> { new("Staff Wellfare", 75) };

            List<Deduction> _otherFixedDeductions = new List<Deduction> { new Deduction("Staff Wellfare", 75) };
            //List<SalaryLineItem> _fixedAllowances = new List<SalaryLineItem> { new SalaryLineItem("TCoLA", 41, true) };
            var taxCalculator = TaxCalculatorFactory.GetTaxCalculator(TaxCalculators.NewAPITaxCalculator_Proposed_On_22_10_11);

            var details = new SalaryCalculator.SalaryDetails(taxCalculator, basic, tiredAllowanceScheme: k, deductions: _otherFixedDeductions);
            //var details = new SalaryCalculator.SalaryDetails(basic, otherFixedDeductions: deductions);


            var kkk = details.TotalAllowances;
            var kaakk = details.GrossSalary;
            var aaa = details.NetSalary;
            //var sss = k.GetSchemeString(620000);
            //var sssss = k.GetSchemeString(350000);
            //var ssssss = k.GetSchemeString(240000);
            Console.WriteLine(details.ToString());

        }
        //else
        //{
        //    Console.WriteLine("Invalid Input!" + Environment.NewLine);
        //}
    }
    catch (System.OverflowException ex)
    {
        Console.WriteLine(ex.ToString());
    }

}