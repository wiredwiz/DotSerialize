using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Attributes;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection.Caching;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public class TypeInspector : ITypeInspector
   {
      private readonly ISerializationInfoCache _Cache;

      /// <summary>
      /// Initializes a new instance of the <see cref="TypeInspector"/> class.
      /// </summary>
      public TypeInspector()
      {
         _Cache = new WeakSerializationInfoCache();
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="TypeInspector"/> class.
      /// </summary>
      /// <param name="cache"></param>
      internal TypeInspector(ISerializationInfoCache cache)
      {
         _Cache = cache;
      }

      public TypeSerializationInfo GetInfo(string fullyQualifiedTypeName)
      {
         return GetInfo(Type.GetType(fullyQualifiedTypeName, true));
      }

      public TypeSerializationInfo GetInfo(Type type)
      {
         TypeSerializationInfo result = _Cache.GetInfo(type);
         if (result != null)
            return result;
         string rootName = null;
         string @namespace = null;
         List<TypeMemberSerializationInfo> infoList = null;
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
            throw new TypeLayoutException(string.Format("Attribute node name \"{0}\" is used for more than one member of Type {1}",
                                                        duplicateAttribs.First(),
                                                        type.Name()));
         result = new TypeSerializationInfo(type.Name, type, rootName, @namespace, infoList);
         _Cache.AddInfo(result);
         return result;
      }

      private List<TypeMemberSerializationInfo> GetFieldMembersInfo(Type type, IList<string> propertyExclusionList)
      {
         var fieldInfo = type.Fields(Flags.InstanceAnyVisibility | Flags.ExcludeHiddenMembers);
         List<TypeMemberSerializationInfo> infoList = new List<TypeMemberSerializationInfo>(fieldInfo.Count);
         foreach (var field in fieldInfo)
         {
            var ignoreAttrib = field.Attribute<XmlIgnoreAttribute>();
            if (ignoreAttrib == null)
            {
               var attributeAttrib = field.Attribute<XmlAttributeAttribute>();
               var elementAttrib = field.Attribute<XmlElementAttribute>();
               string entityName = elementAttrib != null ? elementAttrib.GetPropertyValue("Name") as String : null;
               if (string.IsNullOrEmpty(entityName))
                  entityName = field.Name;
               var encapsulatingPropName = EncapsulatingPropertyName(field);
               if (!string.IsNullOrEmpty(encapsulatingPropName))
               {
                  propertyExclusionList.Add(encapsulatingPropName);
                  var property = type.Property(encapsulatingPropName);
                  ignoreAttrib = property.Attribute<XmlIgnoreAttribute>();
                  if (ignoreAttrib != null)
                     continue;
                  attributeAttrib = property.Attribute<XmlAttributeAttribute>();
                  elementAttrib = property.Attribute<XmlElementAttribute>();
                  entityName = elementAttrib != null ? elementAttrib.GetPropertyValue("Name") as String : null;
                  if (string.IsNullOrEmpty(entityName))
                     entityName = encapsulatingPropName;
               }
               var memberInfo = new TypeMemberSerializationInfo(field.Name,
                                                                TypeMemberSerializationInfo.MemberType.Field,
                                                                entityName,
                                                                field.FieldType,
                                                                (attributeAttrib != null));
               infoList.Add(memberInfo);
            }
         }
         return infoList;
      }

      private List<TypeMemberSerializationInfo> GetPropertyMembersInfo(Type type, IList<string> propertyExclusionList)
      {
         var propInfo = type.Properties(Flags.InstanceAnyVisibility | Flags.ExcludeHiddenMembers);
         List<TypeMemberSerializationInfo> infoList = new List<TypeMemberSerializationInfo>(propInfo.Count);
         foreach (var prop in propInfo)
         {
            var ignoreAttrib = prop.Attribute<XmlIgnoreAttribute>();
            var elementAttrib = prop.Attribute<XmlElementAttribute>();
            if ((ignoreAttrib == null) && !propertyExclusionList.Contains(prop.Name) && (elementAttrib != null))
            {
               var attributeAttrib = prop.Attribute<XmlAttributeAttribute>();
               string entityName = elementAttrib.GetPropertyValue("Name") as String;
               if (string.IsNullOrEmpty(entityName))
                  entityName = prop.Name;
               var memberInfo = new TypeMemberSerializationInfo(prop.Name,
                                                                TypeMemberSerializationInfo.MemberType.Property,
                                                                entityName,
                                                                prop.PropertyType,
                                                                (attributeAttrib != null));
               infoList.Add(memberInfo);
            }
         }
         return infoList;
      }

      private string CleanNodeName(string name)
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

      private string EncapsulatingPropertyName(FieldInfo info)
      {
         var result = Regex.Match(info.Name, "<(.+)>k__BackingField", RegexOptions.Compiled);
         if (!result.Success)
            return null;
         return result.Groups[1].Value;
      }
   }
}