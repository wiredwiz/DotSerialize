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
      public static Dictionary<string, TypeMemberSerializationInfo> MapTypeMembersToParameters(Type type, IList<ParameterInfo> parameters, IList<TypeMemberSerializationInfo> members)
      {
         var result = new Dictionary<string, TypeMemberSerializationInfo>(parameters.Count);
         foreach (ParameterInfo parameter in parameters)
         {
            foreach (TypeMemberSerializationInfo member in members)
            {
               if (member.DataType == parameter.ParameterType)
                  if (member.ConstructorFriendlyName.ToLowerInvariant().Trim('_') == parameter.Name.ToLowerInvariant().Trim('_'))
                  {
                     result.Add(parameter.Name, member);
                     break;
                  }
            }
            if (!result.ContainsKey(parameter.Name))
               result.Add(parameter.Name, null);
         }
         return result;
      }
   }
}
