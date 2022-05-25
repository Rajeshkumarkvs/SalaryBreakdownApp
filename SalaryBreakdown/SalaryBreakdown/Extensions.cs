using System;
using System.Globalization;

namespace SalaryBreakdownApp.SalaryPackage
{
    public static partial class Extensions
    {
        /// <summary>
        /// Round the decimal value to specified decimal places
        /// </summary>
        /// <param name="d">decimal number</param>
        /// <param name="decimalPlaces">number of places to round up</param>
        /// <returns>
        /// <see cref="decimal"/> Rounded decimal value
        /// </returns>
        public static decimal Round(this decimal d, int decimalPlaces)
        {
            return Math.Round(d, decimalPlaces);
        }

        /// <summary>
        /// Round up the decimal value to nearest dollar value
        /// </summary>
        /// <param name="d">decimal value that needs to be rounded up to the nearest dollar</param>
        /// <returns>
        /// A <see cref="decimal"/> that is rounded up to nearest dollar
        /// </returns>
        public static decimal RoundUpToDollar(this decimal d)
        {
            return Math.Ceiling(d);
        }

        /// <summary>
        /// Round the decimal value to the nearest value
        /// </summary>
        /// <param name="d">decimal number</param>
        /// <returns>
        /// <see cref="decimal"/> Rounded decimal value to nearest value
        /// </returns>
        public static decimal Round(this decimal d)
        {
            return Math.Round(d);
        }

        /// <summary>
        /// Round the decimal value to the nearest integral value that is less than or equal to the specified decimal value 
        /// </summary>
        /// <param name="d">decimal number</param>
        /// <returns>
        /// A <see cref="decimal"/> that is rounded to nearest integral value that is less than or equal to the specified decimal value
        /// </returns>
        public static decimal RoundDown(this decimal d)
        {
            return Math.Floor(d);
        }

        /// <summary>
        /// Returns the decimal values in currency format
        /// </summary>
        /// <param name="d">decimal value that needs to displayed in currency format</param>
        /// <returns>
        /// A <see cref="string"/> that is formatted from decimal value
        /// </returns>
        public static string ToCurrency(this decimal d)
        {
            return d.ToString("C", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Represents function to check whether the string value is present in list of values
        /// </summary>
        /// <param name="s">
        /// The <see cref="string"/> that needs to be validated
        /// </param>
        /// <param name="values">
        /// <see cref="string[]"/> list of values to check
        /// </param>
        /// <returns>
        /// A <see cref="bool"/> value. True if the string found in the list of values provided. Otherwise false.
        /// </returns>
        public static bool ExistsIn(this string s, string[] values)
        {
            bool isStringfoundInList = false;

            foreach (var item in values)
            {
                if (s == item)
                {
                    isStringfoundInList = true;
                    break;
                }
            }

            return isStringfoundInList;
        }
    }
}
