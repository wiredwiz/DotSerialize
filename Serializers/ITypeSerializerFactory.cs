using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Org.Edgerunner.DotSerialize.Serializers
{
   public interface ITypeSerializerFactory
   {
      ITypeSerializer<T> GetTypeSerializer<T>();
   }
}
