using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaPlatform.Domain.Entities
{
   
    public class WorkOrder
    {
      
        public Guid Id { get; init; }
        public DateTime KayitZamani { get; init; }

        public string IsTanimi { get; set; }
        public DateOnly PlanlananTarih { get; set; }
        public Usta AtanmisUsta { get; set; }
        public Vatandas TalepEden { get; set; }
        public decimal TemelUcret { get; set; }
        public decimal SonFiyat { get; set; } // Kurallar uygulandıktan sonra

        public WorkOrder()
        {
            Id = Guid.NewGuid();
            KayitZamani = DateTime.UtcNow;
        }
    }
}