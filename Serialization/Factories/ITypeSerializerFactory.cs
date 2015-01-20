using System;
using Org.Edgerunner.DotSerialize.Serialization.Generic;

namespace Org.Edgerunner.DotSerialize.Serialization.Factories
{
   public interface ITypeSerializerFactory
   {
      ITypeSerializer<T> GetTypeSerializer<T>();
      DefaultTypeSerializer GetDefaultSerializer();
   }
}
