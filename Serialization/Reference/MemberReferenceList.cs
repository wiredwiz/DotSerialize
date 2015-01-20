using System;
using System.Collections.Generic;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public sealed class MemberReferenceList : List<MemberReference>
   {
      private readonly List<MemberReference> _PendingReferences;

      public MemberReferenceList()
      {
         _PendingReferences = new List<MemberReference>();
      }
      public MemberReferenceList(int capacity)
         : base(capacity)
      {
         _PendingReferences = new List<MemberReference>();
      }
      public MemberReferenceList(IEnumerable<MemberReference> collection)
         : base(collection)
      {
         _PendingReferences = new List<MemberReference>();
      }

      public void UpdateReferences(object newValue)
      {
         foreach (MemberReference reference in this)
            reference.UpdateValue(newValue);
      }

      public void LogPendingReference(System.Reflection.MemberTypes type, string name)
      {
         _PendingReferences.Add(new MemberReference(null, type, name));
      }

      public void SavePendingReferences(object source)
      {
         foreach (MemberReference reference in _PendingReferences)
         {
            reference.Source = source;
            Add(reference);
         }
         _PendingReferences.Clear();
      }
   }
}
