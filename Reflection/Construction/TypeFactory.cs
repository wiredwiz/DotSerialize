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
using Fasterflect;

namespace Org.Edgerunner.DotSerialize.Reflection.Construction
{
   public static class TypeFactory
   {
      private static TypeConstructorCache Cache { get; set; }

      static TypeFactory()
      {
         Cache = new TypeConstructorCache();
      }

      public static T CreateInstance<T>(IDictionary<TypeMemberSerializationInfo, object> data)
      {
         return (T)CreateInstance(typeof(T), data);
      }

      public static object CreateInstance(Type type, IDictionary<TypeMemberSerializationInfo, object> data)
      {
         //object result = type.TryCreateInstance((from x in data.Keys
         //                                        select x.ConstructorFriendlyName).ToArray<string>(),
         //                                       (from x in data.Keys
         //                                        select x.DataType).ToArray<Type>(),
         //                                       data.Values.ToArray()
         //   );
         object result = null;

         object[] paramValues;
         var memberInfoList = data.Keys.ToList();
         var cachedMap = Cache.GetMappingFor(type, memberInfoList);
         if (cachedMap != null)
         {
            paramValues = BuildParameterVaules(cachedMap.Parameters, data);
            result = cachedMap.Constructor.Invoke(paramValues);
         }
         else
         {
            var constructors = type.Constructors().OrderBy(x => x.Parameters().Count).ToList();
            foreach (var constructor in constructors)
               try
               {
                  Dictionary<string, TypeMemberSerializationInfo> paramMap;
                  if (constructor.Parameters().Count == 0)
                  {
                     paramMap = new Dictionary<string, TypeMemberSerializationInfo>();
                     result = constructor.Invoke(null);
                  }
                  else
                  {                     
                     paramMap = ParameterMapper.MapTypeMembersToParameters(type, constructor.Parameters(), memberInfoList);
                     paramValues = BuildParameterVaules(paramMap, data);
                     result = constructor.Invoke(paramValues);
                  }

                  if (result != null)
                     Cache.AddMappingFor(type, memberInfoList, new ConstructorMap(constructor, paramMap));
                     break;
               }
               catch (Exception ex)
               {
               }
         }
         if (result == null)
            throw new Exception(string.Format("Unable to create instance of type \"{0}\"", type.Name()));

         foreach (var memberInfo in data.Keys)
         {
            switch (memberInfo.Type)
            {
               case TypeMemberSerializationInfo.MemberType.Field:
                  result.SetFieldValue(memberInfo.Name, data[memberInfo]);
                  break;
               case TypeMemberSerializationInfo.MemberType.Property:
                  result.SetPropertyValue(memberInfo.Name, data[memberInfo]);
                  break;
            }
         }

         return result;
      }

      private static object[] BuildParameterVaules(Dictionary<string, TypeMemberSerializationInfo> parameterMap,
                                                 IDictionary<TypeMemberSerializationInfo, object> data)
      {
         var paramValues = new object[parameterMap.Count];
         int counter = 0;
         foreach (var pair in parameterMap)
         {
            if (pair.Value == null)
               paramValues[counter] = null;
            else
               paramValues[counter] = data[pair.Value];
            counter++;
         }
         return paramValues;
      }
   }
}