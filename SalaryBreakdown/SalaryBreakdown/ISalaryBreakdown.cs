namespace SalaryBreakdownApp.SalaryPackage
{
    public interface ISalaryBreakdown
    {
        public decimal GrossIncome { get; set; }
        public decimal Superannuation { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal MedicareLevy { get; set; }
        public decimal BudgetRepairLevy { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal NetIncome { get; set; }
        public decimal PayBucket { get; set; }
        public void CalculateSalaryBreakdown(decimal grossAmount, PayFrequency frequency);
    }
}
