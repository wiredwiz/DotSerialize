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
using System.Runtime.Serialization;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Utilities;

namespace Org.Edgerunner.DotSerialize.Reflection.Construction
{
   public static class TypeFactory
   {
      static TypeFactory()
      {
         Cache = new TypeConstructorCache();
         Helpers = new Dictionary<Type, IHelperFactory>();
      }

      private static TypeConstructorCache Cache { get; set; }
      private static Dictionary<Type, IHelperFactory> Helpers { get; set; }

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

      /// <summary>Attempts to create an instance of T and populate its members with data from the supplied IDictionary.</summary>
      /// <typeparam name="T">Type of instance to be created.</typeparam>
      /// <param name="data">
      /// IDictionary containing mappings of
      /// <see cref="Org.Edgerunner.DotSerialize.Reflection.TypeMemberInfo"/> instances to object data.
      /// </param>
      /// <param name="useConstructor">Specifies whether an object's constructor should be used when creating a new instance.</param>
      /// <returns>A new instance of type T populated with the data from the data argument.</returns>
      /// <exception caption=""cref="System.TypeLoadException">T is not a valid type.</exception>
      /// <exception caption=""cref="System.Reflection.TargetInvocationException">
      /// Unable to automatically create a new instance
      /// of type.
      /// </exception>
      /// <seealso cref="Org.Edgerunner.DotSerialize.Reflection.TypeMemberInfo"/>
      public static T CreateInstance<T>(IDictionary<TypeMemberInfo, object> data, bool useConstructor = false)
      {
         return (T)CreateInstance(typeof(T), data, useConstructor);
      }

      /// <summary>Attempts to create an instance of Type and populate its members with data from the supplied IDictionary.</summary>
      /// <param name="type">Type of instance to be created.</param>
      /// <param name="data">
      /// IDictionary containing mappings of
      /// <see cref="Org.Edgerunner.DotSerialize.Reflection.TypeMemberInfo"/> instances to object data.
      /// </param>
      /// <param name="useConstructor">Specifies whether an object's constructor should be used when creating a new instance.</param>
      /// <returns>A new instance of type Type populated with the data from the data argument.</returns>
      /// <exception caption=""cref="System.ArgumentNullException">type is null</exception>
      /// <exception caption=""cref="System.Reflection.TargetInvocationException">
      /// Unable to automatically create a new instance
      /// of type.
      /// </exception>
      /// <seealso cref="Org.Edgerunner.DotSerialize.Reflection.TypeMemberInfo"/>
      public static object CreateInstance(Type type, IDictionary<TypeMemberInfo, object> data, bool useConstructor = false)
      {
         object result = null;

         if (Helpers.ContainsKey(type))
            result = Helpers[type].CreateInstance();
         else
         {
            if (useConstructor)
            {
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
                  if (constructors.Count == 0)
                     result = Activator.CreateInstance(type);
                  else
                     foreach (var constructor in constructors)
                        if (AttemptConstructorMatch(type, constructor, memberInfoList, data, out result))
                           break;
               }
            }

            if (result == null)
               result = FormatterServices.GetUninitializedObject(type);
         }

         result = result.WrapIfValueType();
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
         result = result.UnwrapIfWrapped();
         return result;
      }

      /// <summary>
      ///    Registers an implementation of IHelperFactory to be used when creating new instances of Type T.
      /// </summary>
      /// <param name="helper">Instance of IHelperFactory.</param>
      /// <typeparam name="T">Type that the IHelperFactory will create.</typeparam>
      public static void RegisterHelper<T>(IHelperFactory helper)
      {
         Helpers[typeof(T)] = helper;
      }

      /// <summary>
      ///    Unregisters an implementation of IHelperFactory for Type T.
      /// </summary>
      /// <typeparam name="T">Type to unregister IHelperFactory from.</typeparam>
      public static void UnregisterHelper<T>()
      {
         Helpers.Remove(typeof(T));
      }
   }
}