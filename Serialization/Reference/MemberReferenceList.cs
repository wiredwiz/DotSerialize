#region Apapche License 2.0

// <copyright file="MemberReferenceList.cs" company="Edgerunner.org">
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
using System.Reflection;

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

      public void LogPendingReference(MemberTypes type, string name)
      {
         _PendingReferences.Add(new MemberReference(null, type, name));
      }

      public void LogPendingReference(MemberTypes type, string name, int[] indices)
      {
         _PendingReferences.Add(new MemberReference(null, type, name, indices));
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

      public void UpdateReferences(object newValue)
      {
         foreach (MemberReference reference in this)
            reference.UpdateValue(newValue);
      }
   }
}