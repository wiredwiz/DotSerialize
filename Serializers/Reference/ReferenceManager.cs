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

      public virtual void RegisterId(Guid id, object obj)
      {
         ReferencesByGuid.Add(id, new ReferenceNode(obj.GetType(), obj));
         ReferencesByInstance[obj] = id;
      }

      public virtual void RegisterId(Guid id)
      {
         ReferencesByGuid.Add(id, new ReferenceNode(typeof(object), null));
      }

      public virtual bool IsRegistered(Guid id)
      {
         return ReferencesByGuid.ContainsKey(id);
      }

      public virtual bool IsRegistered(object obj)
      {
         return ReferencesByInstance.ContainsKey(obj);
      }

      public object GetObject(Guid id)
      {
         if (!ReferencesByGuid.ContainsKey(id))
            throw new ReferenceException(string.Format("No object exists for id {0}", id));
         return ReferencesByGuid[id].SourceObject;
      }

      public virtual Guid GetObjectId(object obj)
      {
         return ReferencesByInstance[obj];
      }

      public virtual void UpdateObject(Guid id, object newObject)
      {
         var node = ReferencesByGuid[id];
         node.SourceObject = newObject;
         node.SourceType = newObject.GetType();
      }

      public virtual MemberReferenceList MemberReferences(Guid id)
      {
         if (!ReferencesByGuid.ContainsKey(id))
            throw new ReferenceException(string.Format("No reference exists for id {0}", id));
         return ReferencesByGuid[id].References;
      }
   }
}
