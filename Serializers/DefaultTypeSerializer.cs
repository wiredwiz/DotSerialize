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
using System.Collections.Generic;
using System.Xml;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Serializers.Reference;
using System.Reflection;
using System.Collections;

namespace Org.Edgerunner.DotSerialize.Serializers
{
   public class DefaultTypeSerializer
   {
      protected ITypeSerializerFactory Factory { get; set; }
      protected IReferenceManager ReferenceManager { get; set; }
      public ITypeInspector TypeInspector { get; set; }

      /// <summary>
      ///    Initializes a new instance of the <see cref="GenericTypeSerializer" /> class.
      /// </summary>
      /// <param name="factory"></param>
      /// <param name="referenceManager"></param>
      /// <param name="typeInspector"></param>
      public DefaultTypeSerializer(ITypeSerializerFactory factory, IReferenceManager referenceManager, ITypeInspector typeInspector)
      {
         Factory = factory;
         ReferenceManager = referenceManager;
         TypeInspector = typeInspector;
      }

      public virtual T Deserialize<T>(XmlReader reader)
      {
         return (T)Deserialize(typeof(T), reader);
      }

      public virtual object Deserialize(Type type, XmlReader reader)
      {
         object result = null;
         var memberValues = new Dictionary<string, object>();
         // Handle attributes
         if ((reader.NodeType == XmlNodeType.Attribute))
         {
            if (!((TypeHelper.IsPrimitive(type)) || (TypeHelper.IsEnum(type))))
               throw new SerializationException("Only primitives or enums can be stored in attributes");

            if (TypeHelper.IsPrimitive(type))
               result = ReadPrimitive(type, reader);
            else if (TypeHelper.IsEnum(type))
               result = ReadEnum(type, reader);
            else
               throw new SerializationException(string.Format("Unable to deserialize unexpected type \"{0}\"", type.Name()));
            return result;
         }
         // Handle Elements
         if (reader.NodeType == XmlNodeType.Element)
         {
            if (TypeHelper.IsPrimitive(type))
               result = ReadPrimitive(type, reader);
            else if (TypeHelper.IsArray(type))
               result = ReadArray(type, reader);
            else if (TypeHelper.IsEnum(type))
               result = ReadEnum(type, reader);
            else
            {
               // Class or struct
               if (TypeHelper.ReferenceIsNull(reader))
                  return null;

               var info = TypeInspector.GetInfo(type);

               // read attributes
               while (reader.MoveToNextAttribute())
                  DeserializeAttribMember(reader, info, memberValues);
               reader.MoveToContent();

               if (!reader.IsEmptyElement)
                  // read child elements
                  while (ReadNextElement(reader))
                     if (reader.NodeType == XmlNodeType.Element)
                        DeserializeElementMember(reader, info, memberValues);

               result = type.TryCreateInstance(memberValues);
               if (result == null)
                  throw new SerializationException(string.Format("Unable to create an instance of type \"{0}\"", type.Name()));
            }

            // continue deserializing            
            return result;
         }
         throw new SerializationException(
            string.Format("Reader was not positioned on a node of type Attribute or Element.\r\n" +
                          "A custom type serializer probably positioned the reader incorrectly." +
                          "Unable to deserialize type \"{0}\".",
                          type.Name()));
      }

      protected virtual void DeserializeAttribMember(XmlReader reader,
                                                  TypeSerializationInfo info,
                                                  Dictionary<string, object> memberValues)
      {
         if (!info.MemberInfoByEntityName.ContainsKey(reader.Name))
            return;
         if (!info.MemberInfoByEntityName[reader.Name].IsAttribute)
            return;
         var memberInfo = info.MemberInfoByEntityName[reader.Name];
         // Attempt to fetch a custom type serializer
         ITypeSerializer typeSerializer =
            Factory.CallMethod(new[] { memberInfo.DataType }, "GetTypeSerializer") as ITypeSerializer;
         if (typeSerializer != null)
            memberValues.Add(memberInfo.Name, typeSerializer.Deserialize(memberInfo.DataType, reader));
         else
         // Since there was no bound custom type serializer we default to the GenericTypeSerializer
         {
            var genericSerializer = Factory.GetDefaultSerializer();
            memberValues.Add(memberInfo.Name, genericSerializer.Deserialize(memberInfo.DataType, reader));
         }
      }

