using System;
using Org.Edgerunner.DotSerialize.Serializers.Generic;

namespace Org.Edgerunner.DotSerialize.Serializers.Factories
{
   public interface ITypeSerializerFactory
   {
      ITypeSerializer<T> GetTypeSerializer<T>();
      DefaultTypeSerializer GetDefaultSerializer();
   }
}
