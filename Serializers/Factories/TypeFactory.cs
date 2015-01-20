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
using Org.Edgerunner.DotSerialize.Reflection;

namespace Org.Edgerunner.DotSerialize.Serializers.Factories
{
   public static class TypeFactory
   {
      public static T CreateInstance<T>(IDictionary<TypeMemberSerializationInfo, object> data)
      {
         return (T)CreateInstance(typeof(T), data);
      }

      public static object CreateInstance(Type type, IDictionary<TypeMemberSerializationInfo, object> data)
      {
         object result = type.TryCreateInstance((from x in data.Keys
                                                 select x.ConstructorFriendlyName).ToArray<string>(),
                                                (from x in data.Keys
                                                 select x.DataType).ToArray<Type>(),
                                                data.Values.ToArray()
            );
         return result;
      }
   }
}