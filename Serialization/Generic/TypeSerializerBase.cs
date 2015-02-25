﻿#region Apache License 2.0

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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Properties;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Reflection.Construction;
using Org.Edgerunner.DotSerialize.Serialization.Factories;
using Org.Edgerunner.DotSerialize.Serialization.Reference;
using Org.Edgerunner.DotSerialize.Utilities;
using TypeInfo = Org.Edgerunner.DotSerialize.Reflection.TypeInfo;

namespace Org.Edgerunner.DotSerialize.Serialization.Generic
{
   public abstract class TypeSerializerBase<T> : ITypeSerializer<T>
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeSerializerBase" /> class.
      /// </summary>
      /// <param name="settings"></param>
      /// <param name="factory"></param>
      /// <param name="inspector"></param>
      /// <param name="refManager"></param>
      protected TypeSerializerBase(Settings settings,
                                   ITypeSerializerFactory factory,
                                   ITypeInspector inspector,
                                   IReferenceManager refManager)
      {
         Factory = factory;
         Inspector = inspector;
         Settings = settings;
         RefManager = refManager;
      }

      protected ITypeSerializerFactory Factory { get; set; }
      protected ITypeInspector Inspector { get; set; }
      protected Settings Settings { get; set; }
      protected IReferenceManager RefManager { get; set; }

      protected virtual object DeserializeAttribute(XmlReader reader, Type type)
      {
         if (!((TypeHelper.IsPrimitive(type)) || (TypeHelper.IsEnum(type))))
            throw new SerializerException("Only primitives or enums can be stored in attributes.");

         object result;

         if (TypeHelper.IsPrimitive(type))
            result = DeserializePrimitive(type, reader);
         else if (TypeHelper.IsEnum(type))
            result = DeserializeEnum(type, reader);
         else
            throw new SerializerException(string.Format("Unable to deserialize unexpected type \"{0}\" from an attribute.",
                                                        type.Name()));
         return result;
      }

      protected virtual object DeserializeElement(XmlReader reader, Type type)
      {
         object result = null;
         var actualType = TypeHelper.GetReferenceType(reader) ?? type;
         if (TypeHelper.IsPrimitive(actualType))
            result = DeserializePrimitive(actualType, reader);
         else if (TypeHelper.IsArray(actualType))
            result = DeserializeArray(actualType, reader);
         else if (TypeHelper.IsEnum(actualType))
            result = DeserializeEnum(actualType, reader);
         else
         {
            // Class or struct
            if (TypeHelper.ReferenceIsNull(reader))
               return null;

            var memberValues = new Dictionary<TypeMemberInfo, object>();
            RefManager.StartLateBindingCapture(actualType);
            var info = Inspector.GetInfo(actualType);

            // read attributes
            while (reader.MoveToNextAttribute())
               DeserializeAttribMember(reader, info, memberValues);
            reader.MoveToContent();

            if (!reader.IsEmptyElement)
               // read child elements
               // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
               while (ReadNextElement(reader))
                  if (reader.NodeType == XmlNodeType.Element)
                     DeserializeElementMember(reader, info, memberValues);

            result = TypeFactory.CreateInstance(actualType, memberValues);
            if (result == null)
               throw new SerializerException(string.Format("Unable to create an instance of type \"{0}\".", type.Name()));

            RefManager.FinishCaptures(result);
         }
         return result;
      }

      public virtual object Deserialize(XmlReader reader, Type type)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         if (type == null) throw new ArgumentNullException("type");

         // Handle attributes
         if ((reader.NodeType == XmlNodeType.Attribute))
            return DeserializeAttribute(reader, type);

         // Handle Elements
         if (reader.NodeType == XmlNodeType.Element)
            return DeserializeElement(reader, type);

