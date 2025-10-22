using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Rules.Loyalty
{
    
    // Bu kural, ana uygulama değişmeden sisteme eklenecek
    public class LoyaltyDiscountRule : IPricingRule
    {
        public string RuleName => "Sadakat İndirimi (%10)";

        public decimal CalculatePriceAdjustment(WorkOrder workOrder)
        {
            // %10 indirim uygula
            decimal indirim = workOrder.TemelUcret * 0.10m;
            return -indirim; // İndirim olduğu için negatif
        }
    }
}
