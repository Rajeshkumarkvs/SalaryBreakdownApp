using System;
using System.Collections.Generic;
using System.Text;

namespace SalaryBreakdownApp.SalaryPackage
{
    /// <summary>
    /// Represents the class that is used for Tax bracket rules when calculating salary breakdown
    /// </summary>
    public class TaxBracketRules
    {
        public decimal SuperContribution { get; set; }
        public List<MedicareLevy> MedicareLevy { get; set; }
        public List<BudgetRepairLevy> BudgetRepairLevy { get; set; }
        public List<IncomeTax> IncomeTax { get; set; }
    }

    /// <summary>
    /// Represents the class that is used for Medicare Levy tax bracket rules while calculating Medicare Levy
    /// </summary>
    public class MedicareLevy
    {
        public decimal startAmount { get; set; }
        public decimal endAmount { get; set; }
        public decimal percent { get; set; }
        public bool calculateExcessOfMinimum { get; set; }
    }

    /// <summary>
    /// Represents the class that is used for Budget Repair Levy tax bracket rules while calculating Budget Repair Levy
    /// </summary>
    public class BudgetRepairLevy
    {
        public decimal startAmount { get; set; }
        public decimal endAmount { get; set; }
        public decimal percent { get; set; }
        public bool calculateExcessOfMinimum { get; set; }
    }

    /// <summary>
    /// Represents the class that is used for Income tax bracket rules while calculating Income tax
    /// </summary>
    public class IncomeTax
    {
        public decimal startAmount { get; set; }
        public decimal endAmount { get; set; }
        public decimal percent { get; set; }
        public bool calculateExcessOfMinimum { get; set; }
        public decimal? otherTaxAmount { get; set; }
    }    
}