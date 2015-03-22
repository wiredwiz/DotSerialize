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
using System.Linq.Expressions;
using System.Xml;
using Org.Edgerunner.DotSerialize.Properties;

namespace Org.Edgerunner.DotSerialize.Utilities
{
   public static class TypeHelper
   {
      private static readonly Type _ByteType = typeof(Byte);
      private static readonly Type _Int16Type = typeof(Int16);
      private static readonly Type _Int32Type = typeof(Int32);
      private static readonly Type _Int64Type = typeof(Int64);
      private static readonly Type _SingleType = typeof(Single);
      private static readonly Type _DoubleType = typeof(Double);
      private static readonly Type _DecimalType = typeof(Decimal);
      private static readonly Type _CharType = typeof(Char);
      private static readonly Type _StringType = typeof(String);
      private static readonly Type _BooleanType = typeof(Boolean);
      private static readonly Type _DateTimeType = typeof(DateTime);

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
         return type == _StringType || type == _Int32Type || type == _BooleanType  ||
                type == _Int64Type || type == _DateTimeType || type == _SingleType || type == _DoubleType ||
                type == _DecimalType || type == _ByteType || type == _Int16Type || type == _CharType;
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