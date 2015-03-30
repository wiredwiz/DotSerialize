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
      public CultureInfo Culture { get; internal set; }
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

         public string Name { get; protected set; }
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
            IsArray = false;
            IsGeneric = false;
            SubTypes = new List<TypeInfo>();
         }
         /// <summary>
         /// Initializes a new instance of the <see cref="TypeInfo"/> class.
         /// </summary>
         /// <param name="name"></param>
         /// <param name="isArray"></param>
         internal TypeInfo(string name, bool isArray)
         {
            IsArray = isArray;
            Name = name;
            IsGeneric = false;
            SubTypes = new List<TypeInfo>();
         }
         /// <summary>
         /// Initializes a new instance of the <see cref="TypeInfo"/> class.
         /// </summary>
         /// <param name="name"></param>
         /// <param name="isArray"></param>
         /// <param name="isGeneric"></param>
         /// <param name="subTypes"></param>
         internal TypeInfo(string name, bool isArray, bool isGeneric, List<TypeInfo> subTypes)
         {
            IsArray = isArray;
            IsGeneric = isGeneric;
            Name = name;
            SubTypes = subTypes;
         }
         public bool IsArray { get; protected set; }
         public bool IsGeneric { get; protected set; }
         public string Name { get; protected set; }
         public List<TypeInfo> SubTypes { get; protected set; }
      }

      #endregion
   }
}