using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryCalculator
{
    public enum TierdAllowanceSchemes
    {
        IFS_April2023
    }

    public static class TierdAllowanceSchemeFactory
    {
        public static ITierdAllowanceScheme GetTiredAllowanceScheme(TierdAllowanceSchemes scheme)
        {
            switch (scheme)
            {
                case TierdAllowanceSchemes.IFS_April2023:
                default:
                    {
                        var s = new TierdAllowanceScheme("Economic Relief Allowance");
                        s.AddTier(0, 41);
                        s.AddTier(320000, 24);
                        s.AddTier(610000, 8);
                        return s;
                    }
            }
        }
    }
}
