using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection.Construction
{
   internal static class ParameterMapper
   {
      public static Dictionary<ParameterInfo, TypeMemberInfo> MapTypeMembersToParameters(Type type, IList<ParameterInfo> parameters, IList<TypeMemberInfo> members, bool requireMatchByname = false)
      {
         var result = new Dictionary<ParameterInfo, TypeMemberInfo>(parameters.Count);
         var used = new List<TypeMemberInfo>(members.Count);
         foreach (ParameterInfo parameter in parameters)
         {
            foreach (TypeMemberInfo member in members)
            {
               if (!used.Contains(member) && parameter.ParameterType.IsAssignableFrom(member.DataType))
                  if (!requireMatchByname || 
                      (member.ConstructorFriendlyName.ToLowerInvariant().Trim('_') == parameter.Name.ToLowerInvariant().Trim('_')))
                  {
                     used.Add(member);
                     result.Add(parameter, member);
                     break;
                  }
            }
            if (!result.ContainsKey(parameter))
               result.Add(parameter, null);
         }
         return result;
      }
   }
}
