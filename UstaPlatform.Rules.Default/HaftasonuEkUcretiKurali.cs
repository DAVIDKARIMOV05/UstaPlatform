using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Rules.Default
{
    
    public class HaftasonuEkUcretiKurali : IPricingRule
    {
        public string RuleName => "Hafta Sonu Ek Ücreti";

        public decimal CalculatePriceAdjustment(WorkOrder workOrder)
        {
            var gun = workOrder.PlanlananTarih.DayOfWeek;
            if (gun == DayOfWeek.Saturday || gun == DayOfWeek.Sunday)
            {
                return 75.0m; // Hafta sonu ise +75 TL
            }
            return 0m;
        }
    }
}
