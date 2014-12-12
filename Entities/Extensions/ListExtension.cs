using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Extensions
{
    public static class ListExtension
    {
        public static void AddIfNotExists<T>(this List<T> list, T element)
        {
            if (list.Contains(element)==false)
            {
                list.Add(element);
            }
        }
    }
}
