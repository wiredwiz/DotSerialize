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
      public static Dictionary<ParameterInfo, TypeMemberInfo> MapTypeMembersToParameters(Type type, IList<ParameterInfo> parameters, IList<TypeMemberInfo> members)
      {
         var result = new Dictionary<ParameterInfo, TypeMemberInfo>(parameters.Count);
         foreach (ParameterInfo parameter in parameters)
         {
            foreach (TypeMemberInfo member in members)
            {
               if (parameter.ParameterType.IsAssignableFrom(member.DataType))
                  if (member.ConstructorFriendlyName.ToLowerInvariant().Trim('_') == parameter.Name.ToLowerInvariant().Trim('_'))
                  {
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
