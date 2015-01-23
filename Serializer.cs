#region Apache License 2.0

// Copyright 2015 Thaddeus Ryker
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Ninject;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Reflection.Caching;
using Org.Edgerunner.DotSerialize.Serialization;
using Org.Edgerunner.DotSerialize.Serialization.Factories;
using Org.Edgerunner.DotSerialize.Serialization.Reference;

// ReSharper disable DoNotCallOverridableMethodsInConstructor

namespace Org.Edgerunner.DotSerialize
{
   public class Serializer
   {
      protected object Scope { get; set; }
      public IKernel Kernel { get; set; }
      private static Serializer _Instance;

      /// <summary>
      ///    Initializes a new instance of the <see cref="Serializer" /> class.
      /// </summary>
      /// <param name="kernel"></param>
      public Serializer(IKernel kernel)
      {
         Kernel = kernel;
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="Serializer" /> class.
      /// </summary>
      public Serializer()
      {
         Kernel = new StandardKernel();
         Kernel.Load(Assembly.GetExecutingAssembly());
         LoadDefaultBindings();
      }

      public virtual void LoadDefaultBindings()
      {
         BindITypeInspector();
         BindISerializationInfoCache();
         BindITypeSerializationFactory();
         BindGenericTypeSerializer();
         BindIReferenceManager();
      }

      protected virtual void BindITypeInspector()
      {
         Kernel.Bind<ITypeInspector>().To<TypeInspector>().InSingletonScope();
      }

      protected virtual void BindISerializationInfoCache()
      {
         Kernel.Bind<ISerializationInfoCache>().To<WeakSerializationInfoCache>();
      }

      protected virtual void BindITypeSerializationFactory()
      {
         Kernel.Bind<ITypeSerializerFactory>().ToConstant(new TypeSerializerFactory(Kernel));
      }

      protected virtual void BindGenericTypeSerializer()
      {
         Kernel.Bind<DefaultTypeSerializer>().ToSelf();
      }

      protected virtual void BindIReferenceManager()
      {
         Kernel.Bind<IReferenceManager>().To<ReferenceManager>().InThreadScope();
      }

      public static Serializer Instance
      {
         get { return _Instance ?? (_Instance = new Serializer()); }
         set { _Instance = value; }
      }

      public virtual void SerializeObject<T>(Stream stream, T obj)
      {
         using (var xmlWriter = XmlWriter.Create(stream))
         {
            SerializeObject<T>(xmlWriter, obj);
         }
      }

      public virtual void SerializeObject<T>(TextWriter writer, T obj)
      {
         using (var xmlWriter = XmlWriter.Create(writer))
         {
            SerializeObject<T>(xmlWriter, obj);
         }
      }

      public virtual void SerializeObject<T>(XmlWriter writer, T obj)
      {
         var mgr = Kernel.Get<IReferenceManager>();
         var inspector = Kernel.Get<ITypeInspector>();
         var info = inspector.GetInfo(typeof(T));
         writer.WriteStartDocument();
         writer.WriteStartElement(info.EntityName);
         if (!string.IsNullOrEmpty(info.Namespace))
            writer.WriteAttributeString("xmlns", info.Namespace);
         writer.WriteAttributeString("xmlns", "dts", null, Properties.Resources.DotserializeUri);


         // Attempt to fetch a custom type serializer
         ITypeSerializerFactory factory = Kernel.Get<ITypeSerializerFactory>();
         var typeSerializer = factory.GetTypeSerializer<T>();
         if (typeSerializer != null)
            typeSerializer.Serialize(writer, obj);
         else
         // Since there was no bound custom type serializer we default to the GenericTypeSerializer
         {
            var defaultSerializer = factory.GetDefaultSerializer();
            defaultSerializer.Serialize<T>(writer, obj);
         }
         writer.WriteEndDocument();
         Kernel.Release(mgr);
      }

      public virtual void SerializeObject<T>(out XmlDocument document, T obj)
      {
         StringWriter writer = new StringWriter();
         SerializeObject<T>(writer, obj);
         document = new XmlDocument();
         document.LoadXml(writer.ToString());
      }

      public virtual void SerializeObjectToFile<T>(string fileName, T obj)
      {
         var document = new XmlDocument();
         SerializeObject<T>(out document, obj);
         document.Save(fileName);
      }

      public virtual T DeserializeObject<T>(Stream stream)
      {
         using (var xmlReader = XmlReader.Create(stream))
         {
            return DeserializeObject<T>(xmlReader);
         }
      }

      public virtual T DeserializeObject<T>(XmlDocument document)
      {
         return DeserializeObject<T>(document.InnerXml);
      }

      public virtual T DeserializeObject<T>(TextReader reader)
      {
         using (var xmlReader = XmlReader.Create(reader))
         {
            return DeserializeObject<T>(xmlReader);
         }
      }

      public virtual T DeserializeObject<T>(XmlReader reader)
      {
         var mgr = Kernel.Get<IReferenceManager>();
         T result;
         IReferenceManager manager = Kernel.Get<IReferenceManager>();
         if (!ReadUntilElement(reader))
            throw new SerializationException("Could not find root node");
         var type = TypeHelper.GetReferenceType(reader);
         if (typeof(T) != type)
            throw new SerializationException(string.Format("Serialized object in file is not of Type {0}", typeof(T).Name));
         var id = TypeHelper.GetReferenceId(reader);
         if (id != 0)
            manager.RegisterId(id);

         // Attempt to fetch a custom type serializer
         ITypeSerializerFactory factory = Kernel.Get<ITypeSerializerFactory>();
         var typeSerializer = factory.GetTypeSerializer<T>();
         if (typeSerializer != null)
            result = typeSerializer.Deserialize(reader);
         else
         // Since there was no bound custom type serializer we default to the GenericTypeSerializer
         {
            var defaultSerializer = factory.GetDefaultSerializer();
            result = defaultSerializer.Deserialize<T>(reader);
         }

         // Now that we have our object constructed we update any refences that should point to it in our object graph
         if (id != 0)
            manager.UpdateObject(id, result);

         if (ReadUntilElement(reader))
            throw new SerializationException("Document cannot contain more than one root node");

         Kernel.Release(mgr);
         return result;
      }

      public virtual T DeserializeObject<T>(string xml)
      {
         using (var strReader = new StringReader(xml))         
         {
            return DeserializeObject<T>(strReader);
         }
      }

      public virtual T DeserializeObjectFromFile<T>(string fileName)
      {
         using (var xmlReader = XmlReader.Create(fileName))
         {
            return DeserializeObject<T>(xmlReader);
         }
      }

      protected virtual bool ReadUntilElement(XmlReader reader)
      {
         if (reader.NodeType == XmlNodeType.Element)
            return true;

         while (reader.Read())
            if (reader.NodeType == XmlNodeType.Element)
               return true;

         return false;
      }

      public static void Serialize<T>(Stream stream, T obj)
      {
         Instance.SerializeObject(stream, obj);
      }

      public static void Serialize<T>(TextWriter writer, T obj)
      {
         Instance.SerializeObject(writer, obj);
      }

      public static void Serialize<T>(XmlWriter writer, T obj)
      {
         Instance.SerializeObject(writer, obj);
      }

      public static void Serialize<T>(out XmlDocument document, T obj)
      {
         Instance.SerializeObject(out document, obj);
      }

      private static void SerializeToFile<T>(string filePath, T obj)
      {
         Instance.SerializeObjectToFile(filePath, obj);
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