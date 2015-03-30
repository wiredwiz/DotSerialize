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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Org.Edgerunner.DotSerialize.Reflection.Types.Naming
{
   public class AssemblyQualifiedName
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="AssemblyQualifiedName"/> class.
      /// </summary>
      internal AssemblyQualifiedName()
      {
      }

      public TypeInfo Type { get; internal set; }
      public AssemblyInfo Assembly { get; internal set; }
      public Version Version { get; internal set; }
      public string Culture { get; internal set; }
      public string PublicKeyToken { get; internal set; }

      #region Nested type: AssemblyInfo

      public class AssemblyInfo
      {
         /// <summary>
         ///    Initializes a new instance of the <see cref="AssemblyInfo" /> class.
         /// </summary>
         /// <param name="name"></param>
         internal AssemblyInfo(string name)
         {
            Name = name;
         }

         public string Name { get; internal set; }

         /// <summary>
         /// Returns a string that represents the current object.
         /// </summary>
         /// <returns>
         /// A string that represents the current object.
         /// </returns>
         public override string ToString()
         {
            return Name;
         }
      }

      #endregion

      #region Nested type: TypeInfo

      public class TypeInfo
      {
         /// <summary>
         /// Initializes a new instance of the <see cref="TypeInfo"/> class.
         /// </summary>
         /// <param name="name"></param>
         internal TypeInfo(string name)
         {
            Name = name;
            ArrayDimensions = 0;
            IsPointer = false;
            IsGeneric = false;
            SubTypes = new List<AssemblyQualifiedName>();
         }
         /// <summary>
         /// Initializes a new instance of the <see cref="TypeInfo"/> class.
         /// </summary>
         /// <param name="name"></param>
         /// <param name="isPointer"></param>
         public TypeInfo(string name, bool isPointer)
         {
            IsPointer = isPointer;
            Name = name;
            ArrayDimensions = 0;
            IsGeneric = false;
            SubTypes = new List<AssemblyQualifiedName>();
         }
         /// <summary>
         /// Initializes a new instance of the <see cref="TypeInfo"/> class.
         /// </summary>
         /// <param name="name"></param>
         /// <param name="arrayDimensions"></param>
         internal TypeInfo(string name, int arrayDimensions)
         {
            ArrayDimensions = arrayDimensions;
            Name = name;
            IsPointer = false;
            IsGeneric = false;
            SubTypes = new List<AssemblyQualifiedName>();
         }
         /// <summary>
         /// Initializes a new instance of the <see cref="TypeInfo"/> class.
         /// </summary>
         /// <param name="name"></param>
         /// <param name="isPointer"></param>
         /// <param name="arrayDimensions"></param>
         public TypeInfo(string name, bool isPointer, int arrayDimensions)
         {
            ArrayDimensions = arrayDimensions;
            IsPointer = isPointer;
            Name = name;
            IsGeneric = false;
            SubTypes = new List<AssemblyQualifiedName>();
         }
         /// <summary>
         /// Initializes a new instance of the <see cref="TypeInfo"/> class.
         /// </summary>
         /// <param name="name"></param>
         /// <param name="isPointer"></param>
         /// <param name="arrayDimensions"></param>
         /// <param name="isGeneric"></param>
         /// <param name="subTypes"></param>
         internal TypeInfo(string name, bool isPointer, int arrayDimensions, bool isGeneric, List<AssemblyQualifiedName> subTypes)
         {
            ArrayDimensions = arrayDimensions;
            IsPointer = false;
            IsGeneric = isGeneric;
            Name = name;
            SubTypes = subTypes;
         }
         public int ArrayDimensions { get; internal set; }
         public bool IsGeneric { get; internal set; }
         public bool IsPointer { get; internal set; }
         public string Name { get; internal set; }
         public List<AssemblyQualifiedName> SubTypes { get; internal set; }

         /// <summary>
         /// Returns a string that represents the current object.
         /// </summary>
         /// <returns>
         /// A string that represents the current object.
         /// </returns>
         public override string ToString()
         {
            return ToString(string.Empty);
         }

         /// <summary>
         /// Returns a string that represents the current object.
         /// </summary>
         /// <param name="format">A format string.</param>
         /// <returns>
         /// A string that represents the current object.
         /// </returns>
         public string ToString(string format)
         {
            return Name + FormatGenerics(format) + (IsPointer ? "*" : string.Empty) + FormatArray();
         }

         protected string FormatArray()
         {
            if (ArrayDimensions == 0)
               return string.Empty;
            if (ArrayDimensions == 1)
               return "[]";
            return String.Format("[{0}]", new string(',', ArrayDimensions - 1));
         }

         protected string FormatGenerics(string format)
         {
            if (!IsGeneric)
               return string.Empty;
            var types = new string[SubTypes.Count];
            for (int i = 0; i < SubTypes.Count; i++)
               types[i] = String.Format("[{0}]", SubTypes[i].ToString(format));
            return String.Format("`{0}[{1}]", SubTypes.Count, string.Join(",", types));
         }
      }

      #endregion

      /// <summary>
      /// Returns a string that represents the current object.
      /// </summary>
      /// <returns>
      /// A string that represents the current object.
      /// </returns>
      public override string ToString()
      {
         return ToString(string.Empty);
      }

      /// <summary>
      /// Returns a string that represents the current object.
      /// </summary>
      /// <param name="format">A format string.</param>
      /// <returns>
      /// A string that represents the current object.
      /// </returns>
      public string ToString(string format)
      {
         if (string.IsNullOrEmpty(format) || (format == "U"))
            return String.Format("{0}, {1}, {2}, {3}, {4}", Type.ToString(format), Assembly.Name, FormatVersion(), FormatCulture(), FormatPublicKeyToken());

         var values = new List<string>(4);
         values.AddRange(format.Select(item => ConvertFormatChar(item)));
         return string.Join(", ", values);
      }

      protected string ConvertFormatChar(char format)
      {
         switch (format)
         {
            case 't':
               return Type.ToString(format.ToString());
            case 'T':
               return String.Format("{0}, {1}", Type.ToString(format.ToString()), Assembly.Name);
            case 'V':
               return FormatVersion();
            case 'C':
               return FormatCulture();
            case 'K':
               return FormatPublicKeyToken();
            default:
               throw new ArgumentException(string.Format("\"{0}\" is not a valid format character", format));
         }
      }

      protected string FormatVersion()
      {
         return "Version=" + Version;
      }

      protected string FormatCulture()
      {
         return "Culture=" + Culture;
      }

      protected string FormatPublicKeyToken()
      {
         return "PublicKeyToken=" + PublicKeyToken;
      }
   }
}