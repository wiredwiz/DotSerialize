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
using System.Linq.Expressions;
using System.Xml;
using Org.Edgerunner.DotSerialize.Properties;

namespace Org.Edgerunner.DotSerialize.Utilities
{
   public static class TypeHelper
   {
      public static readonly Type ByteType = typeof(Byte);
      public static readonly Type Int16Type = typeof(Int16);
      public static readonly Type Int32Type = typeof(Int32);
      public static readonly Type Int64Type = typeof(Int64);
      public static readonly Type UInt16Type = typeof(UInt16);
      public static readonly Type UInt32Type = typeof(UInt32);
      public static readonly Type UInt64Type = typeof(UInt64);
      public static readonly Type SingleType = typeof(Single);
      public static readonly Type DoubleType = typeof(Double);
      public static readonly Type DecimalType = typeof(Decimal);
      public static readonly Type CharType = typeof(Char);
      public static readonly Type StringType = typeof(String);
      public static readonly Type BooleanType = typeof(Boolean);
      public static readonly Type DateTimeType = typeof(DateTime);
      
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
         if (reader == null) throw new ArgumentNullException("reader");
         try
         {
            return Type.GetType(reader.GetAttribute(Resources.ReferenceType, Resources.DotserializeUri), true);
         }
         catch (ArgumentNullException ex)
         {
            return null;
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
         return type == StringType || type == Int32Type || type == BooleanType  ||
                type == Int64Type || type == DateTimeType || type == SingleType || type == DoubleType ||
                type == DecimalType || type == ByteType || type == Int16Type || type == CharType ||
                type == UInt16Type || type == UInt32Type || type == UInt64Type;
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
         return (!type.IsEnum && !type.IsArray && !IsPrimitive(type) && type.IsValueType);
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