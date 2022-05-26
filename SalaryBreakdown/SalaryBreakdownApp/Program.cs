using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SalaryBreakdownApp.SalaryPackage;
using System;

namespace SalaryBreakdownApp
{
    class Program
    {
        private readonly ILogger<Program> _logger;
        private readonly ISalaryBreakdown _salaryBreakdown;

        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            host.Services.GetRequiredService<Program>().Run();
        }

        public Program(ILogger<Program> logger, ISalaryBreakdown salaryBreakdown)
        {
            _logger = logger;
            _salaryBreakdown = salaryBreakdown;
        }

        public void Run()
        {
            Console.Write("Enter your salary package amount: ");
            var grossAmountInput = Console.ReadLine();
            decimal grossAmount;

            //Validate the gross amount entered
            if (!Decimal.TryParse(grossAmountInput, out grossAmount))
            {
                Console.WriteLine("\nThe salary package amount you entered is incorrect.");
                return;
            }

            Console.Write("Enter your pay frequency (W for weekly, F for fortnightly, M for monthly): ");
            string payFrequenceInput = Console.ReadLine();

            //Validate the Pay frequency entered
            if (!payFrequenceInput.IsValidPayRequency())
            {
                Console.WriteLine("\nThe Pay frequency you entered is incorrect.");
                return;
            }

            //Convert input pay frequency to PayFrequency enum
            var payFrequency = payFrequenceInput.ToUpper() == "M" ? PayFrequency.month : payFrequenceInput.ToUpper() == "W" ? PayFrequency.week : PayFrequency.fortnight;

            Console.WriteLine("\nCalculating salary details...");

            _salaryBreakdown.CalculateSalaryBreakdown(grossAmount, payFrequency);

            Console.WriteLine($"\nGross package: {_salaryBreakdown.GrossIncome.ToCurrency()}");
            Console.WriteLine($"Superannuation: {_salaryBreakdown.Superannuation.ToCurrency()}");
            Console.WriteLine($"\nTaxable income: {_salaryBreakdown.TaxableIncome.ToCurrency()}");

            Console.WriteLine("\nDeductions:");
            Console.WriteLine($"Medicare Levy: {_salaryBreakdown.MedicareLevy.ToCurrency()}");
            Console.WriteLine($"Budget Repair Levy: {_salaryBreakdown.BudgetRepairLevy.ToCurrency()}");
            Console.WriteLine($"Income Tax: {_salaryBreakdown.IncomeTax.ToCurrency()}");

            Console.WriteLine($"\nNet income: {_salaryBreakdown.NetIncome.ToCurrency()}");
            Console.WriteLine($"Pay packet: {_salaryBreakdown.PayBucket.ToCurrency()} per {payFrequency.ToString()}");

            Console.WriteLine("\nPress any key to end...");
            Console.ReadKey();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((configuration) =>
                {
                    configuration.Sources.Clear();
                    configuration
                        .AddJsonFile("TaxRules.json", optional: false, reloadOnChange: true)
                        .Build();
                })
                .ConfigureServices(services =>
                {
                    services.AddTransient<Program>();
                    services.AddTransient<ISalaryBreakdown, SalaryBreakdown>();
                    services.AddTransient<TaxBracketRules>();
                });
        }
    }
}
