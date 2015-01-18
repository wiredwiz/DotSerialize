using System;
using System.Xml;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Serializers.Reference;

namespace Org.Edgerunner.DotSerialize.Serializers.Generic
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

      public virtual void Serialize(XmlWriter writer, T obj)
      {
         SerializeObject(writer, obj);
      }

      public virtual object DeserializeObject(XmlReader reader)
      {
         throw new NotImplementedException();
      }

      public virtual void SerializeObject(XmlWriter writer, object obj)
      {
         throw new NotImplementedException();
      }

      public virtual T Deserialize(XmlReader reader)
      {
         return (T)DeserializeObject(reader);
      }
   }
}
