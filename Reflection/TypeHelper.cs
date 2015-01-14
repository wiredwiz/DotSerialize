using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public static class TypeHelper
   {
      public static bool IsPrimitive(Type type)
      {
         switch (type.Name().TrimEnd('[', ']'))
         {
            case "System.Byte":
            case "System.Int16":
            case "System.Int32":
            case "System.Int64":
            case "System.Single":
            case "System.Double":
            case "System.Decimal":
            case "System.Boolean":
            case "System.DateTime":
            case "System.Char":
            case "System.String":
               return true;
            default:
               return false;
         }
      }

      public static bool IsEnum(Type type)
      {
         return type.IsEnum;
      }
   }
}
