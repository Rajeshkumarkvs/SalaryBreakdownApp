using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SalaryBreakdownApp.SalaryPackage;

namespace SalaryBreakdownTest
{
    [TestClass]
    public class SalaryBreakdownUnitTest
    {
        private readonly SalaryBreakdown _salaryBreakdown;

        public SalaryBreakdownUnitTest()
        {
            var mock = new Mock<ILogger<SalaryBreakdown>>();
            ILogger<SalaryBreakdown> _logger = mock.Object;
            var config = new ConfigurationBuilder()
                .AddJsonFile("MockTaxRules.json")
                .Build();

            _salaryBreakdown = new SalaryBreakdown(_logger, config);
        }

        [TestMethod]
        public void ValidateTaxableIncome()
        {
            _salaryBreakdown.CalculateSalaryBreakdown(65000M, PayFrequency.month);
            Assert.AreEqual(_salaryBreakdown.TaxableIncome.Round(2), 59360.73M);
        }

        [TestMethod]
        public void ValidateSuperannuation()
        {
            _salaryBreakdown.CalculateSalaryBreakdown(65000M, PayFrequency.month);
            Assert.AreEqual(_salaryBreakdown.Superannuation.Round(2), 5639.27M);
        }

        [TestMethod]
        public void ValidateMedicareLevy()
        {
            _salaryBreakdown.CalculateSalaryBreakdown(65000M, PayFrequency.month);
            Assert.AreEqual(_salaryBreakdown.MedicareLevy.Round(2), 1188M);
        }

        [TestMethod]
        public void ValidateBudgetRepairLevy()
        {
            _salaryBreakdown.CalculateSalaryBreakdown(65000M, PayFrequency.month);
            Assert.AreEqual(_salaryBreakdown.BudgetRepairLevy.Round(2), 0);
        }

        [TestMethod]
        public void ValidateIncomeTax()
        {
            _salaryBreakdown.CalculateSalaryBreakdown(65000M, PayFrequency.month);
            Assert.AreEqual(_salaryBreakdown.IncomeTax.Round(2), 10839M);
        }

        [TestMethod]
        public void ValidateNetIncome()
        {
            _salaryBreakdown.CalculateSalaryBreakdown(65000M, PayFrequency.month);
            Assert.AreEqual(_salaryBreakdown.NetIncome.Round(2), 47333.73M);
        }

        [TestMethod]
        public void ValidatePayBucketForMonthly()
        {
            _salaryBreakdown.CalculateSalaryBreakdown(65000M, PayFrequency.month);
            Assert.AreEqual(_salaryBreakdown.PayBucket.Round(2), 3944.48M);
        }

        [TestMethod]
        public void ValidatePayBucketForWeekly()
        {
            _salaryBreakdown.CalculateSalaryBreakdown(65000M, PayFrequency.week);
            Assert.AreEqual(_salaryBreakdown.PayBucket.Round(2), 910.26M);
        }

        [TestMethod]
        public void ValidatePayBucketForFortnightly()
        {
            _salaryBreakdown.CalculateSalaryBreakdown(65000M, PayFrequency.fortnight);
            Assert.AreEqual(_salaryBreakdown.PayBucket.Round(2), 1820.53M);
        }
    }
}