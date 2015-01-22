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
using System.Reflection;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Utilities;

namespace Org.Edgerunner.DotSerialize.Reflection.Construction
{
   public static class TypeFactory
   {
      private static TypeConstructorCache Cache { get; set; }

      static TypeFactory()
      {
         Cache = new TypeConstructorCache();
      }

      public static T CreateInstance<T>(IDictionary<TypeMemberInfo, object> data)
      {
         return (T)CreateInstance(typeof(T), data);
      }

      public static object CreateInstance(Type type, IDictionary<TypeMemberInfo, object> data)
      {
         object result = null;

         object[] paramValues;
         var memberInfoList = data.Keys.ToList();
         var cachedMap = Cache.GetMappingFor(type, memberInfoList);
         if (cachedMap != null)
         {
            if (cachedMap.Parameters.Count == 0)
               result = cachedMap.Constructor.Invoke(null);
            else
            {
               paramValues = BuildParameterVaules(cachedMap.Parameters, cachedMap.Members, data);
               result = cachedMap.Constructor.Invoke(paramValues);
            }
         }
         else
         {
            var constructors = type.Constructors().OrderBy(x => x.Parameters().Count).ToList();
            foreach (var constructor in constructors)
               try
               {
                  Dictionary<ParameterInfo, TypeMemberInfo> paramMap;
                  if (constructor.Parameters().Count == 0)
                  {
                     paramMap = new Dictionary<ParameterInfo, TypeMemberInfo>();
                     result = constructor.Invoke(null);
                  }
                  else
                  {
                     paramMap = ParameterMapper.MapTypeMembersToParameters(type, constructor.Parameters(), memberInfoList);
                     paramValues = BuildParameterVaules(paramMap.Keys.ToList(), paramMap.Values.ToList(), data);
                     result = constructor.Invoke(paramValues);
                  }

                  if (result != null)
                     Cache.AddMappingFor(type, memberInfoList, new ConstructorMap(constructor, paramMap.Keys.ToList(), paramMap.Values.ToList()));
                  break;
               }
               catch (Exception ex)
               {
                  // Since we failed to create an instance, we try the next constructor
               }
         }
         if (result == null)
            throw new Exception(string.Format("Unable to create instance of type \"{0}\"", type.Name()));

         foreach (var memberInfo in data.Keys)
         {
            switch (memberInfo.Type)
            {
               case TypeMemberInfo.MemberType.Field:
                  result.SetFieldValue(memberInfo.Name, data[memberInfo]);
                  break;
               case TypeMemberInfo.MemberType.Property:
                  result.SetPropertyValue(memberInfo.Name, data[memberInfo]);
                  break;
            }
         }

         return result;
      }

      private static object[] BuildParameterVaules(IList<ParameterInfo> parameters, IList<TypeMemberInfo> members,
                                                 IDictionary<TypeMemberInfo, object> data)
      {
         if (parameters.Count != members.Count)
            throw new ArgumentException("Parameters Count does not match members Count.", "parameters");
         if (parameters.Count != data.Count)
            throw new ArgumentException("Parameters Count does not match data Count.", "parameters");
         var paramValues = new object[parameters.Count];
         for (int i = 0; i < members.Count; i++)
         {
            if (members[i] == null)
               try { paramValues[i] = parameters[i].ParameterType.GetDefaultValue(); }
               catch (Exception ex) { parameters[i] = null; }
            else
               paramValues[i] = data[members[i]];
         }
         return paramValues;
      }
   }
}