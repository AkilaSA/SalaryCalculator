// See https://aka.ms/new-console-template for more information



using SalaryCalculator;

//var k = new SampathExchangeRateProvider(new HttpClient());
//var kk=await k.GetExchangeRate(CurrencyCode.USD);

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
            var deductions = new List<(string Name, decimal Amount)> { new("Staff Wellfare", 75) };

            List<SalaryLineItem> _otherFixedDeductions = new List<SalaryLineItem> { new SalaryLineItem("Staff Wellfare", 75, false) };
            List<SalaryLineItem> _fixedAllowances = new List<SalaryLineItem> { new SalaryLineItem("TCoLA", 41, true) };
            var taxCalculator = new SalaryCalculator.NewAPITaxCalculator();

            var details = new SalaryCalculator.SalaryDetails(taxCalculator, basic, fixedAllowances: _fixedAllowances, otherFixedDeductions: _otherFixedDeductions);
            //var details = new SalaryCalculator.SalaryDetails(basic, otherFixedDeductions: deductions);
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