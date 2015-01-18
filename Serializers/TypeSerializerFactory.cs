using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Org.Edgerunner.DotSerialize.Serializers.Generic;

namespace Org.Edgerunner.DotSerialize.Serializers
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
         return Kernel.Get<ITypeSerializer<T>>();
      }
   }
}
