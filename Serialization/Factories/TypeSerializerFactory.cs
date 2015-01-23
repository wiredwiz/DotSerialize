using System;
using System.Collections.Generic;
using Ninject;
using Org.Edgerunner.DotSerialize.Serialization.Generic;

namespace Org.Edgerunner.DotSerialize.Serialization.Factories
{
   public class TypeSerializerFactory : ITypeSerializerFactory
   {
      protected List<Type> UnknownTypes { get; set; }

      protected IKernel Kernel { get; set; }
      /// <summary>
      /// Initializes a new instance of the <see cref="TypeSerializerFactory"/> class.
      /// </summary>
      /// <param name="kernel"></param>
      public TypeSerializerFactory(IKernel kernel)
      {
         Kernel = kernel;
         UnknownTypes = new List<Type>();
      }

      public ITypeSerializer<T> GetTypeSerializer<T>()
      {
         try
         {
            // First we check our unknown types to increase performance because Ninject resolution is expensive
            if (UnknownTypes.Contains(typeof(T)))
               return null;

            return Kernel.Get<ITypeSerializer<T>>();
         }
         catch (ActivationException)
         {
            // Since this type was not bound we add it to our unknown types to waste any future resolution time
            UnknownTypes.Add(typeof(T));
            return null;
         }
      }

      public DefaultTypeSerializer GetDefaultSerializer()
      {
         try
         {
            return Kernel.Get<DefaultTypeSerializer>();
         }
         catch (ActivationException)
         {
            return null;
         }
      }
   }
}
