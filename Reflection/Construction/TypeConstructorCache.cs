using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;

namespace Org.Edgerunner.DotSerialize.Reflection.Construction
{
   public class TypeConstructorCache
   {
      protected Dictionary<Type, Dictionary<int, ConstructorMap>> _Mappings;

      /// <summary>
      /// Initializes a new instance of the <see cref="TypeConstructorCache"/> class.
      /// </summary>
      public TypeConstructorCache()
      {
         _Mappings = new Dictionary<Type, Dictionary<int, ConstructorMap>>();
      }

      public ConstructorMap GetMappingFor(Type type, List<TypeMemberSerializationInfo> info)
      {
         if (type == null) throw new ArgumentNullException("type");
         if (info == null) throw new ArgumentNullException("info");

         if (!_Mappings.ContainsKey(type))
            return null;

         var typeMappings = _Mappings[type];
         int key = GetHash(info);
         if (!typeMappings.ContainsKey(key))
            return null;
         return typeMappings[key];
      }

      public void AddMappingFor(Type type, List<TypeMemberSerializationInfo> info, ConstructorMap constructorMap)
      {
         if (type == null) throw new ArgumentNullException("type");
         if (info == null) throw new ArgumentNullException("info");

         if (!_Mappings.ContainsKey(type))
            _Mappings[type] = new Dictionary<int, ConstructorMap>();

         _Mappings[type].Add(GetHash(info), constructorMap);            
      }

      protected int GetHash(List<TypeMemberSerializationInfo> info)
      {
         if (info.Count == 0)
            return 0;
         else if (info.Count == 1)
            return info[0].GetHashCode();
         else
         {
            info.Sort((x, y) => x.Name.CompareTo(y));
            int result = info[0].GetHashCode();
            for (int i = 1; i < info.Count; i++)
               result = (result * 397) ^ info[i].GetHashCode();
            return result;
         }
      }
   }
}
