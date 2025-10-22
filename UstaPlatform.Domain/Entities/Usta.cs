using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UstaPlatform.Domain.Entities
{
  
    public class Usta
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string AdSoyad { get; set; }
        public string UzmanlikAlani { get; set; }
        public double Puan { get; set; }
        public int Yogunluk { get; set; } // Üzerindeki iş yükü
    }
}