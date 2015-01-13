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
      T Deserialize(XmlReader reader);
      void Serialize(XmlWriter writer, T obj);
   }
}
