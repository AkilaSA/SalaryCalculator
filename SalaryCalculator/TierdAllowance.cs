namespace SalaryCalculator
{
    public class TierdAllowance : Allowance
    {
        internal TierdAllowance(Guid schemeId, string schemeName, int sequence, decimal percentage, decimal baseAmount, bool isTaxable = true) : base(schemeName, percentage, baseAmount, isTaxable)
        {
            SchemeId = schemeId;
            SchemeName = schemeName;
            Sequence = sequence;
        }
        internal TierdAllowance(Guid schemeId, string schemeName, int sequence, decimal amount, bool isTaxable) : base(schemeName, amount, isTaxable)
        {
            SchemeId = schemeId;
            SchemeName = schemeName;
            Sequence = sequence;
        }

        public int Sequence { get; }
        public Guid SchemeId { get; }
        public string SchemeName { get; }

        public Allowance Allowance => this;

        public override string ToString()
        {
            return IsPercentage ? $"{string.Format("{0:N}", BaseAmount)}x{string.Format("{0:N}", Amount)}%" : $"{string.Format("{0:N}", Amount)}";
        }
    }

    public class AllowanceTier
    {
        public decimal TierFloorAmount { get; set; }
        public decimal Amount { get; set; }
        public bool IsPercentage { get; set; }
        public bool IsTaxable { get; set; }
    }
}
