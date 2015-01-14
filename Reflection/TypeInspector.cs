using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public class TypeInspector
   {
      public TypeSerializationInfo GetInfo(string fullyQualifiedTypeName)
      {
         return GetInfo(Type.GetType(fullyQualifiedTypeName, true));
      }

      public TypeSerializationInfo GetInfo(Type type)
      {
         string rootName = null;
         string @namespace = null;
         List<TypeMemberSerializationInfo> infoList = new List<TypeMemberSerializationInfo>();
         var rootAttrib = type.Attribute<XmlRootAttribute>();
         if (rootAttrib != null)
         {
            rootName = rootAttrib.GetPropertyValue("name").ToString();
            @namespace = rootAttrib.GetPropertyValue("namespace").ToString();
         }
         var fieldInfo = type.Fields(Flags.InstanceAnyVisibility | Flags.ExcludeExplicitlyImplemented);
         foreach (var field in fieldInfo)
         {
            var ignoreAttrib = field.Attribute<XmlIgnoreAttribute>();
            if (ignoreAttrib == null)
            {
               var attributeAttrib = field.Attribute<XmlAttributeAttribute>();
               var elementAttrib = field.Attribute<XmlElementAttribute>();
               string entityName = elementAttrib != null ? elementAttrib.GetPropertyValue("name").ToString() : null;
               if (string.IsNullOrEmpty(entityName))
                  entityName= field.Name;
               var encapsulatingPropName = EncapsulatingPropertyName(field);
               if (!string.IsNullOrEmpty(encapsulatingPropName))
               {
                  var property = type.Property(encapsulatingPropName);
                  attributeAttrib = property.Attribute<XmlAttributeAttribute>();
                  elementAttrib = property.Attribute<XmlElementAttribute>();
                  entityName = elementAttrib != null ? elementAttrib.GetPropertyValue("name").ToString() : null;
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
         return new TypeSerializationInfo(type.Name, type, rootName, @namespace, infoList);
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