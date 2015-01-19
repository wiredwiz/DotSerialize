using System;
using System.Collections.Generic;
using Org.Edgerunner.DotSerialize.Exceptions;

namespace Org.Edgerunner.DotSerialize.Serializers.Reference
{
   public interface IReferenceManager
   {
      void RegisterId(Guid id, object obj);
      void RegisterId(Guid id);
      bool IsRegistered(Guid id);
      bool IsRegistered(object obj);
      object GetObject(Guid id);
      Guid GetObjectId(object obj);
      void UpdateObject(Guid id, object newObject);
      MemberReferenceList MemberReferences(Guid id);
   }
}
