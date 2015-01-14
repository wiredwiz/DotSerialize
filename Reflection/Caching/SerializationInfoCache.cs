using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Reflection;

namespace Org.Edgerunner.DotSerialize.Reflection.Caching
{
   public class SerializationInfoCache : SerializationInfoCacheBase<TypeSerializationInfo>
   {

      public override void AddInfo(TypeSerializationInfo info)
      {
         _InternalCache.Add(info.DataType, info);
      }

      public override TypeSerializationInfo GetInfo(Type type)
      {
         return _InternalCache.ContainsKey(type) ? _InternalCache[type] : null;
      }
   }
}
