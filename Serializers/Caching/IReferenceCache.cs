using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Serializers.Caching
{
   public interface IReferenceCache
   {
      object GetObjectById(Guid id);
      Guid GetIdByObject(object obj);
   }
}
