using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Serializers.Caching;

namespace Org.Edgerunner.DotSerialize.Serializers
{
   public class GenericTypeSerializer
   {
      protected ITypeInspector Inspector { get; set; }
      protected IReferenceCache ReferenceCache { get; set; }

      public virtual T Deserialize<T>(XmlReader reader)
      {
         T result;
         while ((reader.NodeType != XmlNodeType.Attribute) && (reader.NodeType != XmlNodeType.Element))
            reader.Read();
         if (TypeHelper.IsPrimitive(typeof(T)))
            return (T)ReadPrimitive<T>(reader);
         else if (TypeHelper.IsArray(typeof(T)))
            ; // do stuff
         else if (TypeHelper.IsEnum(typeof(T)))
            ; // do stuff
         Guid refId = TypeHelper.GetReferenceId(reader);
         if (refId != Guid.Empty)
         {
            result = (T)ReferenceCache.GetObjectById(refId);
            if (result != null)
               return result;
         }
         // continue deserializing
         var info = Inspector.GetInfo(TypeHelper.GetReferenceType(reader));
         return default(T);
      }

      public virtual void Serialize<T>(XmlWriter writer, T obj)
      {
         
      }

      public object ReadPrimitive<T>(XmlReader reader)
      {
         if (typeof(T) == typeof(Int16))
            return reader.IsEmptyElement ? default(Int16) : Int16.Parse(reader.ReadContentAsString());
         if (typeof(T) == typeof(Int32))
            return reader.IsEmptyElement ? default(Int32) : Int32.Parse(reader.ReadContentAsString());
         if (typeof(T) == typeof(Int64))
            return reader.IsEmptyElement ? default(Int64) : Int64.Parse(reader.ReadContentAsString());
         if (typeof(T) == typeof(int))
            return reader.IsEmptyElement ? default(int) : int.Parse(reader.ReadContentAsString());
         if (typeof(T) == typeof(string))
            return reader.IsEmptyElement ? string.Empty : reader.ReadContentAsString();
         if (typeof(T) == typeof(Char))
            return reader.IsEmptyElement ? default(Char) : Char.Parse(reader.ReadContentAsString());
         if (typeof(T) == typeof(Byte))
            return reader.IsEmptyElement ? default(Byte) : Byte.Parse(reader.ReadContentAsString());
         if (typeof(T) == typeof(Single))
            return reader.IsEmptyElement ? default(Single) : Single.Parse(reader.ReadContentAsString());
         if (typeof(T) == typeof(Double))
            return reader.IsEmptyElement ? default(Double) : Double.Parse(reader.ReadContentAsString());
         if (typeof(T) == typeof(Decimal))
            return reader.IsEmptyElement ? default(Decimal) : Decimal.Parse(reader.ReadContentAsString());
         if (typeof(T) == typeof(Boolean))
            return !reader.IsEmptyElement && Boolean.Parse(reader.ReadContentAsString());
         if (typeof(T) == typeof(DateTime))
            return reader.IsEmptyElement ? default(DateTime) : DateTime.Parse(reader.ReadContentAsString());
         return null;
      }
   }
}
