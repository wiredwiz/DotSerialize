using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection.Construction
{
   public class ConstructorMap
   {
      public ConstructorInfo Constructor { get; set; }
      public IList<ParameterInfo> Parameters { get; set; }
      public IList<TypeMemberSerializationInfo> Members { get; set; }
      /// <summary>
      /// Initializes a new instance of the <see cref="ConstructorMap"/> class.
      /// </summary>
      /// <param name="constructor"></param>
      public ConstructorMap(ConstructorInfo constructor)
      {
         Constructor = constructor;
         Parameters = null;
         Members = null;
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="ConstructorMap"/> class.
      /// </summary>
      /// <param name="constructor"></param>
      /// <param name="parameters"></param>
      /// <param name="members"></param>
      public ConstructorMap(ConstructorInfo constructor, IList<ParameterInfo> parameters, IList<TypeMemberSerializationInfo> members)
      {
         Constructor = constructor;
         Parameters = parameters;
         Members = members;
      }
   }
}
