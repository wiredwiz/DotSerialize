using System;
using System.Collections.Generic;
using Org.Edgerunner.DotSerialize.Exceptions;

namespace Org.Edgerunner.DotSerialize.Serializers.Reference
{
   public interface IReferenceManager
   {
      Guid GetIdOfObject(object obj);
      void AddMemberReferenceForId(Guid id, MemberReference memberReference);
      ReferenceNode GetReferenceById(Guid id);
      Guid AddObject(object obj);
      void AddRerenceNode(Guid id, ReferenceNode node);
   }
}
