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

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public class TypeInfo
   {
      public string Name { get; set; }
      public Type DataType { get; set; }
      public string EntityName { get; set; }
      public string Namespace { get; set; }
      public IDictionary<string, TypeMemberInfo> MemberInfoByName { get; set; }
      public IDictionary<string, TypeMemberInfo> MemberInfoByEntityName { get; set; }

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeInfo" /> class.
      /// </summary>
      /// <param name="typeName"></param>
      /// <param name="dataType"></param>
      public TypeInfo(string typeName, Type dataType)
      {
         Name = typeName;
         EntityName = typeName;
         DataType = dataType;
         MemberInfoByName = new Dictionary<string, TypeMemberInfo>();
         MemberInfoByEntityName = new Dictionary<string, TypeMemberInfo>();
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeInfo" /> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="dataType"></param>
      /// <param name="entityName"></param>
      public TypeInfo(string name, Type dataType, string entityName)
      {
         Name = name;
         DataType = dataType;
         EntityName = entityName;
         Namespace = String.Empty;
         MemberInfoByName = new Dictionary<string, TypeMemberInfo>();
         MemberInfoByEntityName = new Dictionary<string, TypeMemberInfo>();
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeInfo" /> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="dataType"></param>
      /// <param name="entityName"></param>
      /// <param name="@namespace"></param>
      public TypeInfo(string name, Type dataType, string entityName, string @namespace)
      {
         Name = name;
         DataType = dataType;
         EntityName = entityName;
         Namespace = @namespace;
         MemberInfoByName = new Dictionary<string, TypeMemberInfo>();
         MemberInfoByEntityName = new Dictionary<string, TypeMemberInfo>();
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeInfo" /> class.
      /// </summary>
      /// <param name="typeName"></param>
      /// <param name="dataType"></param>
      /// <param name="members"></param>
      public TypeInfo(string typeName, Type dataType, IList<TypeMemberInfo> members)
      {
         Name = typeName;
         EntityName = typeName;
         DataType = dataType;
         MemberInfoByName = new Dictionary<string, TypeMemberInfo>();
         MemberInfoByEntityName = new Dictionary<string, TypeMemberInfo>();
         foreach (TypeMemberInfo field in members)
         {
            MemberInfoByName.Add(field.Name, field);
            MemberInfoByEntityName.Add(field.EntityName, field);
         }
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeInfo" /> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="dataType"></param>
      /// <param name="entityName"></param>
      /// <param name="@namespace"></param>
      /// <param name="members"></param>
      public TypeInfo(string name, Type dataType, string entityName, string @namespace, IList<TypeMemberInfo> members)
      {
         Name = name;
         DataType = dataType;
         EntityName = entityName;
         Namespace = @namespace;
         MemberInfoByName = new Dictionary<string, TypeMemberInfo>();
         MemberInfoByEntityName = new Dictionary<string, TypeMemberInfo>();
         foreach (TypeMemberInfo field in members)
         {
            MemberInfoByName.Add(field.Name, field);
            MemberInfoByEntityName.Add(field.EntityName, field);
         }
      }
   }
}