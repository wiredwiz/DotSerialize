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
using Org.Edgerunner.DotSerialize.Reflection.Types;

namespace Org.Edgerunner.DotSerialize.Reflection.Caching
{
   public class WeakSerializationInfoCache : SerializationInfoCacheBase<WeakReference<TypeInfo>>
   {
      public override void AddInfo(TypeInfo info)
      {
         _InternalCache.Add(info.DataType, new WeakReference<TypeInfo>(info));
      }

      public override TypeInfo GetInfo(Type type)
      {
         if (!_InternalCache.ContainsKey(type))
            return null;
         var reference = _InternalCache[type];
         TypeInfo result;
         if (reference.TryGetTarget(out result))
            return result;
         _InternalCache.Remove(type);
         return null;
      }
   }
}