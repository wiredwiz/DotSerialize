﻿using System;
using System.Xml;

namespace Org.Edgerunner.DotSerialize.Serializers.Generic
{
   public interface ITypeSerializer<T> : ITypeSerializer
   {
      new T Deserialize(XmlReader reader);
      void Serialize(XmlWriter writer, T obj);
   }
}
