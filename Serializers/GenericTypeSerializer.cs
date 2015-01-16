#region Apache License 2.0

// Copyright 2015 Thaddeus Ryker
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Xml;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Serializers.Caching;
using Org.Edgerunner.DotSerialize;

namespace Org.Edgerunner.DotSerialize.Serializers
{
   public class GenericTypeSerializer
   {
      protected Serializer _Serializer;

      /// <summary>
      /// Initializes a new instance of the <see cref="GenericTypeSerializer"/> class.
      /// </summary>
      /// <param name="serializer"></param>
      public GenericTypeSerializer(Serializer serializer)
      {
         _Serializer = serializer;
      }

      public virtual T Deserialize<T>(XmlReader reader)
      {
         T result = default(T);
         Type dType = typeof(T);

         // Handle attributes
         if ((reader.NodeType == XmlNodeType.Attribute))
         {
            if (!((TypeHelper.IsPrimitive(dType)) || (TypeHelper.IsEnum(typeof(T)))))
               throw new SerializationException("Only primitives or enums can be stored in attributes");

            if (TypeHelper.IsPrimitive(dType))
               result = (T)ReadPrimitive<T>(reader);
            else if (TypeHelper.IsEnum(dType))
               result = (T)ReadEnum<T>(reader);
            throw new SerializationException(string.Format("Unable to deserialize unexpected type \"{0}\"", dType.Name()));
         }
         // Handle Elements
         if (reader.NodeType == XmlNodeType.Element)
         {
            if (TypeHelper.IsPrimitive(dType))
               result = (T)ReadPrimitive<T>(reader);
            else if (TypeHelper.IsArray(dType))
               result = ReadArray<T>(reader);
            else if (TypeHelper.IsEnum(dType))
               result = (T)ReadEnum<T>(reader);

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
         throw new SerializationException(
            string.Format("Reader was not positioned on a node of type Attribute or Element.\r\n" +
                          "A custom type serializer probably positioned the reader incorrectly." +
                          "Unable to deserialize type \"{0}\".",
                          dType.Name()));
      }

      public virtual void Serialize<T>(XmlWriter writer, T obj)
      {
      }

      public T ReadArray<T>(XmlReader reader)
      {
         throw new NotImplementedException();
      }

      public object ReadEnum<T>(XmlReader reader)
      {
         return Enum.Parse(typeof(T), reader.ReadContentAsString());
         ;
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