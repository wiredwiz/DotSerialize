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
using Org.Edgerunner.DotSerialize.Serializers;
using Org.Edgerunner.DotSerialize.Serializers.Reference;

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
         Kernel.Bind<GenericTypeSerializer>().ToSelf().InSingletonScope();
      }

      protected virtual void BindIReferenceManager()
      {
         Kernel.Bind<IReferenceManager>().To<ReferenceManager>().InScope(ctx => Scope);
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
         Scope = new object();
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
         Scope = new object();
         T result;
         IReferenceManager manager = Kernel.Get<IReferenceManager>();
         var type = TypeHelper.GetReferenceType(reader);
         if (typeof(T) != type)
            throw new SerializationException(string.Format("Serialized object in file is not of Type {0}", typeof(T).Name));
         var id = TypeHelper.GetReferenceId(reader);

         Guid refId = TypeHelper.GetReferenceId(reader);
         if (refId != Guid.Empty)
            manager.AddRerenceNode(refId, new ReferenceNode(type, null));

         // Attempt to fetch a custom type serializer
         var typeSerializer = Kernel.Get<ITypeSerializer<T>>();
         if (typeSerializer != null)
            result = typeSerializer.Deserialize(reader);
         else
         // Since there was no bound custom type serializer we default to the GenericTypeSerializer
         {
            var genericSerializer = Kernel.Get<GenericTypeSerializer>();
            result = genericSerializer.Deserialize<T>(reader);
         }
         // Now that we have our object constructed we update any refences that should point to it in our object graph
         if (refId != Guid.Empty)
            manager.GetReferenceById(refId).SourceObject = result;
         return result;
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