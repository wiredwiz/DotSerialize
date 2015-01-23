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
using System.Collections;
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
      protected IReferenceManager RefManager { get; set; }
      public ITypeInspector TypeInspector { get; set; }

      /// <summary>
      ///    Initializes a new instance of the <see cref="DefaultTypeSerializer" /> class.
      /// </summary>
      /// <param name="factory"></param>
      /// <param name="referenceManager"></param>
      /// <param name="typeInspector"></param>
      public DefaultTypeSerializer(ITypeSerializerFactory factory, IReferenceManager referenceManager, ITypeInspector typeInspector)
      {
         Factory = factory;
         RefManager = referenceManager;
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
               throw new SerializationException("Only primitives or enums can be stored in attributes.");

            if (TypeHelper.IsPrimitive(type))
               result = DeserializePrimitive(type, reader);
            else if (TypeHelper.IsEnum(type))
               result = DeserializeEnum(type, reader);
            else
               throw new SerializationException(string.Format("Unable to deserialize unexpected type \"{0}\" from an attribute.", type.Name()));
            return result;
         }
         // Handle Elements
         if (reader.NodeType == XmlNodeType.Element)
         {
            if (TypeHelper.IsPrimitive(type))
               result = DeserializePrimitive(type, reader);
            else if (TypeHelper.IsArray(type))
               result = DeserializeArray(type, reader);
            else if (TypeHelper.IsEnum(type))
               result = DeserializeEnum(type, reader);
            else
            {
               // Class or struct
               if (TypeHelper.ReferenceIsNull(reader))
                  return null;

               var memberValues = new Dictionary<TypeMemberInfo, object>();
               RefManager.StartLateBindingCapture(type);
               var info = TypeInspector.GetInfo(type);

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

               result = TypeFactory.CreateInstance(type, memberValues);
               if (result == null)
                  throw new SerializationException(string.Format("Unable to create an instance of type \"{0}\".", type.Name()));

               RefManager.FinishCaptures(result);
            }

            // hand back our new object          
            return result;
         }
         throw new SerializationException(
            string.Format("Reader was not positioned on a node of type Attribute or Element.\r\n" +
                          "A custom type serializer probably positioned the reader incorrectly." +
                          "Unable to deserialize type \"{0}\".",
                          type.Name()));
      }

      protected virtual void DeserializeAttribMember(XmlReader reader,
                                                  Reflection.TypeInfo info,
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

      protected virtual void DeserializeElementMember(XmlReader reader, Reflection.TypeInfo info, Dictionary<TypeMemberInfo, object> memberValues)
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
            if (memberInfo.DataType != type)
               throw new SerializationException(string.Format("Type attribute of element {0} does not match the object's actual member type of {1}.",
                                                              reader.Name,
                                                              memberInfo.DataType));

            id = TypeHelper.GetReferenceId(reader);
            if (id != 0)
            {
               if (RefManager.IsRegistered(id) && (RefManager.GetObject(id) != null))
               {
                  object instance = RefManager.GetObject(id);
                  if (memberInfo.DataType != instance.GetType())
                     throw new SerializationException("Instance type for id \"{0}\" does not match member type");
                  memberValues[memberInfo] = instance;
                  return;
               }
               else
               {
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
         else
         {
            writer.WriteAttributeString(Properties.Resources.ReferenceType, Properties.Resources.DotserializeUri, FormatType(type.AssemblyQualifiedName));
            // check for null value
            if (obj == null)
            {
               writer.WriteAttributeString(Properties.Resources.ReferenceisNull, Properties.Resources.XsiUri, true.ToString().ToLowerInvariant());
               return;
            }
            // check for struct before writing reference id
            if (!type.IsValueType)
            {
               int id;
               if (RefManager.IsRegistered(obj))
               {
                  id = RefManager.GetObjectId(obj);
                  writer.WriteAttributeString(Properties.Resources.ReferenceId, Properties.Resources.DotserializeUri, id.ToString());
                  // since this object has already been serialized once there is no need to write out the rest of the object
                  return;
               }
               // Given that the instance has not already been seen we keep writing
               id = RefManager.RegisterId(obj);
               writer.WriteAttributeString(Properties.Resources.ReferenceId, Properties.Resources.DotserializeUri, id.ToString());
               writer.WriteAttributeString(Properties.Resources.ReferenceSource, Properties.Resources.DotserializeUri, true.ToString().ToLowerInvariant());
            }

            if (TypeHelper.IsArray(type))
               SerializeArray(writer, obj);
            else
            {
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
                     var defaultSerializer = Factory.GetDefaultSerializer();
                     defaultSerializer.Serialize(writer, attrib.DataType, entityValue);
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
                     var defaultSerializer = Factory.GetDefaultSerializer();
                     defaultSerializer.Serialize(writer, element.DataType, entityValue);
                  }
                  writer.WriteEndElement();
               }
            }
         }
      }

      public object GetEntityValue(object entity, TypeMemberInfo memberInfo)
      {
         switch (memberInfo.Type)
         {
            case TypeMemberInfo.MemberType.Field:
               return entity.GetFieldValue(memberInfo.Name);
            case TypeMemberInfo.MemberType.Property:
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

      protected virtual void SerializeArray(XmlWriter writer, object obj)
      {
         IEnumerable objArray = obj as IEnumerable;
         Type arrayElementType = obj.GetType().GetElementType();
         if (objArray == null)
            throw new SerializationException("Attempt to serialize non-array as an array");

         foreach (object item in objArray)
         {
            writer.WriteStartElement("item");
            Type type;
            type = item == null ? arrayElementType : item.GetType();
            // now we resolve a type serializer to do the work
            ITypeSerializer typeSerializer =
            Factory.CallMethod(new[] { type }, "GetTypeSerializer") as ITypeSerializer;
            if (typeSerializer != null)
               typeSerializer.Serialize(writer, type, item);
            else
            // Since there was no bound custom type serializer we default to the GenericTypeSerializer
            {
               var defaultSerializer = Factory.GetDefaultSerializer();
               defaultSerializer.Serialize(writer, type, item);
            }
            writer.WriteEndElement();
         }
      }

      protected virtual object DeserializeArray(Type type, XmlReader reader)
      {
         if (type == null) throw new ArgumentNullException("type");
         if (reader == null) throw new ArgumentNullException("reader");
         if (reader.NodeType != XmlNodeType.Element)
            throw new SerializationException("Cannot deserialize an array from a non-Element node");

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
            throw new SerializationException(string.Format("Unable to create new instance of \"{0}\"", type.Name()));
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
            if (!arrayElementType.IsAssignableFrom(type))
               throw new SerializationException(string.Format(
                                                              "Cannot deserialize an instance of \"{0}\" into an array of \"{1}\"",
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
               else
               {
                  if (!RefManager.IsRegistered(id))
                     RefManager.RegisterId(id);

                  if (!TypeHelper.IsReferenceSource(reader))
                  {
                     RefManager.CaptureLateBinding(id, arrayIndex);
                     return null;
                  }
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
      public string FormatType(string assemblyQualifiedName)
      {
         string[] parts = assemblyQualifiedName.Split(',');
         return string.Format("{0}, {1}", parts[0], parts[1]);
      }
   }
}