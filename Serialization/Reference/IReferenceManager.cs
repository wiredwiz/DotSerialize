using System;
using Org.Edgerunner.DotSerialize.Reflection;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public interface IReferenceManager
   {
      Guid RegisterId(Guid id, object obj);
      Guid RegisterId(Guid id);
      bool IsRegistered(Guid id);
      bool IsRegistered(object obj);
      object GetObject(Guid id);
      Guid GetObjectId(object obj);
      void UpdateObject(Guid id, object newObject);
      MemberReferenceList MemberReferences(Guid id);
      void StartLateBindingCapture(Type type);
      void FinishCaptures(object source);
      void SetWorkingMember(TypeMemberSerializationInfo info);
      void CaptureLateBinding(Guid id, TypeMemberSerializationInfo info, int index);
      void CaptureLateBinding(Guid id, TypeMemberSerializationInfo info);
      void CaptureLateBinding(Guid id);
   }
}
