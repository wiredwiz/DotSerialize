using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;

namespace Org.Edgerunner.DotSerialize.Utilities
{
   public static class PropertyInfoExtensions
   {
      public static bool IsAutoProperty(this PropertyInfo info)
      {
         var field = info.DeclaringType.Field(string.Format("<{0}>k__BackingField", info.Name));
         if (field == null)
            return false;
         return field.HasAttribute<CompilerGeneratedAttribute>();
      }

      public static FieldInfo GetBackingField(this PropertyInfo info)
      {
         var field = info.DeclaringType.Field(string.Format("<{0}>k__BackingField", info.Name));
         if (field == null)
            return null;
         return field.HasAttribute<CompilerGeneratedAttribute>() ? field : null;
      }
   }
}
