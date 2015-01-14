using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection.Caching
{
   public class WeakSerializationInfoCache : SerializationInfoCacheBase<WeakReference<TypeSerializationInfo>>
   {
      public override void AddInfo(TypeSerializationInfo info)
      {
         _InternalCache.Add(info.DataType, new WeakReference<TypeSerializationInfo>(info));
      }

      public override TypeSerializationInfo GetInfo(Type type)
      {
         if (!_InternalCache.ContainsKey(type))
            return null;
         var reference = _InternalCache[type];
         TypeSerializationInfo result;
         if (reference.TryGetTarget(out result))
            return result;
         _InternalCache.Remove(type);
         return null;
      }
   }
}
