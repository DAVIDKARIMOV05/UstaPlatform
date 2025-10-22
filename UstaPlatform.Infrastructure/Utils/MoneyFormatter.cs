using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;

namespace UstaPlatform.Infrastructure.Utils
{
   
    public static class MoneyFormatter
    {
        private static readonly CultureInfo _trCulture = new("tr-TR");
        public static string Format(decimal amount)
        {
            return amount.ToString("C", _trCulture); // Örn: 150,00 ₺
        }
    }
}