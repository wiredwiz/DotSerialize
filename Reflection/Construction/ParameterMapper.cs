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
using System.Reflection;

namespace Org.Edgerunner.DotSerialize.Reflection.Construction
{
   internal static class ParameterMapper
   {
      #region Static Methods

      public static Dictionary<ParameterInfo, TypeMemberInfo> MapTypeMembersToParameters(Type type,
                                                                                         IList<ParameterInfo> parameters,
                                                                                         IList<TypeMemberInfo> members,
                                                                                         bool requireMatchByname = false)
      {
         var result = new Dictionary<ParameterInfo, TypeMemberInfo>(parameters.Count);
         var used = new List<TypeMemberInfo>(members.Count);
         foreach (ParameterInfo parameter in parameters)
         {
            foreach (TypeMemberInfo member in members)
               if (!used.Contains(member) && parameter.ParameterType.IsAssignableFrom(member.DataType))
                  if (!requireMatchByname ||
                      (member.ConstructorFriendlyName.ToLowerInvariant().Trim('_') == parameter.Name.ToLowerInvariant().Trim('_')))
                  {
                     used.Add(member);
                     result.Add(parameter, member);
                     break;
                  }
            if (!result.ContainsKey(parameter))
               result.Add(parameter, null);
         }
         return result;
      }

      #endregion
   }
}