using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Reflection;

namespace Org.Edgerunner.DotSerialize.Reflection.Caching
{
   public class SerializationInfoCache : SerializationInfoCacheBase<TypeInfo>
   {

      public override void AddInfo(TypeInfo info)
      {
         _InternalCache.Add(info.DataType, info);
      }

      public override TypeInfo GetInfo(Type type)
      {
         return _InternalCache.ContainsKey(type) ? _InternalCache[type] : null;
      }
   }
}
