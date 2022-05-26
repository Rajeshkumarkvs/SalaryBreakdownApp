using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace SalaryBreakdownApp.SalaryPackage
{
    /// <summary>
    /// Represents the class that calculates Salary breakdown for the input Gross amount and Pay frequency
    /// </summary>
    public class SalaryBreakdown : ISalaryBreakdown
    {
        private readonly ILogger<SalaryBreakdown> _logger;
        private TaxBracketRules _taxRules { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal Superannuation { get; set; }
        public decimal TaxableIncome { get; set; }
        public decimal MedicareLevy { get; set; }
        public decimal BudgetRepairLevy { get; set; }
        public decimal IncomeTax { get; set; }
        public decimal NetIncome { get; set; }
        public decimal PayBucket { get; set; }

        public SalaryBreakdown(ILogger<SalaryBreakdown> logger, IConfiguration config)
        {
            _logger = logger;

            //load the tax bracket rules config
            _taxRules = config.GetRequiredSection("TaxRules").Get<TaxBracketRules>();
        }

        /// <summary>
        /// Represents a method that will calculate the salary break for the salary package amount (gross) and pay frequency (weekly or fortnightly or monthly)
        /// </summary>
        /// <param name="grossAmount">
        /// A <see cref="decimal"/> value for the salary package amount
        /// </param>
        /// <param name="frequency">
        /// A <see cref="Enum"/> that represents the Pay frequency. ie Weekly, Fortnightly or Monthly
        /// </param>
        public void CalculateSalaryBreakdown(decimal grossAmount, PayFrequency frequency)
        {
            try
            {
                GrossIncome = grossAmount;
                TaxableIncome = ((grossAmount / (_taxRules.SuperContribution + 100)) * 100);
                Superannuation = (((TaxableIncome * _taxRules.SuperContribution)) / 100).Round(2);
                MedicareLevy = GetMedicareLevy(_taxRules.MedicareLevy);
                BudgetRepairLevy = GetBudgetRepairLevy(_taxRules.BudgetRepairLevy);
                IncomeTax = GetIncomeTax(_taxRules.IncomeTax);
                NetIncome = GrossIncome - Superannuation - (MedicareLevy + BudgetRepairLevy + IncomeTax);
                PayBucket = GetPayBucket(NetIncome, frequency);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while calculating Salary package. Error details {ex.Message}");
                throw;
            }            
        }

        /// <summary>
        /// Represents the method to calculate the Medicare Levy deduction for the salary package. This calculation is based on rounded down Taxable income.
        /// </summary>
        /// <param name="medicareLevyRules">
        /// A list of rules that is used to calculate medicare levy based on rounded down Taxable income.
        /// </param>
        /// <returns>
        /// A <see cref="decimal"/> Medicare levy amount that is rounded up to the nearest dollar.
        /// </returns>
        private decimal GetMedicareLevy(List<MedicareLevy> medicareLevyRules)
        {
            decimal taxableIncome = TaxableIncome.RoundDown();
            decimal medicareLevyAmount = 0;

            foreach (var range in medicareLevyRules)
            {
                //No calculation required if the percentage is 0
                if (range.percent == 0)
                    continue;

                if (taxableIncome >= range.startAmount && taxableIncome <= range.endAmount) //Salary is inbetween the range of amount
                    medicareLevyAmount = ((taxableIncome - (range.startAmount - 1)) * range.percent) / 100;
                else if (taxableIncome > range.startAmount && range.endAmount == -1) // when range has no end amount
                    medicareLevyAmount = (taxableIncome * range.percent) / 100;
            }

            return medicareLevyAmount.RoundUpToDollar();
        }

        /// <summary>
        /// Represents the method to calculate the Budget repair Levy deduction for the salary package. This calculation is based on rounded down Taxable income.
        /// </summary>
        /// <param name="BudgetRepairLevy">
        /// A list of rules that is used to calculate Budget repair levy based on rounded down Taxable income.
        /// </param>
        /// <returns>
        /// A <see cref="decimal"/> Budget repair levy amount that is rounded up to the nearest dollar.
        /// </returns>
        private decimal GetBudgetRepairLevy(List<BudgetRepairLevy> budgetRepairLevyRules)
        {
            decimal taxableIncome = TaxableIncome.RoundDown();
            decimal budgetRepairLevyAmount = 0;

            foreach (var range in budgetRepairLevyRules)
            {
                //No calculation required if the percentage is 0
                if (range.percent == 0)
                    continue;

                if (taxableIncome >= range.startAmount && taxableIncome <= range.endAmount) //Salary is inbetween the range of amount
                    budgetRepairLevyAmount = ((taxableIncome - (range.startAmount - 1)) * range.percent) / 100;
                else if (taxableIncome >= range.startAmount && taxableIncome == -1) // when range has no end amount
                    budgetRepairLevyAmount = ((taxableIncome - (range.startAmount - 1)) * range.percent) / 100;
            }

            return budgetRepairLevyAmount.RoundUpToDollar();
        }

        /// <summary>
        /// Represents the method to calculate the Income tax deduction for the salary package. This calculation is based on rounded down Taxable income.
        /// </summary>
        /// <param name="IncomeTax">
        /// A list of rules that is used to calculate Income tax based on rounded down Taxable income.
        /// </param>
        /// <returns>
        /// A <see cref="decimal"/> Income tax amount that is rounded up to the nearest dollar.
        /// </returns>
        private decimal GetIncomeTax(List<IncomeTax> incomeTaxRules)
        {
            decimal taxableIncome = TaxableIncome.RoundDown();
            decimal incomeTaxAmount = 0;

            foreach (var range in incomeTaxRules)
            {
                //No calculation required if the percentage is 0
                if (range.percent == 0)
                    continue;

                //when taxable income is within the bracket
                if (taxableIncome >= range.startAmount && taxableIncome <= range.endAmount)
                {
                    incomeTaxAmount = ((taxableIncome - (range.startAmount - 1)) * range.percent) / 100;

                    //Add additional amount for this tax bracket
                    if (range.otherTaxAmount != null)
                        incomeTaxAmount += (decimal)range.otherTaxAmount;
                }
                else if (taxableIncome >= range.startAmount && taxableIncome == -1) //when taxable income greater than the tax bracket
                {
                    incomeTaxAmount = ((taxableIncome - (range.startAmount - 1)) * range.percent) / 100;

                    //Add additional amount for this tax bracket
                    if (range.otherTaxAmount != null)
                        incomeTaxAmount += (decimal)range.otherTaxAmount;
                }
            }

            return incomeTaxAmount.RoundUpToDollar();
        }

        /// <summary>
        /// Represents the method to calculate the Pay bucket amount for the salary package and pay frequency. This calculation is based on Net income.
        /// </summary>
        /// <param name="netIncome">
        /// A <see cref="decimal"/> that represents the Net Income in the salary package
        /// </param>
        /// <param name="frequency">
        /// A <see cref="Enum"/> that represents the frequency of the pay bucket. I.e Weekly, Fortnightly or Monthly
        /// </param>
        /// <returns>
        /// A <see cref="decimal"/> that represents the Pay Bucket amount
        /// </returns>
        private decimal GetPayBucket(decimal netIncome, PayFrequency frequency)
        {
            decimal payBucketAmount = netIncome / (int)frequency;

            return payBucketAmount.Round(2);
        }
    }
}