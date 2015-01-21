using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Reflection;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public class CaptureNode
   {
      public Guid Id { get; set; }
      public TypeMemberSerializationInfo MemberInfo { get; set; }
      public int Index { get; set; }
      /// <summary>
      /// Initializes a new instance of the <see cref="CaptureNode"/> class.
      /// </summary>
      /// <param name="id"></param>
      /// <param name="memberInfo"></param>
      /// <param name="index"></param>
      public CaptureNode(Guid id, TypeMemberSerializationInfo memberInfo, int index)
      {
         Id = id;
         MemberInfo = memberInfo;
         Index = index;
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="CaptureNode"/> class.
      /// </summary>
      /// <param name="id"></param>
      /// <param name="memberInfo"></param>
      public CaptureNode(Guid id, TypeMemberSerializationInfo memberInfo)
      {
         Id = id;
         MemberInfo = memberInfo;
         Index = -1;
      }
   }
}
