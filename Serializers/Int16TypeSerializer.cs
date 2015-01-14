using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Org.Edgerunner.DotSerialize.Serializers
{
   public class Int16TypeSerializer : ITypeSerializer<Int16>
   {
      public short Deserialize(XmlReader reader)
      {
         throw new NotImplementedException();
      }

      public void Serialize(XmlWriter writer, short obj)
      {
         throw new NotImplementedException();
      }
   }
}
