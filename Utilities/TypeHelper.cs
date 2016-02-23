#region Apapche License 2.0

// <copyright file="TypeHelper.cs" company="Edgerunner.org">
// Copyright 2015 Thaddeus Ryker
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
#endregion

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Xml;
using Org.Edgerunner.DotSerialize.Properties;

namespace Org.Edgerunner.DotSerialize.Utilities
{
   public static class TypeHelper
   {
      public static readonly Type ByteType = typeof(byte);
      public static readonly Type SByteType = typeof(sbyte);
      public static readonly Type Int16Type = typeof(short);
      public static readonly Type Int32Type = typeof(int);
      public static readonly Type Int64Type = typeof(long);
      public static readonly Type UInt16Type = typeof(ushort);
      public static readonly Type UInt32Type = typeof(uint);
      public static readonly Type UInt64Type = typeof(ulong);
      public static readonly Type SingleType = typeof(float);
      public static readonly Type DoubleType = typeof(double);
      public static readonly Type DecimalType = typeof(decimal);
      public static readonly Type CharType = typeof(char);
      public static readonly Type StringType = typeof(string);
      public static readonly Type BooleanType = typeof(bool);
      public static readonly Type DateTimeType = typeof(DateTime);
      private static readonly Dictionary<string, Type> _TypeNameCache = new Dictionary<string, Type>();

      /// <summary>
      ///    Gets the member expression.
      /// </summary>
      /// <typeparam name="TModel">The type of the model.</typeparam>
      /// <typeparam name="T"></typeparam>
      /// <param name="expression">The expression.</param>
      /// <returns></returns>
      internal static MemberExpression GetMemberExpression<TModel, T>(Expression<Func<TModel, T>> expression)
      {
         // This method was taken from CsvHelper which was taken from FluentNHibernate.Utils.ReflectionHelper.cs and modified.
         // http://joshclose.github.io/CsvHelper/
         // http://fluentnhibernate.org/
         MemberExpression memberExpression = null;
         if (expression.Body.NodeType == ExpressionType.Convert)
         {
            var body = (UnaryExpression)expression.Body;
            memberExpression = body.Operand as MemberExpression;
         }
         else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            memberExpression = expression.Body as MemberExpression;

         if (memberExpression == null)
            throw new ArgumentException("Not a member access", "expression");

         return memberExpression;
      }

      public static int GetReferenceId(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         try
         {
            return int.Parse(reader.GetAttribute(Resources.ReferenceId, Resources.DotserializeUri));
         }
         catch (ArgumentNullException)
         {
            return 0;
         }
      }

      public static Type GetReferenceType(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException(nameof(reader));
         try
         {
            var typeName = reader.GetAttribute(Resources.ReferenceType, Resources.DotserializeUri);
            if (string.IsNullOrEmpty(typeName))
               return null;
            Type value;
            if (_TypeNameCache.TryGetValue(typeName, out value))
               return value;
            var type = Type.GetType(typeName, true);
            _TypeNameCache[typeName] = type;
            return type;
         }
         catch (ArgumentNullException ex)
         {
            return null;
         }
      }

      public static int[] GetArrayDimensions(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException(nameof(reader));
         try
         {
            var textDimensions = reader.GetAttribute(Resources.Dimensions, Resources.DotserializeUri).Split(',');
            var dimensions = new int[textDimensions.Length];
            for (var i = 0; i < dimensions.Length; i++)
               dimensions[i] = int.Parse(textDimensions[i]);
            return dimensions;
         }
         catch (ArgumentNullException)
         {
            return null;
         }
      }

      public static int[] GetArrayItemIndex(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException(nameof(reader));
         try
         {
            var attribute = reader.GetAttribute(Resources.ItemIndex, Resources.DotserializeUri);
            if (string.IsNullOrEmpty(attribute))
               throw new SerializationException(Resources.InvalidArrayItemIndex);

            var indicesText = attribute.Split(',');

            var indices = new int[indicesText.Length];
            for (var i = 0; i < indices.Length; i++)
               indices[i] = int.Parse(indicesText[i]);
            return indices;
         }
         catch (ArgumentNullException ex)
         {
            throw new SerializationException(Resources.InvalidArrayItemIndex, ex);
         }
         catch (OverflowException ex)
         {
            throw new SerializationException(Resources.InvalidArrayItemIndex, ex);
         }
      }

      public static bool IsArray(Type type)
      {
         return type.IsArray;
      }

      public static bool IsClassOrStruct(Type type)
      {
         return !IsPrimitive(type) && (IsStruct(type) || !type.IsValueType);
      }

      public static bool IsEnum(Type type)
      {
         return type.IsEnum;
      }

      public static bool IsPrimitive(Type type)
      {
         return type == StringType || type == Int32Type || type == BooleanType ||
                type == Int64Type || type == DateTimeType || type == SingleType || type == DoubleType ||
                type == DecimalType || type == ByteType || type == Int16Type || type == CharType ||
                type == UInt16Type || type == UInt32Type || type == UInt64Type || type == SByteType;
      }

      public static bool IsReferenceSource(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         try
         {
            return bool.Parse(reader.GetAttribute(Resources.ReferenceSource, Resources.DotserializeUri));
         }
         catch (ArgumentNullException)
         {
            return false;
         }
      }

      public static bool IsStruct(Type type)
      {
         return !type.IsEnum && !type.IsArray && !IsPrimitive(type) && type.IsValueType;
      }

      public static bool ReferenceIsNull(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         try
         {
            return bool.Parse(reader.GetAttribute(Resources.ReferenceisNull, Resources.XsiUri));
         }
         catch (ArgumentNullException)
         {
            return false;
         }
      }
   }
}