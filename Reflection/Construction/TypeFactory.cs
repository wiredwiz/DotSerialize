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

      #region Static Methods

      private static bool AttemptConstructorMatch(Type type,
                                                  ConstructorInfo constructor,
                                                  List<TypeMemberInfo> memberInfoList,
                                                  IDictionary<TypeMemberInfo, object> data,
                                                  out object result)
      {
         Dictionary<ParameterInfo, TypeMemberInfo> paramMap;
         if (constructor.Parameters().Count == 0)
         {
            paramMap = new Dictionary<ParameterInfo, TypeMemberInfo>();
            result = AttemptCreation(constructor);
            if (result == null)
               return false;
         }
         else
         {
            paramMap = ParameterMapper.MapTypeMembersToParameters(type, constructor.Parameters(), memberInfoList, true);
            var paramValues = BuildParameterValues(paramMap.Keys.ToList(), paramMap.Values.ToList(), data);
            result = AttemptCreation(constructor, paramValues);
            if (result == null)
            {
               // Now we make a second and much more forgiving attempt at matching our data to the parameters
               paramMap = ParameterMapper.MapTypeMembersToParameters(type, constructor.Parameters(), memberInfoList);
               paramValues = BuildParameterValues(paramMap.Keys.ToList(), paramMap.Values.ToList(), data);
               result = AttemptCreation(constructor, paramValues);
            }
            if (result == null)
               return false;
         }

         Cache.AddMappingFor(type,
                             memberInfoList,
                             new ConstructorMap(constructor, paramMap.Keys.ToList(), paramMap.Values.ToList()));
         return true;
      }

      private static object AttemptCreation(ConstructorInfo constructor, object[] paramValues = null)
      {
         try
         {
            return constructor.Invoke(paramValues);
         }
         catch (Exception)
         {
            return null;
         }
      }

      private static object[] BuildParameterValues(IList<ParameterInfo> parameters,
                                                   IList<TypeMemberInfo> members,
                                                   IDictionary<TypeMemberInfo, object> data)
      {
         if (parameters.Count != members.Count)
            throw new ArgumentException("Parameters Count does not match members Count.", "parameters");
         var paramValues = new object[parameters.Count];
         for (int i = 0; i < members.Count; i++)
            if (members[i] == null)
               try
               {
                  paramValues[i] = parameters[i].ParameterType.GetDefaultValue();
               }
               catch (Exception ex)
               {
                  parameters[i] = null;
               }
            else
               paramValues[i] = data[members[i]];
         return paramValues;
      }

      public static T CreateInstance<T>(IDictionary<TypeMemberInfo, object> data)
      {
         return (T)CreateInstance(typeof(T), data);
      }

      public static object CreateInstance(Type type, IDictionary<TypeMemberInfo, object> data)
      {
         object result = null;

         var memberInfoList = data.Keys.ToList();
         var cachedMap = Cache.GetMappingFor(type, memberInfoList);
         if (cachedMap != null)
            if (cachedMap.Parameters.Count == 0)
               result = AttemptCreation(cachedMap.Constructor);
            else
            {
               object[] paramValues = BuildParameterValues(cachedMap.Parameters, cachedMap.Members, data);
               result = AttemptCreation(cachedMap.Constructor, paramValues);
            }
         // If there was no cached mapping or if the mapping no longer works, we try to find a new one
         if (result == null)
         {
            var constructors = type.Constructors().OrderBy(x => x.Parameters().Count).ToList();
            foreach (var constructor in constructors)
               if (AttemptConstructorMatch(type, constructor, memberInfoList, data, out result))
                  break;
         }
         if (result == null)
            throw new Exception(string.Format("Unable to create instance of type \"{0}\"", type.Name()));

         foreach (var memberInfo in data.Keys)
            switch (memberInfo.Type)
            {
               case TypeMemberInfo.MemberType.Field:
                  result.SetFieldValue(memberInfo.Name, data[memberInfo]);
                  break;
               case TypeMemberInfo.MemberType.Property:
                  result.SetPropertyValue(memberInfo.Name, data[memberInfo]);
                  break;
            }

         return result;
      }

      #endregion
   }
}