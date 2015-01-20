using System;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
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
         References = new MemberReferenceList();
         SourceType = sourceType;
         _SourceObject = sourceObject;         
      }
      public MemberReferenceList References { get; private set; }
      public Type SourceType { get; set; }
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