      protected virtual void DeserializeElementMember(XmlReader reader, TypeSerializationInfo info, Dictionary<string, object> memberValues)
      {
         if (!info.MemberInfoByEntityName.ContainsKey(reader.Name))
            return;
         if (info.MemberInfoByEntityName[reader.Name].IsAttribute)
            return;
         var memberInfo = info.MemberInfoByEntityName[reader.Name];
         var type = TypeHelper.GetReferenceType(reader);
         if (memberInfo.DataType != type)
            throw new SerializationException(string.Format("Type attribute of element {0} does not match the object's actual member type of {1}",
                                                           reader.Name,
                                                           memberInfo.DataType));
         var id = TypeHelper.GetReferenceId(reader);
         if (id != Guid.Empty)
            if (ReferenceManager.IsRegistered(id))
            {
               if (memberInfo.Type == TypeMemberSerializationInfo.MemberType.Field)
                  ReferenceManager.MemberReferences(id).LogPendingReference(MemberTypes.Field, memberInfo.Name);
               else if (memberInfo.Type == TypeMemberSerializationInfo.MemberType.Property)
                  ReferenceManager.MemberReferences(id).LogPendingReference(MemberTypes.Property, memberInfo.Name);
            }
            else
               ReferenceManager.RegisterId(id);

         object result = null;
         ITypeSerializer typeSerializer =
            Factory.CallMethod(new[] { memberInfo.DataType }, "GetTypeSerializer") as ITypeSerializer;
         if (typeSerializer != null)
         {
            result = typeSerializer.Deserialize(memberInfo.DataType, reader);
            memberValues.Add(memberInfo.Name, result);
         }
         else
         // Since there was no bound custom type serializer we default to the GenericTypeSerializer
         {
            var genericSerializer = Factory.GetDefaultSerializer();
            result = genericSerializer.Deserialize(memberInfo.DataType, reader);
            memberValues.Add(memberInfo.Name, result);
         }

         // Now that we have our object constructed we update any refences that should point to it in our object graph
         if (id != Guid.Empty)
         {
            ReferenceManager.MemberReferences(id).SavePendingReferences(result);
            ReferenceManager.UpdateObject(id, result);
         }
      }

      protected bool ReadToTextNode(XmlReader reader)
      {
         while (reader.NodeType != XmlNodeType.Text)
         {
            if (!reader.Read())
               return false;
         }
         return true;
      }

      protected bool ReadToElementEndNode(XmlReader reader)
      {
         while (reader.NodeType != XmlNodeType.EndElement)
         {
            if (!reader.Read())
               return false;
         }
         return true;
      }

      public virtual void Serialize<T>(XmlWriter writer, T obj)
      {
         Serialize(writer, typeof(T), obj);
      }

      public void Serialize(XmlWriter writer, Type type, object obj)
      {
         throw new NotImplementedException();
      }

      protected virtual bool ReadNextElement(XmlReader reader)
      {
         while (reader.Read())
            if (reader.NodeType == XmlNodeType.Element)
               return true;
            else if (reader.NodeType == XmlNodeType.EndElement)
               return false;
         return false;
      }

      protected virtual object ReadArray(Type type, XmlReader reader)
      {
         bool isElement = (reader.NodeType == XmlNodeType.Element);
         if (isElement)
            ReadToTextNode(reader);
         throw new NotImplementedException();
         if (isElement)
            ReadToElementEndNode(reader);
      }

      protected virtual object ReadEnum(Type type, XmlReader reader)
      {
         bool isElement = (reader.NodeType == XmlNodeType.Element);
         if (isElement)
            ReadToTextNode(reader);
         var result = Enum.Parse(type, reader.ReadContentAsString());
         if (isElement)
            ReadToElementEndNode(reader);
         return result;
      }

      protected virtual object ReadPrimitive(Type type, XmlReader reader)
      {
         object result = null;
         bool isElement = (reader.NodeType == XmlNodeType.Element);
         if (isElement)
            ReadToTextNode(reader);
         if (type == typeof(Int16))
            result = reader.IsEmptyElement ? default(Int16) : Int16.Parse(reader.ReadContentAsString());
         if (type == typeof(Int32))
            result = reader.IsEmptyElement ? default(Int32) : Int32.Parse(reader.ReadContentAsString());
         if (type == typeof(Int64))
            result = reader.IsEmptyElement ? default(Int64) : Int64.Parse(reader.ReadContentAsString());
         if (type == typeof(string))
            result = reader.IsEmptyElement ? string.Empty : reader.ReadContentAsString();
         if (type == typeof(Char))
            result = reader.IsEmptyElement ? default(Char) : Char.Parse(reader.ReadContentAsString());
         if (type == typeof(Byte))
            result = reader.IsEmptyElement ? default(Byte) : Byte.Parse(reader.ReadContentAsString());
         if (type == typeof(Single))
            result = reader.IsEmptyElement ? default(Single) : Single.Parse(reader.ReadContentAsString());
         if (type == typeof(Double))
            result = reader.IsEmptyElement ? default(Double) : Double.Parse(reader.ReadContentAsString());
         if (type == typeof(Decimal))
            result = reader.IsEmptyElement ? default(Decimal) : Decimal.Parse(reader.ReadContentAsString());
         if (type == typeof(Boolean))
            result = !reader.IsEmptyElement && Boolean.Parse(reader.ReadContentAsString());
         if (type == typeof(DateTime))
            result = reader.IsEmptyElement ? default(DateTime) : DateTime.Parse(reader.ReadContentAsString());
         if (isElement)
            ReadToElementEndNode(reader);
         return result;
      }
   }
}