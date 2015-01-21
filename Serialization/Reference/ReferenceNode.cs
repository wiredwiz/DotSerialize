using System;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public sealed class ReferenceNode
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ReferenceNode"/> class.
      /// </summary>
      /// <param name="sourceObject"></param>
      public ReferenceNode(object sourceObject)
      {
         References = new MemberReferenceList();
         _SourceObject = sourceObject;         
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="ReferenceNode"/> class.
      /// </summary>
      public ReferenceNode()
      {
         References = new MemberReferenceList();
         _SourceObject = null;
      }
      public MemberReferenceList References { get; private set; }
      private object _SourceObject;
      public object SourceObject
      {
         get { return _SourceObject; }
         set { UpdateSourceReference(value); }
      }

      private void UpdateSourceReference(object newValue)
      {
         _SourceObject = newValue;
         References.UpdateReferences(_SourceObject);         
      }
   }
}
