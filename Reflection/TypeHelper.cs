﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Fasterflect;
using System.Threading;
using Org.Edgerunner.DotSerialize.Exceptions;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public static class TypeHelper
   {
      public static bool IsPrimitive(Type type)
      {
         switch (type.FullName)
         {
            case "System.Byte":
            case "System.Int16":
            case "System.Int32":
            case "System.Int64":
            case "System.Single":
            case "System.Double":
            case "System.Decimal":
            case "System.Boolean":
            case "System.DateTime":
            case "System.Char":
            case "System.String":
               return true;
            default:
               return false;
         }
      }

      public static bool IsEnum(Type type)
      {
         return type.IsEnum;
      }

      public static bool IsArray(Type type)
      {
         return type.IsArray;
      }

      public static bool IsStruct(Type type)
      {
         return (!type.IsEnum && !type.IsArray && !IsPrimitive(type) && type.IsValueType);
      }

      public static bool IsClassOrStruct(Type type)
      {
         return !IsPrimitive(type) && (IsStruct(type) || !type.IsValueType);
      }

      public static Guid GetReferenceId(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         try
         {
            return Guid.Parse(reader.GetAttribute("reference__id"));
         }
         catch (ArgumentNullException)
         {
            return Guid.Empty;
         }
      }

      public static Type GetReferenceType(XmlReader reader)
      {
         const string typeAttributeName = "reference__type";
         if (reader == null) throw new ArgumentNullException("reader");
         try
         {
            return Type.GetType(reader.GetAttribute(typeAttributeName), true);
         }
         catch (ArgumentNullException ex)
         {
            throw new SerializationException(string.Format("Attribute \"{0}\" is missing", typeAttributeName), ex);
         }
      }

      public static bool ReferenceIsNull(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         try
         {
            return bool.Parse(reader.GetAttribute("reference__isNull"));
         }
         catch (ArgumentNullException)
         {
            return false;
         }
      }
   }
}