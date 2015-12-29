#region Apapche License 2.0

// <copyright file="CaptureSet.cs" company="Edgerunner.org">
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
using Org.Edgerunner.DotSerialize.Reflection.Types;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public class CaptureSet
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="CaptureSet" /> class.
      /// </summary>
      /// <param name="type"></param>
      public CaptureSet(Type type)
      {
         Type = type;
         CurrentMember = null;
         CaptureNodes = new List<CaptureNode>();
      }

      public Type Type { get; set; }
      public TypeMemberInfo CurrentMember { get; set; }
      public List<CaptureNode> CaptureNodes { get; set; }
   }
}