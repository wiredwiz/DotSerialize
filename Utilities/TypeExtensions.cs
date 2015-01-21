using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Utilities
{
   public static class TypeExtensions
   {
      public static object GetDefaultValue(this Type t)
      {
         if (t.IsValueType)
            return Activator.CreateInstance(t);

         return null;
      }
   }
}
