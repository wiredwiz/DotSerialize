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
using System.Runtime.Serialization;
using System.Text;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Attributes;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Mapping;
using Org.Edgerunner.DotSerialize.Reflection.Caching;
using Org.Edgerunner.DotSerialize.Utilities;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public class TypeInspector : ITypeInspector
   {
      protected readonly ISerializationInfoCache _Cache;
      protected readonly Settings _Settings;

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeInspector" /> class.
      /// </summary>
      public TypeInspector()
      {
         _Cache = new WeakSerializationInfoCache();
         _Settings = Settings.Default;
         WhiteListMode = false;
         PropertiesToExclude = new List<string>();
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeInspector" /> class.
      /// </summary>
      /// <param name="cache"></param>
      /// <param name="settings"></param>
      public TypeInspector(ISerializationInfoCache cache, Settings settings)
      {
         _Cache = cache;
         _Settings = settings;
         WhiteListMode = false;
         PropertiesToExclude = new List<string>();
      }

      protected bool WhiteListMode { get; set; }
      public List<string> PropertiesToExclude { get; set; }

      #region ITypeInspector Members

      public virtual TypeInfo GetInfo(string fullyQualifiedTypeName)
      {
         return GetInfo(Type.GetType(fullyQualifiedTypeName, true));
      }

      public virtual TypeInfo GetInfo(Type type)
      {
         WhiteListMode = false;
         PropertiesToExclude.Clear();
         TypeInfo result = _Cache.GetInfo(type);
         if (result != null)
            return result;
         string rootName = null;
         string @namespace = null;
         List<TypeMemberInfo> infoList = null;
         var rootAttrib = type.Attribute<XmlRootAttribute>();
         if (rootAttrib != null)
         {
            rootName = rootAttrib.GetPropertyValue("Name").ToString();
            @namespace = rootAttrib.GetPropertyValue("Namespace").ToString();
         }
         else
            rootName = CleanNodeName(type.Name());
         var dcAttrib = type.Attribute<DataContractAttribute>();
         if (dcAttrib != null)
         {
            WhiteListMode = true;
            rootName = !string.IsNullOrEmpty(dcAttrib.Name) ? dcAttrib.GetPropertyValue("Name").ToString() : rootName;
            @namespace = !string.IsNullOrEmpty(dcAttrib.Namespace)
               ? dcAttrib.GetPropertyValue("Namespace").ToString()
               : @namespace;
         }
         infoList = GetFieldMembersInfo(type);
         infoList.AddRange(GetPropertyMembersInfo(type));
         var duplicateElements = from x in infoList
                                 where !x.IsAttribute
                                 group x by x.EntityName
                                    into grouped
                                    where (grouped.Count() > 1)
                                    select grouped.Key;
         if (duplicateElements.Count() != 0)
            throw new TypeLayoutException(string.Format("Element node name \"{0}\" is used for more than one member of Type {1}",
                                                        duplicateElements.First(),
                                                        type.Name()));
         var duplicateAttribs = from x in infoList
                                where x.IsAttribute
                                group x by x.EntityName
                                   into grouped
                                   where (grouped.Count() > 1)
                                   select grouped.Key;
         if (duplicateAttribs.Count() != 0)
            throw new TypeLayoutException(string.Format(
                                                        "Attribute node name \"{0}\" is used for more than one member of Type {1}",
                                                        duplicateAttribs.First(),
                                                        type.Name()));
         result = new TypeInfo(type.Name, type, rootName, @namespace, infoList);
         _Cache.AddInfo(result);
         return result;
      }

      /// <summary>
      /// Extracts a <see cref="TypeInfo"/> instance from an class map and adds it to the internal cache.
      /// </summary>
      /// <param name="map"><see cref="Org.Edgerunner.DotSerialize.Mapping.ClassMapBase" /> instance to register.</param>
      public void RegisterMap(ClassMapBase map)
      {
         _Cache.AddInfo(map.GetTypeInfo());
      }

      #endregion

      public static string CleanNodeName(string name)
      {
         StringBuilder builder = new StringBuilder(name.Length);
         foreach (char item in name)
            if ((item < 48) ||
                ((item > 57) && (item < 65)) ||
                ((item > 90) && (item < 97)) ||
                (item > 122))
               builder.Append('_');
            else
               builder.Append(item);
         return builder.ToString();
      }

      protected virtual List<TypeMemberInfo> GetFieldMembersInfo(Type type)
      {
         List<TypeMemberInfo> infoList = new List<TypeMemberInfo>();
         // Get information for fields
         var allFields = type.Fields(Flags.InstanceAnyVisibility);
         var results = from p in allFields
                       group p by p.Name
                          into g
                          select new { FieldName = g.Key, Value = g.ToList() };
         foreach (var item in results)
         {
            bool ignore;
            var fields = item.Value;
            var memberInfo = GetFieldMemberInfo(type, fields, out ignore);
            if (!ignore && (memberInfo != null))
               infoList.Add(memberInfo);
         }

         return infoList;
      }

      protected virtual List<TypeMemberInfo> GetPropertyMembersInfo(Type type)
      {
         List<TypeMemberInfo> infoList = new List<TypeMemberInfo>();
         var allProperties = type.Properties(Flags.InstanceAnyVisibility);
         var results = from p in allProperties
                       group p by p.Name
                          into g
                          select new { PropertyName = g.Key, Value = g.ToList() };
         foreach (var item in results)
         {
            bool ignore;
            if (PropertiesToExclude.Contains(item.PropertyName))
               continue;
            var properties = item.Value;
            var memberInfo = GetPropertyMemberInfo(type, properties, out ignore);
            if (!ignore && (memberInfo != null))
               infoList.Add(memberInfo);
         }

         return infoList;
      }

      protected virtual TypeMemberInfo GetFieldMemberInfo(Type type, List<FieldInfo> fields, out bool ignore)
      {
         var topLevelField = fields.First();
         int ordering = 999;
         ignore = false;
         TypeMemberInfo memberInfo = null;
         foreach (var field in fields)
            if (field.IsBackingField())
            {
               // get information from property
               var autoProperty = field.GetEncapsulatingAutoProperty();
               PropertiesToExclude.Add(autoProperty.Name);
               var allProperties = type.Properties(Flags.InstanceAnyVisibility, autoProperty.Name);
               memberInfo = GetPropertyMemberInfo(type, allProperties.ToList(), out ignore);
               if (memberInfo != null)
               {
                  memberInfo.Name = field.Name;
                  memberInfo.Type = TypeMemberInfo.MemberType.Field;
               }
               else if (!ignore)
                  memberInfo = new TypeMemberInfo(field.Name,
                                                  TypeMemberInfo.MemberType.Field,
                                                  autoProperty.Name,
                                                  field.FieldType,
                                                  false) { Order = ordering };
               break;
            }
            else
            {
               // If any of our AttributesToIgnore are found then set the ignore flag
               if (_Settings.AttributesToIgnore.Any(attribute => field.HasAttribute(attribute.GetType()))) ignore = true;
               var elementAttrib = field.Attribute<XmlElementAttribute>();
               var attribAttribute = field.Attribute<XmlAttributeAttribute>();
               var dataMemberAttribute = field.Attribute<DataMemberAttribute>();

               if (ignore && (elementAttrib == null) && (attribAttribute == null) && (dataMemberAttribute == null))
                  break; // skip the current field
               if ((elementAttrib != null) || (attribAttribute != null) || (dataMemberAttribute != null))
               {
                  ignore = false;
                  string entityName;
                  if (attribAttribute != null)
                     entityName = attribAttribute.GetPropertyValue("Name").ToString();
                  else if (elementAttrib != null)
                  {
                     entityName = elementAttrib.GetPropertyValue("Name").ToString();
                     ordering = (int)elementAttrib.GetPropertyValue("Order");
                  }
                  else
                  {
                     entityName = dataMemberAttribute.GetPropertyValue("Name").ToString();
                     ordering = (int)dataMemberAttribute.GetPropertyValue("Order");
                  }
                  entityName = string.IsNullOrEmpty(entityName) ? field.Name : entityName;
                  ordering = ordering == 0 ? 999 : ordering;
                  memberInfo = new TypeMemberInfo(field.Name,
                                                  TypeMemberInfo.MemberType.Field,
                                                  entityName,
                                                  field.FieldType,
                                                  (attribAttribute != null)) { Order = ordering };
                  break;
               }
            }
         if ((memberInfo == null) && !ignore && !WhiteListMode)
            memberInfo = new TypeMemberInfo(topLevelField.Name,
                                               TypeMemberInfo.MemberType.Field,
                                               topLevelField.Name,
                                               topLevelField.FieldType,
                                               false) { Order = ordering };
         return memberInfo;
      }

      protected virtual TypeMemberInfo GetPropertyMemberInfo(Type type, List<PropertyInfo> properties, out bool ignore)
      {
         if (type == null) throw new ArgumentNullException("type");
         if (properties == null) throw new ArgumentNullException("properties");

         ignore = false;
         if (properties.Count == 0)
            return null;

         int ordering = 999;
         TypeMemberInfo memberInfo = null;
         foreach (var property in properties)
         {
            // If any of our AttributesToIgnore are found then set the ignore flag
            ignore = _Settings.AttributesToIgnore.Any(attribute => property.HasAttribute(attribute.GetType()));            
            var elementAttrib = property.Attribute<XmlElementAttribute>();
            var attribAttribute = property.Attribute<XmlAttributeAttribute>();
            var dataMemberAttribute = property.Attribute<DataMemberAttribute>();

            // If we are set to explicitly ignore then we break out now
            if (ignore && (elementAttrib == null) && (attribAttribute == null) && (dataMemberAttribute == null))
               break; // skip the current field
            // If we found an element, attribute ot datamember attribute then we build type member info from
            if ((elementAttrib != null) || (attribAttribute != null) || (dataMemberAttribute != null))
            {
               ignore = false;
               string entityName;
               if (attribAttribute != null)
                  entityName = attribAttribute.GetPropertyValue("Name").ToString();
               else if (elementAttrib != null)
               {
                  entityName = elementAttrib.GetPropertyValue("Name").ToString();
                  ordering = (int)elementAttrib.GetPropertyValue("Order");
               }
               else
               {
                  entityName = dataMemberAttribute.GetPropertyValue("Name").ToString();
                  ordering = (int)dataMemberAttribute.GetPropertyValue("Order");
               }
               entityName = string.IsNullOrEmpty(entityName) ? property.Name : entityName;
               ordering = ordering == 0 ? 999 : ordering;
               memberInfo = new TypeMemberInfo(property.Name,
                                               TypeMemberInfo.MemberType.Property,
                                               entityName,
                                               property.PropertyType,
                                               (attribAttribute != null)) { Order = ordering };
               break;
            }
         }

         return memberInfo;
      }
   }
}