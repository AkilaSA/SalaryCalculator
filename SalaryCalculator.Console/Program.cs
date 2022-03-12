// See https://aka.ms/new-console-template for more information

while (true)
{
    Console.WriteLine("=================");
    Console.Write("Basic Salary: ");
    if (decimal.TryParse(Console.ReadLine()?.Trim(), out decimal basic) && basic > 0)
    {
        var deductions = new List<(string Name, decimal Amount)> { new("Staff Wellfare", 75) };
        var details = new SalaryCalculator.SalaryDetails(basic, otherFixedDeductions: deductions);
        Console.WriteLine(details.ToString());
    }
    else
    {
        Console.WriteLine("Invalid Input!" + Environment.NewLine);
    }

}