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

namespace Org.Edgerunner.DotSerialize.Attributes
{
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
   public class XmlRootAttribute : Attribute
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="XmlRootAttribute" /> class.
      /// </summary>
      /// <param name="name"></param>
      public XmlRootAttribute(string name)
      {
         Name = name;
         Namespace = String.Empty;
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="XmlRootAttribute" /> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="@namespace"></param>
      public XmlRootAttribute(string name, string @namespace)
      {
         Name = name;
         Namespace = @namespace;
      }

      public string Name { get; set; }
      public string Namespace { get; set; }
   }
}