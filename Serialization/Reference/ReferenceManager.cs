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
using System.Collections.Generic;
using System.Linq;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public class ReferenceManager : IReferenceManager
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="ReferenceManager" /> class.
      /// </summary>
      public ReferenceManager()
      {
         ReferencesByGuid = new Dictionary<Guid, ReferenceNode>();
         ReferencesByInstance = new Dictionary<object, Guid>();
         CaptureNodes = new List<CaptureNode>();
      }

      protected Dictionary<Guid, ReferenceNode> ReferencesByGuid { get; set; }
      protected Dictionary<object, Guid> ReferencesByInstance { get; set; }
      protected Type CurrentCaptureType { get; set; }
      protected TypeMemberSerializationInfo CurrentMember { get; set; }
      protected List<CaptureNode> CaptureNodes { get; set; }

      public virtual Guid RegisterId(Guid id, object obj)
      {
         if (id == Guid.Empty) throw new ArgumentNullException("id");
         if (obj == null) throw new ArgumentNullException("obj");

         ReferencesByGuid.Add(id, new ReferenceNode(obj));
         ReferencesByInstance[obj] = id;
         return id;
      }

      public virtual Guid RegisterId(Guid id)
      {
         if (id == Guid.Empty) throw new ArgumentNullException("id");

         ReferencesByGuid.Add(id, new ReferenceNode());
         return id;
      }

      public virtual bool IsRegistered(Guid id)
      {
         if (id == Guid.Empty) throw new ArgumentNullException("id");

         return ReferencesByGuid.ContainsKey(id);
      }

      public virtual bool IsRegistered(object obj)
      {
         if (obj == null) throw new ArgumentNullException("obj");

         return ReferencesByInstance.ContainsKey(obj);
      }

      public object GetObject(Guid id)
      {
         if (id == Guid.Empty) throw new ArgumentNullException("id");

         if (!ReferencesByGuid.ContainsKey(id))
            throw new ReferenceException(string.Format("No object exists for id {0}", id));
         return ReferencesByGuid[id].SourceObject;
      }

      public virtual Guid GetObjectId(object obj)
      {
         if (obj == null) throw new ArgumentNullException("obj");

         return ReferencesByInstance[obj];
      }

      public virtual void UpdateObject(Guid id, object newObject)
      {
         if (id == Guid.Empty) throw new ArgumentNullException("id");
         if (newObject == null) throw new ArgumentNullException("newObject");

         var node = ReferencesByGuid[id];
         node.SourceObject = newObject;
         // Now that we updated all references, we clear the current list of latebound references
         node.References.Clear();
      }

      public virtual MemberReferenceList MemberReferences(Guid id)
      {
         if (id == Guid.Empty) throw new ArgumentNullException("id");

         if (!ReferencesByGuid.ContainsKey(id))
            throw new ReferenceException(string.Format("No reference exists for id {0}", id));
         return ReferencesByGuid[id].References;
      }

      public void StartLateBindingCapture(Type type)
      {
         CurrentMember = null;
         CaptureNodes.Clear();
         CurrentCaptureType = type;
      }

      public void FinishCaptures(object source)
      {
         CurrentMember = null;
         if (source.GetType() != CurrentCaptureType)
            throw new ReferenceException(string.Format("Source object does not match type \"{0}\" specified at the start of the capture.",
                                                       CurrentCaptureType.Name()));

         foreach (CaptureNode node in CaptureNodes)
         {
            if (node.Index >= 0)
            {
               if (node.MemberInfo.Type == TypeMemberSerializationInfo.MemberType.Field)
                  MemberReferences(node.Id).Add(new MemberReference(source, System.Reflection.MemberTypes.Field, node.MemberInfo.Name, node.Index));
               else if (node.MemberInfo.Type == TypeMemberSerializationInfo.MemberType.Property)
                  MemberReferences(node.Id).Add(new MemberReference(source, System.Reflection.MemberTypes.Property, node.MemberInfo.Name, node.Index));
            }
            else
            {
               if (node.MemberInfo.Type == TypeMemberSerializationInfo.MemberType.Field)
                  MemberReferences(node.Id).Add(new MemberReference(source, System.Reflection.MemberTypes.Field, node.MemberInfo.Name));
               else if (node.MemberInfo.Type == TypeMemberSerializationInfo.MemberType.Property)
                  MemberReferences(node.Id).Add(new MemberReference(source, System.Reflection.MemberTypes.Property, node.MemberInfo.Name));
            }
            var idList = CaptureNodes.Select(item => item.Id).GroupBy(x => x).Select(grp => grp.First());
            foreach (Guid item in idList)
            {
               if (GetObject(item) != null)
                  UpdateObject(item, GetObject(item));
            }
         }
      }

      public void SetWorkingMember(TypeMemberSerializationInfo member)
      {
         if (member == null) throw new ArgumentNullException("member");

         CurrentMember = member;
      }

      public void CaptureLateBinding(Guid id, TypeMemberSerializationInfo info, int index)
      {
         if (id == Guid.Empty) throw new ArgumentNullException("id");
         if (info == null) throw new ArgumentNullException("info");
         if (index < 0) throw new ArgumentNullException("index");

         CaptureNodes.Add(new CaptureNode(id, info, index));
      }

      public void CaptureLateBinding(Guid id, TypeMemberSerializationInfo info)
      {
         if (id == Guid.Empty) throw new ArgumentNullException("id");
         if (info == null) throw new ArgumentNullException("info");

         CaptureNodes.Add(new CaptureNode(id, info));
      }

      public void CaptureLateBinding(Guid id)
      {
         if (id == Guid.Empty) throw new ArgumentNullException("id");
         if (CurrentMember == null)
            throw new ReferenceException("CurrentMember of ReferenceManager is null");

         CaptureNodes.Add(new CaptureNode(id, CurrentMember));
      }
   }
}