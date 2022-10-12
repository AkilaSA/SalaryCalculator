using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator
{
    public enum TaxCalculators
    {
        APITaxCalculator_Old,
        NewAPITaxCalculator_Initial,
        NewAPITaxCalculator_Proposed_On_22_10_11
    }

    public static class TaxCalculatorFactory
    {
        public static ITaxCalculator GetTaxCalculator(TaxCalculators taxCalculator)
        {
            switch (taxCalculator)
            {
                case TaxCalculators.APITaxCalculator_Old:
                    {
                        return new APITaxCalculator(250000, 250000, 0.06m, 3);
                    }
                case TaxCalculators.NewAPITaxCalculator_Initial:
                    {
                        return new APITaxCalculator(150000, 100000, 0.04m, 8);
                    }
                case TaxCalculators.NewAPITaxCalculator_Proposed_On_22_10_11:
                default:
                    {
                        return new APITaxCalculator(100000, 500000m / 12m, 0.06m, 6);
                    }
            }
        }
    }

}
