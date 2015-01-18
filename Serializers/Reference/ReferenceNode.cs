using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Serializers.Reference
{
   public sealed class ReferenceNode
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ReferenceNode"/> class.
      /// </summary>
      /// <param name="sourceType"></param>
      /// <param name="sourceObject"></param>
      public ReferenceNode(Type sourceType, object sourceObject)
      {
         SourceType = sourceType;
         SourceObject = sourceObject;
         References = new MemberReferenceList();
      }
      public MemberReferenceList References { get; private set; }
      public Type SourceType { get; private set; }
      private object _SourceObject;
      public object SourceObject
      {
         get { return _SourceObject; }
         set { UpdateSourceReference(value); }
      }

      private void UpdateSourceReference(object newValue)
      {
         SourceObject = newValue;
         References.UpdateReferences(SourceObject);         
      }
   }
}
