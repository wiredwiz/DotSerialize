using System;
using System.Collections.Generic;
using Org.Edgerunner.DotSerialize.Exceptions;

namespace Org.Edgerunner.DotSerialize.Serializers.Reference
{
   public class ReferenceManager : IReferenceManager 
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ReferenceCache"/> class.
      /// </summary>
      public ReferenceManager()
      {
         ReferencesByGuid = new Dictionary<Guid, ReferenceNode>();
         ReferencesByInstance = new Dictionary<object, Guid>();
      }

      protected Dictionary<Guid, ReferenceNode> ReferencesByGuid { get; set; }
      protected Dictionary<object, Guid> ReferencesByInstance { get; set; }

      public virtual Guid GetIdOfObject(object obj)
      {
         if (!ReferencesByInstance.ContainsKey(obj))
            return Guid.Empty;
         return ReferencesByInstance[obj];
      }

      public void AddMemberReferenceForId(Guid id, MemberReference memberReference)
      {
         if (!ReferencesByGuid.ContainsKey(id))
            throw new ReferenceException(string.Format("No reference exists for id {0}", id));
         ReferencesByGuid[id].References.Add(memberReference);
      }

      public ReferenceNode GetReferenceById(Guid id)
      {
         if (!ReferencesByGuid.ContainsKey(id))
            throw new ReferenceException(string.Format("No reference exists for id {0}", id));
         return ReferencesByGuid[id];
      }

      public Guid AddObject(object obj)
      {
         throw new NotImplementedException();
      }

      public void AddRerenceNode(Guid id, ReferenceNode node)
      {
         ReferencesByGuid.Add(id, node);
      }
   }
}
