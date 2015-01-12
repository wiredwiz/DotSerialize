using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Org.Edgerunner.DotSerialize.Serializers
{
   public interface ITypeSerializer<T>
   {
      T Deserialize(string xml);
      T Deserialize(XmlDocument document);
      T Deserialize(Stream stream);
      T Deserialize(TextReader reader);
      T Deserialize(XmlReader reader);
      void Serialize(Stream stream, T obj);
      void Serialize(TextWriter writer, T obj);
      void Serialize(XmlWriter writer, T obj);
   }
}
