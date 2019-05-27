using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileScout
{
    public static class Tools
    {
        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            return items != null && !items.Any();
        }
    }
}
