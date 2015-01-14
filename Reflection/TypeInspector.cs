using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public class TypeInspector
   {
      public TypeSerializationInfo GetInfo(string fullyQualifiedTypeName)
      {
         return GetInfo(Type.GetType(fullyQualifiedTypeName, true));
      }

      public TypeSerializationInfo GetInfo(Type type)
      {
         string rootName = null;
         string @namespace = null;
         var rootAttrib = type.Attribute<Attributes.XmlRootAttribute>();
         if (rootAttrib != null)
         {
            rootName = rootAttrib.GetPropertyValue("name").ToString();
            @namespace = rootAttrib.GetPropertyValue("namespace").ToString();
         }
         var fieldInfo = type.Fields(Flags.InstanceAnyVisibility | Flags.ExcludeExplicitlyImplemented);
         foreach (var field in fieldInfo)
         {
            
         }
         var typeInfo = new TypeSerializationInfo(type.Name, type, rootName, @namespace);
         return null;
      }
   }
}
