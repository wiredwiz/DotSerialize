using System;
using System.Collections.Generic;
using Ninject;
using Org.Edgerunner.DotSerialize.Serialization.Generic;
using Org.Edgerunner.DotSerialize.Serialization;

namespace Org.Edgerunner.DotSerialize.Serialization.Factories
{
   public class TypeSerializerFactory : ITypeSerializerFactory
   {
      protected IKernel Kernel { get; set; }
      public IList<Type> CustomerSerializerTypes { get; set; }
      protected Dictionary<Type, ITypeSerializer> SerializerInstances { get; set; }
      protected DefaultTypeSerializer DefaultSerializer { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="TypeSerializerFactory"/> class.
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

      public ITypeSerializer<T> GetTypeSerializer<T>()
      {
         Type type = typeof(ITypeSerializer<T>);
         if (!CustomerSerializerTypes.Contains(type))
            return null;

         if (!SerializerInstances.ContainsKey(type))
            SerializerInstances[type] = Kernel.Get<ITypeSerializer<T>>();
            
         return SerializerInstances[type] as ITypeSerializer<T>;

      }

      public DefaultTypeSerializer GetDefaultSerializer()
      {
         return DefaultSerializer ?? (DefaultSerializer = Kernel.Get<DefaultTypeSerializer>());
      }
   }
}
