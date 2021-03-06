﻿#region Apapche License 2.0

// <copyright file="ReferenceManager.cs" company="Edgerunner.org">
// Copyright 2015 Thaddeus Ryker
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection.Types;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public class ReferenceManager : IReferenceManager
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="ReferenceManager" /> class.
      /// </summary>
      public ReferenceManager()
      {
         ReferencesByGuid = new Dictionary<int, ReferenceNode>();
         ReferencesByInstance = new Dictionary<object, int>();
         CaptureStack = new Stack<CaptureSet>();
         CurrentId = 1;
      }

      protected Dictionary<int, ReferenceNode> ReferencesByGuid { get; set; }
      protected Dictionary<object, int> ReferencesByInstance { get; set; }
      protected Stack<CaptureSet> CaptureStack { get; set; }
      protected int CurrentId { get; set; }

      #region IReferenceManager Members

      public void CaptureLateBinding(int id, TypeMemberInfo info, int[] indices)
      {
         if (id == 0) throw new ArgumentNullException("id");
         if (info == null) throw new ArgumentNullException("info");
         if (indices == null) throw new ArgumentNullException("indices");

         CaptureStack.Peek().CaptureNodes.Add(new CaptureNode(id, info, indices));
      }

      public void CaptureLateBinding(int id, TypeMemberInfo info)
      {
         if (id == 0) throw new ArgumentNullException("id");
         if (info == null) throw new ArgumentNullException("info");

         CaptureStack.Peek().CaptureNodes.Add(new CaptureNode(id, info));
      }

      public void CaptureLateBinding(int id, int[] indices)
      {
         if (id == 0) throw new ArgumentNullException("id");
         if (indices == null) throw new ArgumentNullException("indices");

         CaptureSet set = CaptureStack.Peek();
         if (set.CurrentMember == null)
            throw new ReferenceException("CurrentMember of ReferenceManager is null");
         set.CaptureNodes.Add(new CaptureNode(id, set.CurrentMember, indices));
      }

      public void CaptureLateBinding(int id)
      {
         if (id == 0) throw new ArgumentNullException("id");

         CaptureSet set = CaptureStack.Peek();
         if (set.CurrentMember == null)
            throw new ReferenceException("CurrentMember of ReferenceManager is null");
         set.CaptureNodes.Add(new CaptureNode(id, set.CurrentMember));
      }

      public void FinishCaptures(object source)
      {
         var set = CaptureStack.Pop();
         if (source.GetType() != set.Type)
            throw new ReferenceException(
               string.Format("Source object does not match type \"{0}\" specified at the start of the capture.", 
                             set.Type.Name()));

         foreach (CaptureNode node in set.CaptureNodes)
         {
            if (node.Indices != null)
            {
               if (node.MemberInfo.Type == TypeMemberInfo.MemberType.Field)
                  MemberReferences(node.Id)
                     .Add(new MemberReference(source, MemberTypes.Field, node.MemberInfo.Name, node.Indices));
               else if (node.MemberInfo.Type == TypeMemberInfo.MemberType.Property)
                  MemberReferences(node.Id)
                     .Add(new MemberReference(source, MemberTypes.Property, node.MemberInfo.Name, node.Indices));
            }
            else if (node.MemberInfo.Type == TypeMemberInfo.MemberType.Field)
               MemberReferences(node.Id).Add(new MemberReference(source, MemberTypes.Field, node.MemberInfo.Name));
            else if (node.MemberInfo.Type == TypeMemberInfo.MemberType.Property)
               MemberReferences(node.Id).Add(new MemberReference(source, MemberTypes.Property, node.MemberInfo.Name));
            var idList = set.CaptureNodes.Select(item => item.Id).GroupBy(x => x).Select(grp => grp.First());
            foreach (int item in idList)
               if (GetObject(item) != null)
                  UpdateObject(item, GetObject(item));
         }
      }

      public object GetObject(int id)
      {
         if (id == 0) throw new ArgumentNullException("id");

         ReferenceNode value;
         if (!ReferencesByGuid.TryGetValue(id, out value))
            throw new ReferenceException(string.Format("No object exists for id {0}", id));

         return value.SourceObject;
      }

      public virtual int GetObjectId(object obj)
      {
         if (obj == null) throw new ArgumentNullException("obj");

         return ReferencesByInstance[obj];
      }

      public virtual bool IsRegistered(int id)
      {
         if (id == 0) throw new ArgumentNullException("id");

         return ReferencesByGuid.ContainsKey(id);
      }

      public virtual bool IsRegistered(object obj)
      {
         if (obj == null) throw new ArgumentNullException("obj");

         return ReferencesByInstance.ContainsKey(obj);
      }

      public virtual MemberReferenceList MemberReferences(int id)
      {
         if (id == 0) throw new ArgumentNullException("id");

         ReferenceNode value;
         if (!ReferencesByGuid.TryGetValue(id, out value))
            throw new ReferenceException(string.Format("No reference exists for id {0}", id));
         return value.References;
      }

      public virtual int RegisterId(int id, object obj)
      {
         if (id == 0) throw new ArgumentNullException("id");
         if (obj == null) throw new ArgumentNullException("obj");

         CurrentId = id + 1;
         ReferencesByGuid.Add(id, new ReferenceNode(obj));
         ReferencesByInstance[obj] = id;
         return id;
      }

      public virtual int RegisterId(object obj)
      {
         if (obj == null) throw new ArgumentNullException("obj");

         int id = CurrentId;
         CurrentId++;
         ReferencesByGuid.Add(id, new ReferenceNode(obj));
         ReferencesByInstance[obj] = id;
         return id;
      }

      public virtual int RegisterId(int id)
      {
         if (id == 0) throw new ArgumentNullException("id");

         CurrentId = id + 1;
         ReferencesByGuid.Add(id, new ReferenceNode());
         return id;
      }

      public virtual int RegisterId()
      {
         int id = CurrentId;
         ReferencesByGuid.Add(id, new ReferenceNode());
         CurrentId++;
         return id;
      }

      public void SetWorkingMember(TypeMemberInfo member)
      {
         if (member == null) throw new ArgumentNullException("member");

         CaptureStack.Peek().CurrentMember = member;
      }

      public void StartLateBindingCapture(Type type)
      {
         CaptureStack.Push(new CaptureSet(type));
      }

      public virtual void UpdateObject(int id, object newObject)
      {
         if (id == 0) throw new ArgumentNullException("id");
         if (newObject == null) throw new ArgumentNullException("newObject");

         var node = ReferencesByGuid[id];
         node.SourceObject = newObject;

         // Now that we updated all references, we clear the current list of latebound references
         node.References.Clear();
      }

      #endregion
   }
}