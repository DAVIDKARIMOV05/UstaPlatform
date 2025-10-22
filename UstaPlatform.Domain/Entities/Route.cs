using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Collections.Generic;

namespace UstaPlatform.Domain.Entities
{
 
    public class Route : IEnumerable<(int X, int Y)>
    {
        private readonly List<(int X, int Y)> _duraklar = new();

       
        public void Add(int X, int Y)
        {
            _duraklar.Add((X, Y));
        }

        public IEnumerator<(int X, int Y)> GetEnumerator()
        {
            return _duraklar.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}