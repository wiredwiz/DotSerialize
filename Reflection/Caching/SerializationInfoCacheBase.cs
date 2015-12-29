#region Apapche License 2.0

// <copyright file="SerializationInfoCacheBase.cs" company="Edgerunner.org">
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

namespace Org.Edgerunner.DotSerialize.Reflection.Caching
{
   public abstract class SerializationInfoCacheBase<T> : ISerializationInfoCache
   {
      protected readonly Dictionary<Type, T> _InternalCache;

      /// <summary>
      /// Initializes a new instance of the <see cref="SerializationInfoCacheBase{T}"/> class. 
      ///    Initializes a new instance of the <see cref="SerializationInfoCache"/> class.
      /// </summary>
      protected SerializationInfoCacheBase()
      {
         _InternalCache = new Dictionary<Type, T>();
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SerializationInfoCacheBase{T}"/> class. 
      ///    Initializes a new instance of the <see cref="SerializationInfoCache"/> class.
      /// </summary>
      /// <param name="capacity">
      /// </param>
      protected SerializationInfoCacheBase(int capacity)
      {
         _InternalCache = new Dictionary<Type, T>(capacity);
      }

      #region ISerializationInfoCache Members

      public abstract void AddInfo(TypeInfo info);
      public abstract TypeInfo GetInfo(Type type);

      #endregion
   }
}