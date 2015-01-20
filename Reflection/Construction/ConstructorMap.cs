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
      public Dictionary<string, TypeMemberSerializationInfo> Parameters { get; set; }
      /// <summary>
      /// Initializes a new instance of the <see cref="ConstructorMap"/> class.
      /// </summary>
      /// <param name="constructor"></param>
      /// <param name="parameters"></param>
      public ConstructorMap(ConstructorInfo constructor, Dictionary<string, TypeMemberSerializationInfo> parameters)
      {
         Constructor = constructor;
         Parameters = parameters;
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="ConstructorMap"/> class.
      /// </summary>
      /// <param name="constructor"></param>
      public ConstructorMap(ConstructorInfo constructor)
      {
         Constructor = constructor;
         Parameters = new Dictionary<string, TypeMemberSerializationInfo>();
      }
   }
}
