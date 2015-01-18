using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Org.Edgerunner.DotSerialize.Serializers
{
   public interface ITypeSerializer
   {
      object DeserializeObject(XmlReader reader);
      void SerializeObject(XmlWriter writer, object obj);
   }
}
