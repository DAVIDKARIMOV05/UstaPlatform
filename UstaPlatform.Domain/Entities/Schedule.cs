using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace UstaPlatform.Domain.Entities
{
    
    public class Schedule
    {
        // Tarihe göre iş emirlerini listeleyen özel bir sözlük
        private readonly Dictionary<DateOnly, List<WorkOrder>> _isEmirleri = new();

        public void AddWorkOrder(WorkOrder order)
        {
            if (order == null) return;
            if (!_isEmirleri.ContainsKey(order.PlanlananTarih))
            {
                _isEmirleri[order.PlanlananTarih] = new List<WorkOrder>();
            }
            _isEmirleri[order.PlanlananTarih].Add(order);
        }

        
        public List<WorkOrder> this[DateOnly gun]
        {
            get
            {
                if (_isEmirleri.TryGetValue(gun, out var liste))
                {
                    return liste;
                }
                return new List<WorkOrder>(); // Boş liste döndür
            }
        }
    }
}