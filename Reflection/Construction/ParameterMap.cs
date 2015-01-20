using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection.Construction
{
   internal static class ParameterMap
   {
      public static IList<TypeMemberSerializationInfo> MapTypeMembersToParameters(IList<ParameterInfo> parameters)
      {
         var members = new List<TypeMemberSerializationInfo>();

         return members;
      }
   }
}