         throw new SerializerException(
            string.Format("Reader was not positioned on a node of type Attribute or Element.\r\n" +
                          "A custom type serializer probably positioned the reader incorrectly." +
                          "Unable to deserialize type \"{0}\".",
                          type.Name()));
      }

      protected virtual void SerializeAttribute(XmlWriter writer, object obj, TypeMemberInfo attribute)
      {
         writer.WriteStartAttribute(attribute.EntityName);
         object entityValue = GetEntityValue(obj, attribute);
         // now we resolve a type serializer to do the work
         ITypeSerializer typeSerializer =
            Factory.CallMethod(new[] { attribute.DataType }, "GetTypeSerializer") as ITypeSerializer;
         if (typeSerializer != null)
            typeSerializer.Serialize(writer, attribute.DataType, entityValue);
         else
         // Since there was no bound custom type serializer we default to the GenericTypeSerializer
         {
            var defaultSerializer = Factory.GetDefaultSerializer();
            defaultSerializer.Serialize(writer, attribute.DataType, entityValue);
         }
         writer.WriteEndAttribute();
      }

      protected virtual void SerializeElement(XmlWriter writer, object obj, TypeMemberInfo element)
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
            var defaultSerializer = Factory.GetDefaultSerializer();
            defaultSerializer.Serialize(writer, element.DataType, entityValue);
         }
         writer.WriteEndElement();
      }

      public virtual void Serialize(XmlWriter writer, Type type, object obj)
      {
         if (writer == null) throw new ArgumentNullException("writer");
         if (type == null) throw new ArgumentNullException("type");

         if (TypeHelper.IsPrimitive(type))
            writer.WriteValue(FormatPrimitive(type, obj, Settings.Culture));
         else if (TypeHelper.IsEnum(type))
            writer.WriteValue(obj.ToString());
         else
         {
            Type actualType = null;
            // check for null value
            if (obj == null)
            {
               actualType = type;
               writer.WriteAttributeString(Resources.ReferenceisNull,
                                           Resources.XsiUri,
                                           true.ToString().ToLowerInvariant());
               return;
            }
            actualType = obj.GetType();
            if ((actualType != type) || (!Settings.OmitTypeWhenPossible))
               writer.WriteAttributeString(Resources.ReferenceType,
                                           Resources.DotserializeUri,
                                           FormatType(actualType.AssemblyQualifiedName));
            // check for struct before writing reference id
            if (!type.IsValueType && !Settings.DisableReferentialIntegrity)
            {
               int id;
               if (RefManager.IsRegistered(obj))
               {
                  id = RefManager.GetObjectId(obj);
                  writer.WriteAttributeString(Resources.ReferenceId, Resources.DotserializeUri, id.ToString());
                  // since this object has already been serialized once there is no need to write out the rest of the object
                  return;
               }
               // Given that the instance has not already been seen we keep writing
               id = RefManager.RegisterId(obj);
               writer.WriteAttributeString(Resources.ReferenceId, Resources.DotserializeUri, id.ToString());
               writer.WriteAttributeString(Resources.ReferenceSource,
                                           Resources.DotserializeUri,
                                           true.ToString().ToLowerInvariant());
            }

            if (TypeHelper.IsArray(type))
               SerializeArray(writer, obj);
            else
            {
               var info = Inspector.GetInfo(actualType);
               var attribs = from x in info.MemberInfoByEntityName.Values
                             where x.IsAttribute
                             select x;

               foreach (var attrib in attribs)
                  SerializeAttribute(writer, obj, attrib);

               var elements = (from x in info.MemberInfoByEntityName.Values
                               where !x.IsAttribute
                               select x).OrderBy(x => x.Order).ToList();

               foreach (var element in elements)
                  SerializeElement(writer, obj, element);
            }
         }
      }

      public virtual T Deserialize(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         return (T)Deserialize(reader, typeof(T));
      }

      public virtual void Serialize(XmlWriter writer, T obj)
      {
         if (writer == null) throw new ArgumentNullException("writer");
         Serialize(writer, typeof(T), obj);
      }

      protected virtual void DeserializeAttribMember(XmlReader reader,
                                                     TypeInfo info,
                                                     Dictionary<TypeMemberInfo, object> memberValues)
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
            var defaultSerializer = Factory.GetDefaultSerializer();
            memberValues[memberInfo] = defaultSerializer.Deserialize(reader, memberInfo.DataType);
         }
      }

      protected virtual void DeserializeElementMember(XmlReader reader,
                                                      TypeInfo info,
                                                      Dictionary<TypeMemberInfo, object> memberValues)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         if (info == null) throw new ArgumentNullException("info");
         if (memberValues == null) throw new ArgumentNullException("memberValues");
         if (!info.MemberInfoByEntityName.ContainsKey(reader.Name))
            return;
         if (info.MemberInfoByEntityName[reader.Name].IsAttribute)
            return;
         var memberInfo = info.MemberInfoByEntityName[reader.Name];
         RefManager.SetWorkingMember(memberInfo);
         int id = 0;
         bool isReferenceOrStruct = TypeHelper.IsClassOrStruct(memberInfo.DataType);
         if (isReferenceOrStruct)
         {
            var type = TypeHelper.GetReferenceType(reader);
            if (type == null)
               type = memberInfo.DataType;
            else if (!memberInfo.DataType.IsAssignableFrom(type))
               throw new SerializerException(
                  string.Format("Type attribute of element {0} is not compatible with the object's actual member type of {1}.",
                                reader.Name,
                                memberInfo.DataType));

            id = TypeHelper.GetReferenceId(reader);
            if (id != 0)
            {
               if (RefManager.IsRegistered(id) && (RefManager.GetObject(id) != null))
               {
                  object instance = RefManager.GetObject(id);
                  if (memberInfo.DataType != instance.GetType())
                     throw new SerializerException("Instance type for id \"{0}\" does not match member type");
                  memberValues[memberInfo] = instance;
                  return;
               }
               if (!RefManager.IsRegistered(id))
                  RefManager.RegisterId(id);

               if (!TypeHelper.IsReferenceSource(reader))
               {
                  memberValues[memberInfo] = null;
                  RefManager.CaptureLateBinding(id, memberInfo);
                  return;
               }
            }
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
            var defaultSerializer = Factory.GetDefaultSerializer();
            result = defaultSerializer.Deserialize(reader, memberInfo.DataType);
            memberValues[memberInfo] = result;
         }
         if (id != 0)
            RefManager.UpdateObject(id, result);
      }

      protected virtual bool ReadToTextNode(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         while (reader.NodeType != XmlNodeType.Text)
            if (!reader.Read())
               return false;
         return true;
      }

      protected virtual bool ReadToElementEndNode(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         while (reader.NodeType != XmlNodeType.EndElement)
            if (!reader.Read())
               return false;
         return true;
      }

      protected virtual object GetEntityValue(object entity, TypeMemberInfo memberInfo)
      {
         entity = entity.WrapIfValueType();
         switch (memberInfo.Type)
         {
            case TypeMemberInfo.MemberType.Field:
               return entity.GetFieldValue(memberInfo.Name);
            case TypeMemberInfo.MemberType.Property:
               return entity.GetPropertyValue(memberInfo.Name);
            default:
               throw new SerializerException("Cannot serialize unknown member type");
         }
      }

      public virtual string FormatPrimitive(Type type, object obj, CultureInfo culture)
      {
         // if we encounter null here it is because it is a string
         //return obj == null ? string.Empty : obj.ToString();

         if (type == null) throw new ArgumentNullException("type");
         if (obj == null)
            return string.Empty;
         if (culture == null)
            culture = CultureInfo.InvariantCulture;
         string result = null;

         if (type == typeof(Int16))
            result = ((Int16)obj).ToString(culture);
         if (type == typeof(Int32))
            result = ((Int32)obj).ToString(culture);
         if (type == typeof(Int64))
            result = ((Int64)obj).ToString(culture);
         if (type == typeof(String))
            result = ((String)obj).ToString(culture);
         if (type == typeof(Char))
            result = ((Char)obj).ToString(culture);
         if (type == typeof(Byte))
            result = ((Byte)obj).ToString(culture);
         if (type == typeof(Single))
            result = ((Single)obj).ToString(culture);
         if (type == typeof(Double))
            result = ((Double)obj).ToString(culture);
         if (type == typeof(Decimal))
            result = ((Decimal)obj).ToString(culture);
         if (type == typeof(Boolean))
            result = ((Boolean)obj).ToString(culture);
         if (type == typeof(DateTime))
            result = ((DateTime)obj).ToString(culture);

         return result;
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

      protected virtual void SerializeArray(XmlWriter writer, object obj)
      {
         IEnumerable objArray = obj as IEnumerable;
         Type arrayElementType = obj.GetType().GetElementType();
         if (objArray == null)
            throw new SerializerException("Attempt to serialize non-array as an array");

         foreach (object item in objArray)
         {
            writer.WriteStartElement("item");
            Type type;
            type = item == null ? arrayElementType : item.GetType();
            // now we resolve a type serializer to do the work
            ITypeSerializer typeSerializer =
               Factory.CallMethod(new[] { arrayElementType }, "GetTypeSerializer") as ITypeSerializer;
            if (typeSerializer != null)
               typeSerializer.Serialize(writer, arrayElementType, item);
            else
            // Since there was no bound custom type serializer we default to the GenericTypeSerializer
            {
               var defaultSerializer = Factory.GetDefaultSerializer();
               defaultSerializer.Serialize(writer, arrayElementType, item);
            }
            writer.WriteEndElement();
         }
      }

      protected virtual object DeserializeArray(Type type, XmlReader reader)
      {
         if (type == null) throw new ArgumentNullException("type");
         if (reader == null) throw new ArgumentNullException("reader");
         if (reader.NodeType != XmlNodeType.Element)
            throw new SerializerException("Cannot deserialize an array from a non-Element node");

         if (TypeHelper.ReferenceIsNull(reader))
            return null;
         List<object> items = new List<object>();
         int counter = 0;
         // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
         while (ReadNextElement(reader))
         {
            items.Add(DeserializeArrayItem(type, reader, counter));
            counter++;
         }
         ReadToElementEndNode(reader);
         Array result = Activator.CreateInstance(type, items.Count) as Array;
         if (result == null)
            throw new SerializerException(string.Format("Unable to create new instance of \"{0}\"", type.Name()));
         for (int i = 0; i < items.Count; i++)
            result.SetValue(items[i], i);
         return result;
      }

      protected virtual object DeserializeArrayItem(Type arrayType, XmlReader reader, int arrayIndex)
      {
         Type arrayElementType = arrayType.GetElementType();
         Type type;
         int id = 0;
         bool isReferenceOrStruct = TypeHelper.IsClassOrStruct(arrayElementType);
         if (isReferenceOrStruct)
         {
            type = TypeHelper.GetReferenceType(reader);
            if (type == null)
               type = arrayElementType;
            else if (!arrayElementType.IsAssignableFrom(type))
               throw new SerializerException(string.Format("Cannot deserialize an instance of \"{0}\" into an array of \"{1}\"",
                                                           type,
                                                           arrayElementType));
            id = TypeHelper.GetReferenceId(reader);
            if (id != 0)
            {
               if (RefManager.IsRegistered(id) && (RefManager.GetObject(id) != null))
               {
                  object instance = RefManager.GetObject(id);
                  //if (memberInfo.DataType != instance.GetType())
                  //   throw new SerializationException("Instance type for id \"{0}\" does not match member type");
                  return instance;
               }
               if (!RefManager.IsRegistered(id))
                  RefManager.RegisterId(id);

               if (!TypeHelper.IsReferenceSource(reader))
               {
                  RefManager.CaptureLateBinding(id, arrayIndex);
                  return null;
               }
            }
         }
         else
            type = arrayElementType;

         object result = null;
         ITypeSerializer typeSerializer =
            Factory.CallMethod(new[] { type }, "GetTypeSerializer") as ITypeSerializer;
         if (typeSerializer != null)
            result = typeSerializer.Deserialize(reader, type);
         else
         // Since there was no bound custom type serializer we default to the GenericTypeSerializer
         {
            var defaultSerializer = Factory.GetDefaultSerializer();
            result = defaultSerializer.Deserialize(reader, type);
         }
         return result;
      }

      protected virtual object DeserializeEnum(Type type, XmlReader reader)
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

      protected virtual object DeserializePrimitive(Type type, XmlReader reader)
      {
         if (type == null) throw new ArgumentNullException("type");
         if (reader == null) throw new ArgumentNullException("reader");
         object result = null;
         bool isElement = (reader.NodeType == XmlNodeType.Element);
         if (isElement)
            ReadToTextNode(reader);
         if (reader.IsEmptyElement)
            result = type.GetDefaultValue();
         else
         {
            string primitiveValue = reader.ReadContentAsString();
            result = ParsePrimitive(type, primitiveValue);
         }
         if (isElement)
            ReadToElementEndNode(reader);
         return result;
      }

      protected virtual object ParsePrimitive(Type type, string primitiveValue)
      {
         object result = null;
         if (type == typeof(Int16))
            result = Int16.Parse(primitiveValue, Settings.Culture);
         else if (type == typeof(Int32))
            result = Int32.Parse(primitiveValue, Settings.Culture);
         else if (type == typeof(Int64))
            result = Int64.Parse(primitiveValue, Settings.Culture);
         else if (type == typeof(string))
            result = primitiveValue;
         else if (type == typeof(Char))
            result = Convert.ToChar(primitiveValue, Settings.Culture);
         else if (type == typeof(Byte))
            result = Byte.Parse(primitiveValue, Settings.Culture);
         else if (type == typeof(Single))
            result = Single.Parse(primitiveValue, Settings.Culture);
         else if (type == typeof(Double))
            result = Double.Parse(primitiveValue, Settings.Culture);
         else if (type == typeof(Decimal))
            result = Decimal.Parse(primitiveValue, Settings.Culture);
         else if (type == typeof(Boolean))
            result = Convert.ToBoolean(primitiveValue, Settings.Culture);
         else if (type == typeof(DateTime))
            result = DateTime.Parse(primitiveValue, Settings.Culture);
         else
            throw new SerializerException(string.Format("Cannot parse unexpected primitive type {0}", type.Name()));
         return result;
      }
      protected virtual string FormatType(string assemblyQualifiedName)
      {
         string[] parts = assemblyQualifiedName.Split(',');
         StringBuilder result = new StringBuilder();
         result.AppendFormat("{0}, {1}", parts[0], parts[1]);
         if (Settings.IncludeAssemblyVersionWithType)
            result.AppendFormat(", {0}", parts[2]);
         if (Settings.IncludeAssemblyCultureWithType)
            result.AppendFormat(", {0}", parts[3]);
         if (Settings.IncludeAssemblyKeyWithType)
            result.AppendFormat(", {0}", parts[4]);
         return result.ToString();
      }
   }
}