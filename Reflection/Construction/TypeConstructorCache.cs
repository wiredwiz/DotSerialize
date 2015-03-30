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
using Org.Edgerunner.DotSerialize.Reflection.Types;

namespace Org.Edgerunner.DotSerialize.Reflection.Construction
{
   public class TypeConstructorCache
   {
      protected Dictionary<Type, Dictionary<int, ConstructorMap>> _Mappings;

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeConstructorCache" /> class.
      /// </summary>
      public TypeConstructorCache()
      {
         _Mappings = new Dictionary<Type, Dictionary<int, ConstructorMap>>();
      }

      public void AddMappingFor(Type type, IList<TypeMemberInfo> info, ConstructorMap constructorMap)
      {
         if (type == null) throw new ArgumentNullException("type");
         if (info == null) throw new ArgumentNullException("info");

         if (!_Mappings.ContainsKey(type))
            _Mappings[type] = new Dictionary<int, ConstructorMap>();

         _Mappings[type].Add(GetHash(info), constructorMap);
      }

      protected int GetHash(IList<TypeMemberInfo> info)
      {
         if (info.Count == 0)
            return 0;
         if (info.Count == 1)
            return info[0].GetHashCode();
         info = info.OrderBy(x => x.Name).ToList();
         //info.Sort((x, y) => x.Name.CompareTo(y));
         int result = info[0].GetHashCode();
         for (int i = 1; i < info.Count; i++)
            result = (result * 397) ^ info[i].GetHashCode();
         return result;
      }

      public ConstructorMap GetMappingFor(Type type, IList<TypeMemberInfo> info)
      {
         if (type == null) throw new ArgumentNullException("type");
         if (info == null) throw new ArgumentNullException("info");

         if (!_Mappings.ContainsKey(type))
            return null;

         var typeMappings = _Mappings[type];
         int key = GetHash(info);
         if (!typeMappings.ContainsKey(key))
            return null;
         return typeMappings[key];
      }
   }
}