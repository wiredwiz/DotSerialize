using System;
using System.Xml;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Serialization.Reference;
using Org.Edgerunner.DotSerialize;
using Org.Edgerunner.DotSerialize.Serialization.Factories;

namespace Org.Edgerunner.DotSerialize.Serialization.Generic
{
   public abstract class TypeSerializerBase<T> : ITypeSerializer<T>
   {
      protected ITypeSerializerFactory Factory { get; set; }
      protected ITypeInspector Inspector { get; set; }
      protected Settings Settings { get; set; }
      protected IReferenceManager RefManager { get; set; }


      /// <summary>
      /// Initializes a new instance of the <see cref="TypeSerializerBase"/> class.
      /// </summary>
      /// <param name="settings"></param>
      /// <param name="factory"></param>
      /// <param name="inspector"></param>
      /// <param name="refManager"></param>
      protected TypeSerializerBase(Settings settings, ITypeSerializerFactory factory, ITypeInspector inspector, IReferenceManager refManager)
      {
         Factory = factory;
         Inspector = inspector;
         Settings = settings;
         RefManager = refManager;
      }

      public virtual void Serialize(XmlWriter writer, T obj)
      {
         Serialize(writer, typeof(T), obj);
      }

      public virtual object Deserialize(XmlReader reader, Type type)
      {
         throw new NotImplementedException();
      }

      public virtual void Serialize(XmlWriter writer, Type type, object obj)
      {
         throw new NotImplementedException();
      }

      public virtual T Deserialize(XmlReader reader)
      {
         return (T)Deserialize(reader, typeof(T));
      }
   }
}
