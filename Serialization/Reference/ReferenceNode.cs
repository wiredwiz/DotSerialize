#region Apapche License 2.0

// <copyright file="ReferenceNode.cs" company="Edgerunner.org">
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

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public sealed class ReferenceNode
   {
      private object _SourceObject;

      /// <summary>
      ///    Initializes a new instance of the <see cref="ReferenceNode" /> class.
      /// </summary>
      /// <param name="sourceObject"></param>
      public ReferenceNode(object sourceObject)
      {
         References = new MemberReferenceList();
         _SourceObject = sourceObject;
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="ReferenceNode" /> class.
      /// </summary>
      public ReferenceNode()
      {
         References = new MemberReferenceList();
         _SourceObject = null;
      }

      public MemberReferenceList References { get; private set; }

      public object SourceObject
      {
         get { return _SourceObject; }
         set { UpdateSourceReference(value); }
      }

      private void UpdateSourceReference(object newValue)
      {
         _SourceObject = newValue;
         References.UpdateReferences(_SourceObject);
      }
   }
}