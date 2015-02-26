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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Utilities;
using TypeInfo = Org.Edgerunner.DotSerialize.Reflection.TypeInfo;

namespace Org.Edgerunner.DotSerialize.Mapping
{
   /// <summary>
   ///    Class used to define the xml layout for a given class type.
   /// </summary>
   /// <typeparam name="T">Class type for which the layout is being defined.</typeparam>
   public abstract class XmlClassMap<T> : ClassMapBase, IXmlClassMap<T>
   {
      protected string _Namespace;
      protected string _RootNodeName;
      protected readonly List<XmlNodeMap> _Mappings;

      /// <summary>
      ///    Initializes a new instance of the <see cref="XmlClassMap" /> class.
      /// </summary>
      protected XmlClassMap()
      {
         _Mappings = new List<XmlNodeMap>();
         _RootNodeName = TypeInspector.CleanNodeName(typeof(T).Name());
         _Namespace = string.Empty;
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="XmlClassMap" /> class.
      /// </summary>
      /// <param name="rootNodeName"></param>
      protected XmlClassMap(string rootNodeName)
      {
         _RootNodeName = rootNodeName;
         _Mappings = new List<XmlNodeMap>();
         _Namespace = String.Empty;
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="XmlClassMap" /> class.
      /// </summary>
      /// <param name="rootNodeName"></param>
      /// <param name="@namespace"></param>
      protected XmlClassMap(string rootNodeName, string @namespace)
      {
         _RootNodeName = rootNodeName;
         _Namespace = @namespace;
         _Mappings = new List<XmlNodeMap>();
      }

      #region IXmlClassMap<T> Members

      /// <summary>
      ///    Specifies the name that should be used for the xml root node.
      /// </summary>
      /// <param name="name">Name to use.</param>
      /// <returns>Current class map instance.</returns>
      public XmlClassMap<T> Named(string name)
      {
         _RootNodeName = name;
         return this;
      }

      /// <summary>
      ///    Specifies the namespace to use for the xml root.
      /// </summary>
      /// <param name="namespace">Namespace to use</param>
      /// <returns>Current class map instance.</returns>
      public XmlClassMap<T> WithNamespace(string @namespace)
      {
         _Namespace = @namespace;
         return this;
      }

      #endregion

      /// <summary>
      ///    Maps a property to a CSV field.
      /// </summary>
      /// <param name="expression">The property to map.</param>
      /// <returns>The property mapping.</returns>
      protected virtual XmlNodeMap Map(Expression<Func<T, object>> expression)
      {
         var member = TypeHelper.GetMemberExpression(expression).Member;
         XmlNodeMap nodeMap;
         var property = member as PropertyInfo;
         if (property != null)
         {
            nodeMap = new XmlNodeMap(property.Name, TypeMemberInfo.MemberType.Property, property.PropertyType);
            _Mappings.Add(nodeMap);
            return nodeMap;
         }
         var field = member as FieldInfo;
         if (field != null)
         {
            nodeMap = new XmlNodeMap(field.Name, TypeMemberInfo.MemberType.Field, field.FieldType);
            _Mappings.Add(nodeMap);
            return nodeMap;
         }

         throw new MappingException(string.Format("'{0}' is not a property or field reference.", member.Name));
      }

      internal override TypeInfo GetTypeInfo()
      {
         var type = typeof(T);
         var memberInfo = new List<TypeMemberInfo>(_Mappings.Count);
         memberInfo.AddRange(_Mappings.Select(mapping => mapping.Info));
         var info = new TypeInfo(type.Name, type, _RootNodeName, _Namespace, memberInfo);
         return info;
      }
   }
}