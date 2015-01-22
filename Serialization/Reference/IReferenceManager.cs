using System;
using Org.Edgerunner.DotSerialize.Reflection;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public interface IReferenceManager
   {
      int RegisterId(int id, object obj);
      int RegisterId(object obj);
      int RegisterId(int id);
      int RegisterId();
      bool IsRegistered(int id);
      bool IsRegistered(object obj);
      object GetObject(int id);
      int GetObjectId(object obj);
      void UpdateObject(int id, object newObject);
      MemberReferenceList MemberReferences(int id);
      void StartLateBindingCapture(Type type);
      void FinishCaptures(object source);
      void SetWorkingMember(TypeMemberSerializationInfo info);
      void CaptureLateBinding(int id, TypeMemberSerializationInfo info, int index);
      void CaptureLateBinding(int id, TypeMemberSerializationInfo info);
      void CaptureLateBinding(int id, int index);
      void CaptureLateBinding(int id);
   }
}
