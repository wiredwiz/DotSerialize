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
using System.Runtime.CompilerServices;
using Org.Edgerunner.DotSerialize.Reflection;

namespace Org.Edgerunner.DotSerialize.Mapping
{
   public class XmlNodeMap
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="XmlNodeMap" /> class.
      /// </summary>
      internal XmlNodeMap(string name, TypeMemberInfo.MemberType memberType, Type dataType)
      {
         Info = new TypeMemberInfo(name, memberType, dataType);
      }

      internal TypeMemberInfo Info { get; set; }

      /// <summary>
      ///    Specifies the name to use for the xml attribute or element node.
      /// </summary>
      /// <param name="name">Name to use.</param>
      /// <returns>Current node map instance.</returns>
      public XmlNodeMap UsingName(string name)
      {
         Info.EntityName = name;
         return this;
      }

      /// <summary>
      ///    Specifies the index that represents the order in which xml elements should be written out.
      /// </summary>
      /// <param name="index">Integer specifying the order.</param>
      /// <returns>Current node map instance.</returns>
      public XmlNodeMap OrderedAs(int index)
      {
         Info.Order = index;
         return this;
      }

      /// <summary>
      ///    Specifies whether the xml node should be an attribute
      /// </summary>
      /// <returns>Current node map instance.</returns>
      public XmlNodeMap AsAttribute()
      {
         Info.IsAttribute = true;
         return this;
      }
   }
}