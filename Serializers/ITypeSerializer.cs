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
      object Deserialize(XmlReader reader, Type type);
      void Serialize(XmlWriter writer, Type type, object obj);
   }
}
