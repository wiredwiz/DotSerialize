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
using System.Linq;
using System.Reflection;
using System.Xml;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Reflection.Construction;
using Org.Edgerunner.DotSerialize.Serialization.Factories;
using Org.Edgerunner.DotSerialize.Serialization.Reference;

namespace Org.Edgerunner.DotSerialize.Serialization
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
         if (reader == null) throw new ArgumentNullException("reader");
         return (T)Deserialize(reader, typeof(T));
      }

      public virtual object Deserialize(XmlReader reader, Type type)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         if (type == null) throw new ArgumentNullException("type");
         object result = null;
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

               var memberValues = new Dictionary<TypeMemberSerializationInfo, object>();
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

               //result = type.TryCreateInstance(memberValues);
               result = TypeFactory.CreateInstance(type, memberValues);
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
                                                  Dictionary<TypeMemberSerializationInfo, object> memberValues)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         if (info == null) throw new ArgumentNullException("info");
         if (memberValues == null) throw new ArgumentNullException("memberValues");
         if (!info.MemberInfoByEntityName.ContainsKey(reader.Name))
            return;
         if (!info.MemberInfoByEntityName[reader.Name].IsAttribute)
            return;
         var memberInfo = info.MemberInfoByEntityName[reader.Name];
         // Attempt to fetch a custom type serializer
         ITypeSerializer typeSerializer =
            Factory.CallMethod(new[] { memberInfo.DataType }, "GetTypeSerializer") as ITypeSerializer;
         if (typeSerializer != null)
            memberValues[memberInfo] = typeSerializer.Deserialize(reader, memberInfo.DataType);
         else
         // Since there was no bound custom type serializer we default to the GenericTypeSerializer
         {
            var genericSerializer = Factory.GetDefaultSerializer();
            memberValues[memberInfo] = genericSerializer.Deserialize(reader, memberInfo.DataType);
         }
      }

      protected virtual void DeserializeElementMember(XmlReader reader, TypeSerializationInfo info, Dictionary<TypeMemberSerializationInfo, object> memberValues)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         if (info == null) throw new ArgumentNullException("info");
         if (memberValues == null) throw new ArgumentNullException("memberValues");
         if (!info.MemberInfoByEntityName.ContainsKey(reader.Name))
            return;
         if (info.MemberInfoByEntityName[reader.Name].IsAttribute)
            return;
         var memberInfo = info.MemberInfoByEntityName[reader.Name];
         Guid id = Guid.Empty;
         if (TypeHelper.IsClassOrStruct(memberInfo.DataType))
         {
            var type = TypeHelper.GetReferenceType(reader);
            if (memberInfo.DataType != type)
               throw new SerializationException(string.Format("Type attribute of element {0} does not match the object's actual member type of {1}",
                                                              reader.Name,
                                                              memberInfo.DataType));
            id = TypeHelper.GetReferenceId(reader);
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
         }

         object result = null;
         ITypeSerializer typeSerializer =
            Factory.CallMethod(new[] { memberInfo.DataType }, "GetTypeSerializer") as ITypeSerializer;
         if (typeSerializer != null)
         {
            result = typeSerializer.Deserialize(reader, memberInfo.DataType);
            memberValues[memberInfo] = result;
         }
         else
         // Since there was no bound custom type serializer we default to the GenericTypeSerializer
         {
            var genericSerializer = Factory.GetDefaultSerializer();
            result = genericSerializer.Deserialize(reader, memberInfo.DataType);
            memberValues[memberInfo] = result;
         }

         // Now that we have our object constructed we update any refences that should point to it in our object graph
         if (TypeHelper.IsClassOrStruct(memberInfo.DataType))
            if (id != Guid.Empty)
            {
               ReferenceManager.MemberReferences(id).SavePendingReferences(result);
               ReferenceManager.UpdateObject(id, result);
            }
      }

      protected bool ReadToTextNode(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         while (reader.NodeType != XmlNodeType.Text)
         {
            if (!reader.Read())
               return false;
         }
         return true;
      }

      protected bool ReadToElementEndNode(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         while (reader.NodeType != XmlNodeType.EndElement)
         {
            if (!reader.Read())
               return false;
         }
         return true;
      }

      public virtual void Serialize<T>(XmlWriter writer, T obj)
      {
         if (writer == null) throw new ArgumentNullException("writer");
         Serialize(writer, typeof(T), obj);
      }

      public void Serialize(XmlWriter writer, Type type, object obj)
      {
         if (writer == null) throw new ArgumentNullException("writer");
         if (type == null) throw new ArgumentNullException("type");

         if (TypeHelper.IsPrimitive(type))
            writer.WriteValue(obj.ToString());
         else if (TypeHelper.IsEnum(type))
            writer.WriteValue(obj.ToString());
         else if (TypeHelper.IsArray(type))
            ;// TODO: format an array
         else
         {
            writer.WriteAttributeString("reference__type", type.AssemblyQualifiedName);
            // check for null value
            if (obj == null)
            {
               writer.WriteAttributeString("reference__isNull", true.ToString());
               return;
            }
            // check for struct before writing reference id
            if (!type.IsValueType)
            {
               ReferenceManager.RegisterId(Guid.NewGuid(), obj);
               writer.WriteAttributeString("reference__id", ReferenceManager.GetObjectId(obj).ToString());
            }

            var info = TypeInspector.GetInfo(type);
            var attribs = from x in info.MemberInfoByEntityName.Values
                          where x.IsAttribute
                          select x;
            foreach (var attrib in attribs)
            {
               writer.WriteStartAttribute(attrib.EntityName);
               object entityValue = GetEntityValue(obj, attrib);
               // now we resolve a type serializer to do the work
               ITypeSerializer typeSerializer =
               Factory.CallMethod(new[] { attrib.DataType }, "GetTypeSerializer") as ITypeSerializer;
               if (typeSerializer != null)
                  typeSerializer.Serialize(writer, attrib.DataType, entityValue);
               else
               // Since there was no bound custom type serializer we default to the GenericTypeSerializer
               {
                  var genericSerializer = Factory.GetDefaultSerializer();
                  genericSerializer.Serialize(writer, attrib.DataType, entityValue);
               }
               writer.WriteEndAttribute();
            }
            var elements = from x in info.MemberInfoByEntityName.Values
                           where !x.IsAttribute
                           select x;
            foreach (var element in elements)
            {
               writer.WriteStartElement(element.EntityName);
               // now we resolve a type serializer to do the work
               ITypeSerializer typeSerializer =
               Factory.CallMethod(new[] { element.DataType }, "GetTypeSerializer") as ITypeSerializer;
               object entityValue = GetEntityValue(obj, element);
               if (typeSerializer != null)
                  typeSerializer.Serialize(writer, element.DataType, entityValue);
               else
               // Since there was no bound custom type serializer we default to the GenericTypeSerializer
               {
                  var genericSerializer = Factory.GetDefaultSerializer();
                  genericSerializer.Serialize(writer, element.DataType, entityValue);
               }
               writer.WriteEndElement();
            }
         }
      }

      public object GetEntityValue(object entity, TypeMemberSerializationInfo memberInfo)
      {
         switch (memberInfo.Type)
         {
            case TypeMemberSerializationInfo.MemberType.Field:
               return entity.GetFieldValue(memberInfo.Name);
            case TypeMemberSerializationInfo.MemberType.Property:
               return entity.GetPropertyValue(memberInfo.Name);
            default:
               throw new SerializationException("Cannot serialize unknown member type");
         }
      }
      protected virtual bool ReadNextElement(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         while (reader.Read())
            if (reader.NodeType == XmlNodeType.Element)
               return true;
            else if (reader.NodeType == XmlNodeType.EndElement)
               return false;
         return false;
      }

      protected virtual object ReadArray(Type type, XmlReader reader)
      {
         if (type == null) throw new ArgumentNullException("type");
         if (reader == null) throw new ArgumentNullException("reader");
         bool isElement = (reader.NodeType == XmlNodeType.Element);
         if (isElement)
            ReadToTextNode(reader);
         throw new NotImplementedException();
         if (isElement)
            ReadToElementEndNode(reader);
      }

      protected virtual object ReadEnum(Type type, XmlReader reader)
      {
         if (type == null) throw new ArgumentNullException("type");
         if (reader == null) throw new ArgumentNullException("reader");
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
         if (type == null) throw new ArgumentNullException("type");
         if (reader == null) throw new ArgumentNullException("reader");
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