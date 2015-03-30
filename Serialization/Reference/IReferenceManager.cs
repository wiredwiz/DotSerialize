#region Apache License 2.0

// Copyright 2015 Thaddeus Ryker
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Reflection.Types;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public interface IReferenceManager
   {
      void CaptureLateBinding(int id, TypeMemberInfo info, int index);
      void CaptureLateBinding(int id, TypeMemberInfo info);
      void CaptureLateBinding(int id, int index);
      void CaptureLateBinding(int id);
      void FinishCaptures(object source);
      object GetObject(int id);
      int GetObjectId(object obj);
      bool IsRegistered(int id);
      bool IsRegistered(object obj);
      MemberReferenceList MemberReferences(int id);
      int RegisterId(int id, object obj);
      int RegisterId(object obj);
      int RegisterId(int id);
      int RegisterId();
      void SetWorkingMember(TypeMemberInfo info);
      void StartLateBindingCapture(Type type);
      void UpdateObject(int id, object newObject);
   }
}