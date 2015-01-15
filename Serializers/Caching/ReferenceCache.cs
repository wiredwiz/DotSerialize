using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Serializers.Caching
{
   public class ReferenceCache : IReferenceCache
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ReferenceCache"/> class.
      /// </summary>
      public ReferenceCache()
      {
         ReferencesByGuid = new Dictionary<Guid, object>();
         ReferencesByInstance = new Dictionary<object, Guid>();
      }

      protected Dictionary<Guid, object> ReferencesByGuid { get; set; }
      protected Dictionary<object, Guid> ReferencesByInstance { get; set; }

      public virtual object GetObjectById(Guid id)
      {
         if (!ReferencesByGuid.ContainsKey(id))
            return null;
         return ReferencesByGuid[id];
      }

      public virtual Guid GetIdByObject(object obj)
      {
         if (!ReferencesByInstance.ContainsKey(obj))
            return Guid.Empty;
         return ReferencesByInstance[obj];
      }
   }
}
