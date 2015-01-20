using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Fasterflect;

namespace Org.Edgerunner.DotSerialize.Utilities
{
   public static class FieldInfoExtensions
   {
      public static bool IsBackingField(this FieldInfo info)
      {
         var propertyName = NamingUtils.GetAutoPropertyName(info.Name);
         return !string.IsNullOrEmpty(propertyName);
      }

      public static PropertyInfo GetEncapsulatingAutoProperty(this FieldInfo info)
      {
         var propertyName = NamingUtils.GetAutoPropertyName(info.Name);
         if (!string.IsNullOrEmpty(propertyName))
            return info.DeclaringType.Property(propertyName);

         return null;
      }
   }
}
