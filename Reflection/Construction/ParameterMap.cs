using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection.Construction
{
   internal class ParameterMap
   {
      public IList<TypeMemberSerializationInfo> MapTypeMembersToParameters(Type type, IList<ParameterInfo> parameters)
      {
         var members = new List<TypeMemberSerializationInfo>();

         return members;
      }
   }
}
