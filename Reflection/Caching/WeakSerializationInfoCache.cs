using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection.Caching
{
   public class WeakSerializationInfoCache : SerializationInfoCacheBase<WeakReference<TypeInfo>>
   {
      public override void AddInfo(TypeInfo info)
      {
         _InternalCache.Add(info.DataType, new WeakReference<TypeInfo>(info));
      }

      public override TypeInfo GetInfo(Type type)
      {
         if (!_InternalCache.ContainsKey(type))
            return null;
         var reference = _InternalCache[type];
         TypeInfo result;
         if (reference.TryGetTarget(out result))
            return result;
         _InternalCache.Remove(type);
         return null;
      }
   }
}
