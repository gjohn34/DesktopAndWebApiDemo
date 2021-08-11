using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDesktop.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string taxRate = ConfigurationManager.AppSettings["taxRate"];

            decimal.TryParse(taxRate, out decimal output);

            if (output > 0)
            {
                return output;
            }
            throw new Exception("Tax Rate not set up.");
        }
    }
}
