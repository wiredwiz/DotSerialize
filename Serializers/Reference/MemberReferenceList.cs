using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Serializers.Reference
{
   public sealed class MemberReferenceList : List<MemberReference>
   {
      public MemberReferenceList()
      {
         
      }
      public MemberReferenceList(int capacity)
         : base(capacity)
      {
         
      }
      public MemberReferenceList(IEnumerable<MemberReference> collection)
         : base(collection)
      {
         
      }

      public void UpdateReferences(object newValue)
      {
         foreach (MemberReference reference in this)
            reference.UpdateValue(newValue);
      }
   }
}
