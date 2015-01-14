using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection.Caching
{
   public interface ISerializationInfoCache
   {
      void AddInfo(TypeSerializationInfo info);
      TypeSerializationInfo GetInfo(Type type);
   }
}
