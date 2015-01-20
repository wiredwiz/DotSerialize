using System;
using Ninject;
using Org.Edgerunner.DotSerialize.Serialization.Generic;

namespace Org.Edgerunner.DotSerialize.Serialization.Factories
{
   public class TypeSerializerFactory : ITypeSerializerFactory
   {
      protected IKernel Kernel { get; set; }
      /// <summary>
      /// Initializes a new instance of the <see cref="TypeSerializerFactory"/> class.
      /// </summary>
      /// <param name="kernel"></param>
      public TypeSerializerFactory(IKernel kernel)
      {
         Kernel = kernel;
      }

      public ITypeSerializer<T> GetTypeSerializer<T>()
      {
         try
         {
            return Kernel.Get<ITypeSerializer<T>>();
         }
         catch (ActivationException)
         {
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
