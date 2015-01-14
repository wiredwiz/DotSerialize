using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection.Caching
{
   public abstract class SerializationInfoCacheBase<T> : ISerializationInfoCache
   {
      protected readonly Dictionary<Type, T> _InternalCache;

      /// <summary>
      /// Initializes a new instance of the <see cref="SerializationInfoCache"/> class.
      /// </summary>
      protected SerializationInfoCacheBase()
      {
         _InternalCache = new Dictionary<Type, T>();
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="SerializationInfoCache"/> class.
      /// </summary>
      /// <param name="capacity"></param>
      protected SerializationInfoCacheBase(int capacity)
      {
         _InternalCache = new Dictionary<Type, T>(capacity);
      }

      public abstract void AddInfo(TypeSerializationInfo info);
      public abstract TypeSerializationInfo GetInfo(Type type);
   }
}
