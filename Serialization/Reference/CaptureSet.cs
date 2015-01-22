using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Reflection;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public class CaptureSet
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="CaptureSet"/> class.
      /// </summary>
      /// <param name="type"></param>
      public CaptureSet(Type type)
      {
         Type = type;
         CurrentMember = null;
         CaptureNodes = new List<CaptureNode>();
      }
      public Type Type { get; set; }
      public TypeMemberSerializationInfo CurrentMember { get; set; }
      public List<CaptureNode> CaptureNodes { get; set; }
   }
}
