using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Org.Edgerunner.DotSerialize;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Serializers.Reference;

namespace Org.Edgerunner.DotSerialize.Serializers
{
   public abstract class TypeSerializerBase<T> : ITypeSerializer<T>
   {
      protected ITypeInspector Inspector { get; set; }
      protected IReferenceManager ReferenceCache { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="TypeSerializerBase"/> class.
      /// </summary>
      /// <param name="inspector"></param>
      /// <param name="referenceCache"></param>
      protected TypeSerializerBase(ITypeInspector inspector, IReferenceManager referenceCache)
      {
         Inspector = inspector;
         ReferenceCache = referenceCache;
      }
       
      public abstract T Deserialize(XmlReader reader);
      public abstract void Serialize(XmlWriter writer, T obj);
   }
}
