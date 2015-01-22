using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;

namespace Org.Edgerunner.DotSerialize.Utilities
{
   public static class ObjectExtensions
   {
      public static void SetArrayPropertyValue(this object obj, string name, object value, int index)
      {
         var type = obj.GetType();
         var propInfo = type.Property(name);
         if (propInfo == null)
            throw new Exception(string.Format("Cannot set non-existant property \"{0}\"", name));
         if (!propInfo.Type().IsArray)
            throw new TargetException("Property \"{0}\" is not an array.");
         Array array = propInfo.GetValue(obj) as Array;
         if (array == null)
            throw new TargetException("The value of property \"{0}\" is null.");
         array.SetValue(value, index);
      }

      public static void SetArrayFieldValue(this object obj, string name, object value, int index)
      {
         var type = obj.GetType();
         var fieldInfo = type.Field(name);
         if (fieldInfo == null)
            throw new Exception(string.Format("Cannot set non-existant field \"{0}\"", name));
         if (!fieldInfo.Type().IsArray)
            throw new TargetException("Field \"{0}\" is not an array.");
         Array array = fieldInfo.GetValue(obj) as Array;
         if (array == null)
            throw new TargetException("The value of field \"{0}\" is null.");
         array.SetValue(value, index);
      }
   }
}
