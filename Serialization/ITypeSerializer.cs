using System;
using System.Xml;

namespace Org.Edgerunner.DotSerialize.Serialization
{
   public interface ITypeSerializer
   {
      object Deserialize(XmlReader reader, Type type);
      void Serialize(XmlWriter writer, Type type, object obj);
   }
}
