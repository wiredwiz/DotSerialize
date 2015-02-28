﻿#region Apache License 2.0

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

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public class CaptureNode
   {
      public int Id { get; set; }
      public TypeMemberInfo MemberInfo { get; set; }
      public int Index { get; set; }

      /// <summary>
      ///    Initializes a new instance of the <see cref="CaptureNode" /> class.
      /// </summary>
      /// <param name="id"></param>
      /// <param name="memberInfo"></param>
      /// <param name="index"></param>
      public CaptureNode(int id, TypeMemberInfo memberInfo, int index)
      {
         Id = id;
         MemberInfo = memberInfo;
         Index = index;
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="CaptureNode" /> class.
      /// </summary>
      /// <param name="id"></param>
      /// <param name="memberInfo"></param>
      public CaptureNode(int id, TypeMemberInfo memberInfo)
      {
         Id = id;
         MemberInfo = memberInfo;
         Index = -1;
      }
   }
}