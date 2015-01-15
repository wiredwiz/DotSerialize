using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Ninject;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Serializers.Caching;
using Org.Edgerunner.DotSerialize.Reflection.Caching;
using Org.Edgerunner.DotSerialize.Serializers;

namespace Org.Edgerunner.DotSerialize
{
   public class Serializer
   {
      public IKernel Kernel { get; set; }
      private static Serializer _Instance;

      /// <summary>
      /// Initializes a new instance of the <see cref="Serializer"/> class.
      /// </summary>
      /// <param name="kernel"></param>
      public Serializer(IKernel kernel)
      {
         Kernel = kernel;
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="Serializer"/> class.
      /// </summary>
      public Serializer()
      {
         Kernel = new StandardKernel();
         Kernel.Load(Assembly.GetExecutingAssembly());
         // TODO: consider making a custom scope for the reference cache
         Kernel.Bind<IReferenceCache>().To<ReferenceCache>().InThreadScope();
         Kernel.Bind<ISerializationInfoCache>().To<WeakSerializationInfoCache>().InSingletonScope();
         Kernel.Bind<ITypeInspector>().To<TypeInspector>().InSingletonScope();
         Kernel.Bind<GenericTypeSerializer>().ToSelf().InSingletonScope();
      }

      public static Serializer Instance
      {
         get { return _Instance ?? (_Instance = new Serializer()); }
         set { _Instance = value; }
      }

      protected virtual void SerializeObject<T>(Stream stream, T obj)
      {
         throw new NotImplementedException();
      }

      protected virtual void SerializeObject<T>(TextWriter writer, T obj)
      {
         throw new NotImplementedException();
      }

      protected virtual void SerializeObject<T>(XmlWriter writer, T obj)
      {
         throw new NotImplementedException();
      }

      protected virtual void SerializeObject<T>(out XmlDocument document, T obj)
      {
         throw new NotImplementedException();
      }

      protected virtual void SerializeObjectToFile<T>(string filePath, T obj)
      {
         throw new NotImplementedException();
      }

      protected virtual T DeserializeObject<T>(Stream stream)
      {
         throw new NotImplementedException();
      }

      protected virtual T DeserializeObject<T>(XmlDocument document)
      {
         throw new NotImplementedException();
      }

      protected virtual T DeserializeObject<T>(TextReader reader)
      {
         throw new NotImplementedException();
      }

      protected virtual T DeserializeObject<T>(XmlReader reader)
      {
         Kernel.Release(Kernel.Get<IReferenceCache>());
         throw new NotImplementedException();
      }

      protected virtual T DeserializeObject<T>(string xml)
      {
         throw new NotImplementedException();
      }

      protected virtual T DeserializeObjectFromFile<T>(string filePath)
      {
         throw new NotImplementedException();
      }


      public static void Serialize<T>(Stream stream, T obj)
      {
         Instance.SerializeObject<T>(stream, obj);
      }

      public static void Serialize<T>(TextWriter writer, T obj)
      {
         Instance.SerializeObject<T>(writer, obj);
      }

      public static void Serialize<T>(XmlWriter writer, T obj)
      {
         Instance.SerializeObject<T>(writer, obj);
      }

      public static void Serialize<T>(out XmlDocument document, T obj)
      {
         Instance.SerializeObject<T>(out document, obj);
      }

      private static void SerializeToFile<T>(string filePath, T obj)
      {
         Instance.SerializeObjectToFile<T>(filePath, obj);
      }

      public static T Deserialize<T>(Stream stream)
      {
         return Instance.DeserializeObject<T>(stream);
      }

      public static T Deserialize<T>(XmlDocument document)
      {
         return Instance.DeserializeObject<T>(document);
      }

      public static T Deserialize<T>(TextReader reader)
      {
         return Instance.DeserializeObject<T>(reader);
      }

      public static T Deserialize<T>(XmlReader reader)
      {
         return Instance.DeserializeObject<T>(reader);
      }

      public static T Deserialize<T>(string xml)
      {
         return Instance.DeserializeObject<T>(xml);
      }

      public static T DeserializeFromFile<T>(string filePath)
      {
         return Instance.DeserializeObjectFromFile<T>(filePath);
      }
   }
}
