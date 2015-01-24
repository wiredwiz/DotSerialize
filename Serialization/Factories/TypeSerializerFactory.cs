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
using Ninject;
using Org.Edgerunner.DotSerialize.Serialization.Generic;

namespace Org.Edgerunner.DotSerialize.Serialization.Factories
{
   public class TypeSerializerFactory : ITypeSerializerFactory
   {
      protected IKernel Kernel { get; set; }
      public IList<Type> CustomerSerializerTypes { get; set; }
      protected Dictionary<Type, ITypeSerializer> SerializerInstances { get; set; }
      protected DefaultTypeSerializer DefaultSerializer { get; set; }

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeSerializerFactory" /> class.
      /// </summary>
      /// <param name="kernel"></param>
      /// <param name="customerSerializerTypes"></param>
      public TypeSerializerFactory(IKernel kernel, IList<Type> customerSerializerTypes)
      {
         Kernel = kernel;
         CustomerSerializerTypes = customerSerializerTypes;
         SerializerInstances = new Dictionary<Type, ITypeSerializer>();
         DefaultSerializer = null;
      }

      #region ITypeSerializerFactory Members

      public DefaultTypeSerializer GetDefaultSerializer()
      {
         return DefaultSerializer ?? (DefaultSerializer = Kernel.Get<DefaultTypeSerializer>());
      }

      public ITypeSerializer<T> GetTypeSerializer<T>()
      {
         Type type = typeof(ITypeSerializer<T>);
         if (!CustomerSerializerTypes.Contains(type))
            return null;

         if (!SerializerInstances.ContainsKey(type))
            SerializerInstances[type] = Kernel.Get<ITypeSerializer<T>>();

         return SerializerInstances[type] as ITypeSerializer<T>;
      }

      #endregion
   }
}