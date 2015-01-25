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
using System.Text;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Attributes;
using Org.Edgerunner.DotSerialize.Exceptions;
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
      }

      #region ITypeInspector Members

      public virtual TypeInfo GetInfo(string fullyQualifiedTypeName)
      {
         return GetInfo(Type.GetType(fullyQualifiedTypeName, true));
      }

      public virtual TypeInfo GetInfo(Type type)
      {
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
         var propertyExclusionList = new List<string>();
         infoList = GetFieldMembersInfo(type, propertyExclusionList);
         infoList.AddRange(GetPropertyMembersInfo(type, propertyExclusionList));
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

      #endregion

      protected virtual string CleanNodeName(string name)
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

      protected virtual List<TypeMemberInfo> GetFieldMembersInfo(Type type, IList<string> propertyExclusionList)
      {
         var fieldInfo = type.Fields(Flags.InstanceAnyVisibility | Flags.ExcludeHiddenMembers);
         List<TypeMemberInfo> infoList = new List<TypeMemberInfo>(fieldInfo.Count);
         foreach (var field in fieldInfo)
         {
            var attribs = field.Attributes();
            Attribute elementAttrib = null;
            Attribute attributeAttrib = null;
            bool ignore = false;
            foreach (var attrib in attribs)
            {
               if (_Settings.AttributesToIgnore.Contains(attrib))
                  ignore = true;
               if (attrib.GetType() == typeof(XmlAttributeAttribute))
                  attributeAttrib = attrib;
               if (attrib.GetType() == typeof(XmlElementAttribute))
                  elementAttrib = attrib;
            }
            if (!ignore)
            {
               string entityName = null;
               int ordering = 999;
               if (attributeAttrib != null)
                  entityName = attributeAttrib.GetPropertyValue("Name").ToString();
               else if (elementAttrib != null)
               {
                  entityName = elementAttrib.GetPropertyValue("Name").ToString();
                  ordering = (int)elementAttrib.GetPropertyValue("Order");
               }
               if (string.IsNullOrEmpty(entityName))
                  entityName = field.Name;
               if (field.IsBackingField())
               {
                  var property = field.GetEncapsulatingAutoProperty();
                  propertyExclusionList.Add(property.Name);
                  attribs = property.Attributes();
                  foreach (var attrib in attribs)
                  {
                     if (_Settings.AttributesToIgnore.Contains(attrib))
                        ignore = true;
                     if (attrib.GetType() == typeof(XmlAttributeAttribute))
                        attributeAttrib = attrib;
                     if (attrib.GetType() == typeof(XmlElementAttribute))
                        elementAttrib = attrib;
                  }
                  if (ignore)
                     continue;
                  if (attributeAttrib != null)
                     entityName = attributeAttrib.GetPropertyValue("Name").ToString();
                  else
                     entityName = elementAttrib != null ? elementAttrib.GetPropertyValue("Name").ToString() : null;
                  if (string.IsNullOrEmpty(entityName))
                     entityName = property.Name;
                  if (elementAttrib != null) 
                     ordering = (int)elementAttrib.GetPropertyValue("Order");
               }
               var memberInfo = new TypeMemberInfo(field.Name,
                                                   TypeMemberInfo.MemberType.Field,
                                                   entityName,
                                                   field.FieldType,
                                                   (attributeAttrib != null)) { Order = ordering };
               infoList.Add(memberInfo);
            }
         }
         return infoList;
      }

      protected virtual List<TypeMemberInfo> GetPropertyMembersInfo(Type type, IList<string> propertyExclusionList)
      {
         var propInfo = type.Properties(Flags.InstanceAnyVisibility | Flags.ExcludeHiddenMembers);
         List<TypeMemberInfo> infoList = new List<TypeMemberInfo>(propInfo.Count);
         foreach (var prop in propInfo)
         {
            var ignore = false;
            Attribute elementAttrib = null;
            var attribs = prop.Attributes();
            foreach (var attrib in attribs)
            {
               if (_Settings.AttributesToIgnore.Contains(attrib))
                  ignore = true;
               if (attrib.GetType() == typeof(XmlElementAttribute))
                  elementAttrib = attrib;
            }
            if (!(ignore || propertyExclusionList.Contains(prop.Name) || (elementAttrib == null)))
            {
               int ordering = 999;
               if (prop.GetIndexParameters().Length != 0)
                  throw new TypeLayoutException(
                     "Indexed properties should not be serialized.  Instead the underlying value being indexed should be serialized.");
               var attributeAttrib = prop.Attribute<XmlAttributeAttribute>();
               string entityName = elementAttrib.GetPropertyValue("Name").ToString();
               ordering = (int)elementAttrib.GetPropertyValue("Order");
               if (string.IsNullOrEmpty(entityName))
                  entityName = prop.Name;
               var memberInfo = new TypeMemberInfo(prop.Name,
                                                   TypeMemberInfo.MemberType.Property,
                                                   entityName,
                                                   prop.PropertyType,
                                                   (attributeAttrib != null)) { Order = ordering };
               infoList.Add(memberInfo);
            }
         }
         return infoList;
      }
   }
}