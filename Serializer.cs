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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Ninject;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Properties;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Reflection.Caching;
using Org.Edgerunner.DotSerialize.Serialization;
using Org.Edgerunner.DotSerialize.Serialization.Factories;
using Org.Edgerunner.DotSerialize.Serialization.Generic;
using Org.Edgerunner.DotSerialize.Serialization.Reference;
using Org.Edgerunner.DotSerialize.Serialization.Registration;

// ReSharper disable DoNotCallOverridableMethodsInConstructor

namespace Org.Edgerunner.DotSerialize
{
   /// <summary>
   /// Class used to serialize and deserialize data.
   /// </summary>
   public class Serializer
   {
      /// <summary>
      /// The current Serializer instance to use in static method calls.
      /// </summary>
      private static Serializer _Instance;
      /// <summary>
      /// Gets or sets the Settings instance to use with the Serializer.
      /// </summary>
      /// <value>The settings.</value>
      public Settings Settings { get; set; }
      /// <summary>
      /// Gets or sets the current Ninject kernel.
      /// </summary>
      /// <value>The kernel.</value>
      protected IKernel Kernel { get; set; }
      /// <summary>
      /// Gets or sets the registered custom type serializers.
      /// </summary>
      /// <value>The registered custom type serializers.</value>
      protected IList<Type> RegisteredTypeSerializers { get; set; }

      /// <summary>
      /// The current Serializer instance to use in static method calls.
      /// </summary>
      /// <value>The instance.</value>
      public static Serializer Instance
      {
         get { return _Instance ?? (_Instance = new Serializer()); }
         set { _Instance = value; }
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Serializer" /> class.
      /// </summary>
      /// <param name="settings">The Settings to use.</param>
      public Serializer(Settings settings)
      {
         RegisteredTypeSerializers = new List<Type>();
         Settings = settings;
         LoadDefaultKernel();
         BindSettings();
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Serializer" /> class.
      /// </summary>
      public Serializer()
      {
         RegisteredTypeSerializers = new List<Type>();
         Settings = Settings.Default;
         LoadDefaultKernel();
         BindSettings();
      }

      /// <summary>
      /// Clears the custom type serializer registrations.
      /// </summary>
      public void ClearTypeSerializerRegistrations()
      {
         foreach (Type item in RegisteredTypeSerializers)
            Kernel.Unbind(item);
         RegisteredTypeSerializers.Clear();
         BindITypeSerializationFactory();
      }

      /// <summary>
      /// Deserializes an object from a Stream and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="stream">The Stream to read from.</param>
      /// <returns>Object of Type T.</returns>
      /// <example>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="C#">
      /// var serializer = new Serializer();
      /// var user;
      /// using (FileStream stream = File.Open(@"C:\user.xml", FileMode.Open))
      ///   user = serializer.DeserializeObject&lt;User&gt;(stream);</code>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="VB">
      /// Dim serializer = New Serializer()
      /// Dim user
      /// Using stream As FileStream = File.Open("C:\user.xml", FileMode.Open)
      ///     user = serializer.DeserializeObject(Of User)(stream)
      /// End Using</code>
      /// </example>
      public virtual T DeserializeObject<T>(Stream stream)
      {
         using (var xmlReader = XmlReader.Create(stream))
            return DeserializeObject<T>(xmlReader);
      }

      /// <summary>
      /// Deserializes an object from an XmlDocument and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="document">The XmlDocument to read from.</param>
      /// <returns>Object of Type T.</returns>
      /// <example>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="C#">
      /// var serializer = new Serializer();
      /// var user = serializer.DeserializeObject&lt;User&gt;(xmlDoc);</code>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="VB">
      /// Dim serializer = New Serializer()
      /// Dim user = serializer.DeserializeObject(Of User)(xmlDoc)</code>
      /// </example>
      public virtual T DeserializeObject<T>(XmlDocument document)
      {
         return DeserializeObject<T>(document.InnerXml);
      }

      /// <summary>
      /// Deserializes an object from a TextReader and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="reader">The TextReader to read from.</param>
      /// <returns>Object of Type T.</returns>
      /// <example>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="C#">
      /// var serializer = new Serializer();
      /// var user = serializer.DeserializeObject&lt;User&gt;(reader);</code>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="VB">
      /// Dim serializer = New Serializer()
      /// Dim user = serializer.DeserializeObject(Of User)(reader)</code>
      /// </example>
      public virtual T DeserializeObject<T>(TextReader reader)
      {
         using (var xmlReader = XmlReader.Create(reader))
            return DeserializeObject<T>(xmlReader);
      }

      /// <summary>
      /// Deserializes an object from a XmlReader and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="reader">The XmlReader to read from.</param>
      /// <returns>Object of Type T.</returns>
      /// <exception caption="" cref="SerializerException">Thrown if the root node cannot be found or if there is more than one root node.</exception>
      /// <example>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="C#">
      /// var serializer = new Serializer();
      /// var user = serializer.DeserializeObject&lt;User&gt;(reader);</code>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="VB">
      /// Dim serializer = New Serializer()
      /// Dim user = serializer.DeserializeObject(Of User)(reader)</code>
      /// </example>
      public virtual T DeserializeObject<T>(XmlReader reader)
      {
         var mgr = Kernel.Get<IReferenceManager>();
         T result;
         IReferenceManager manager = Kernel.Get<IReferenceManager>();
         if (!ReadUntilElement(reader))
            throw new SerializerException("Could not find root node");
         var type = TypeHelper.GetReferenceType(reader);
         if (type != null)
            if (typeof(T) != type)
               throw new SerializerException(string.Format("Serialized object in file is not of Type {0}", typeof(T).Name));
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
            throw new SerializerException("Document cannot contain more than one root node");

         Kernel.Release(mgr);
         BindITypeSerializationFactory();
         return result;
      }

      /// <summary>
      /// Deserializes an object from a string and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="xml">The string containing valid XML.</param>
      /// <returns>Object of Type T.</returns>
      /// <example>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="C#">
      /// var serializer = new Serializer();
      /// var user = serializer.DeserializeObject&lt;User&gt;(someXmlString);</code>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="VB">
      /// Dim serializer = New Serializer()
      /// Dim user = serializer.DeserializeObject(Of User)(someXmlString)</code>
      /// </example>
      public virtual T DeserializeObject<T>(string xml)
      {
         using (var strReader = new StringReader(xml))
            return DeserializeObject<T>(strReader);
      }

      /// <summary>
      /// Deserializes an object from a file and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="fileName">Name of the file to read.</param>
      /// <returns>Object of Type T.</returns>
      /// <example>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="C#">
      /// var serializer = new Serializer();
      /// var user = serializer.DeserializeObjectFromFile&lt;User&gt;(@"C:\user.xml");</code>
      /// 	<code title="Deserializing a user object" description="" groupname="DeserializeFromFile" lang="VB">
      /// Dim serializer = New Serializer()
      /// Dim user = serializer.DeserializeObjectFromFile(Of User)("C:\user.xml")</code>
      /// </example>
      public virtual T DeserializeObjectFromFile<T>(string fileName)
      {
         using (var xmlReader = XmlReader.Create(fileName))
            return DeserializeObject<T>(xmlReader);
      }

      /// <summary>
      /// Loads the default Ninject bindings.
      /// </summary>
      public virtual void LoadDefaultBindings()
      {
         BindITypeInspector();
         BindISerializationInfoCache();
         BindITypeSerializationFactory();
         BindDefaultTypeSerializer();
         BindIReferenceManager();
      }

      /// <summary>
      /// Registers a given Type to be handled by a custom type serializer.
      /// </summary>
      /// <typeparam name="T">The Type to be handled</typeparam>
      /// <returns>A Registrar&lt;T&gt; instance.</returns>
      public Registrar<T> RegisterType<T>()
      {
         return new Registrar<T>(this);
      }

      /// <summary>
      /// Serializes an object and writes it to the supplied Stream.
      /// </summary>
      /// <typeparam name="T">The Type being Serialized</typeparam>
      /// <param name="stream">The Stream to write to.</param>
      /// <param name="obj">The object to serialize.</param>
      public virtual void SerializeObject<T>(Stream stream, T obj)
      {
         using (var xmlWriter = XmlWriter.Create(stream))
            SerializeObject(xmlWriter, obj);
      }

      /// <summary>
      /// Serializes an object and writes it to the supplied TextWriter.
      /// </summary>
      /// <typeparam name="T">The Type being Serialized</typeparam>
      /// <param name="writer">The TextWriter to write to.</param>
      /// <param name="obj">The object to serialize.</param>
      public virtual void SerializeObject<T>(TextWriter writer, T obj)
      {
         using (var xmlWriter = XmlWriter.Create(writer))
            SerializeObject(xmlWriter, obj);
      }

      /// <summary>
      /// Serializes an object and writes it to the supplied XmlWriter.
      /// </summary>
      /// <typeparam name="T">The Type being Serialized</typeparam>
      /// <param name="writer">The XmlWriter to write to.</param>
      /// <param name="obj">The object to serialize.</param>
      public virtual void SerializeObject<T>(XmlWriter writer, T obj)
      {
         var mgr = Kernel.Get<IReferenceManager>();
         var inspector = Kernel.Get<ITypeInspector>();
         var info = inspector.GetInfo(typeof(T));
         writer.WriteStartDocument();
         writer.WriteStartElement(info.EntityName);
         if (!string.IsNullOrEmpty(info.Namespace))
            writer.WriteAttributeString("xmlns", info.Namespace);
         // ReSharper disable once AssignNullToNotNullAttribute
         writer.WriteAttributeString("xmlns", "dts", null, Resources.DotserializeUri);
         // ReSharper disable once AssignNullToNotNullAttribute
         writer.WriteAttributeString("xmlns", "xsi", null, Resources.XsiUri);

         try
         {
            // Attempt to fetch a custom type serializer
            ITypeSerializerFactory factory = Kernel.Get<ITypeSerializerFactory>();
            var typeSerializer = factory.GetTypeSerializer<T>();
            if (typeSerializer != null)
               typeSerializer.Serialize(writer, obj);
            else
            // Since there was no bound custom type serializer we default to the GenericTypeSerializer
            {
               var defaultSerializer = factory.GetDefaultSerializer();
               defaultSerializer.Serialize(writer, obj);
            }
         }
         catch (StackOverflowException ex)
         {
            if (Settings.DisableReferentialIntegrity)
               throw new SerializerException("Non-ending recursive loop encountered during serialization.\n" +
                                             "This is likely due to a circular reference in the object graph, try enabling the referential integrity setting.",
                                             ex);

            throw;
         }
         writer.WriteEndDocument();
         Kernel.Release(mgr);
         BindITypeSerializationFactory();
      }

      /// <summary>
      /// Serializes an object and writes it to the supplied XmlDocument.
      /// </summary>
      /// <typeparam name="T">The Type being Serialized</typeparam>
      /// <param name="document">The XmlDocument to write to.</param>
      /// <param name="obj">The object to serialize.</param>
      public virtual void SerializeObject<T>(out XmlDocument document, T obj)
      {
         StringWriter writer = new StringWriter();
         SerializeObject(writer, obj);
         document = new XmlDocument();
         document.LoadXml(writer.ToString());
      }

      /// <summary>
      /// Serializes an object and writes it to a file with the specified file name.
      /// </summary>
      /// <typeparam name="T">The type being Serialized</typeparam>
      /// <param name="fileName">Name of the file to write to.</param>
      /// <param name="obj">The object to serialize.</param>
      public virtual void SerializeObjectToFile<T>(string fileName, T obj)
      {
         XmlDocument document;
         SerializeObject(out document, obj);
         document.Save(fileName);
      }

      /// <summary>
      /// Unregisters the custom ITypeSerializer linked to Type.
      /// </summary>
      /// <typeparam name="T">The Type to unregister the customer ITypeSerializer for.</typeparam>
      public virtual void UnRegisterType<T>()
      {
         var type = typeof(ITypeSerializer<T>);
         if (RegisteredTypeSerializers.Contains(type))
         {            
            Kernel.Unbind(type);
            RegisteredTypeSerializers.Remove(type);
            BindITypeSerializationFactory();
         }
      }

      /// <summary>
      /// Binds the default type serializer implementation in the current Ninject Kernel.
      /// </summary>
      protected virtual void BindDefaultTypeSerializer()
      {
         Kernel.Bind<DefaultTypeSerializer>().ToSelf();
      }

      /// <summary>
      /// Binds the IReferenceManager implementation in the current Ninject Kernel.
      /// </summary>
      protected virtual void BindIReferenceManager()
      {
         Kernel.Bind<IReferenceManager>().To<ReferenceManager>().InThreadScope();
      }

      /// <summary>
      /// Binds the ISerializationInformationCache implementation in the current Ninject Kernel.
      /// </summary>
      protected virtual void BindISerializationInfoCache()
      {
         Kernel.Bind<ISerializationInfoCache>().To<WeakSerializationInfoCache>();
      }

      /// <summary>
      /// Binds the ITypeInspector implementation in the current Ninject Kernel.
      /// </summary>
      protected virtual void BindITypeInspector()
      {
         Kernel.Bind<ITypeInspector>().To<TypeInspector>().InSingletonScope();
      }

      /// <summary>
      /// Binds the ITypeSerializationFactory implementation in the current Ninject Kernel.
      /// </summary>
      protected virtual void BindITypeSerializationFactory()
      {
         Kernel.Rebind<ITypeSerializerFactory>().ToConstant(new TypeSerializerFactory(Kernel, RegisteredTypeSerializers));
      }

      /// <summary>
      /// Binds the Settings in the current Ninject Kernel.
      /// </summary>
      protected virtual void BindSettings()
      {
         Kernel.Bind<Settings>().ToConstant(Settings);
      }

      /// <summary>
      /// Loads the default Ninject Kernel.
      /// </summary>
      protected virtual void LoadDefaultKernel()
      {
         Kernel = new StandardKernel();
         Kernel.Load(Assembly.GetExecutingAssembly());
         LoadDefaultBindings();
      }

      /// <summary>
      /// Reads from the XmlReader until an element node is encountered.
      /// </summary>
      /// <param name="reader">The XmlReader to read from.</param>
      /// <returns><c>true</c> if the reader locates an element node, <c>false</c> otherwise.</returns>
      /// <remarks>If the reader is already positioned on an element node, the method returns.</remarks>
      protected virtual bool ReadUntilElement(XmlReader reader)
      {
         if (reader.NodeType == XmlNodeType.Element)
            return true;

         while (reader.Read())
            if (reader.NodeType == XmlNodeType.Element)
               return true;

         return false;
      }

      /// <summary>
      /// Registers a specified ITypeSerializer to a custom type serializer implementation.
      /// </summary>
      /// <param name="sourceInterface">The ITypeSerializer to register for.</param>
      /// <param name="implementation">The ITypeSerializer implementation to register against.</param>
      /// <param name="constructorArguments">Constructor arguments to be passed into new instances of the custom type serializer.</param>
      internal void RegisterTypeSerializer(Type sourceInterface, Type implementation, params object[] constructorArguments)
      {
         var bindingSyntax = Kernel.Rebind(sourceInterface).To(implementation);
         foreach (object arg in constructorArguments)
            bindingSyntax.WithConstructorArgument(arg.GetType(), arg);
         if (!RegisteredTypeSerializers.Contains(sourceInterface))
            RegisteredTypeSerializers.Add(sourceInterface);
         BindITypeSerializationFactory();
      }

      #region Static Methods

      /// <summary>
      /// Deserializes an object from a Stream and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="stream">The Stream to read from.</param>
      /// <returns>Object of Type T.</returns>
      /// <remarks>Calls the DeserializeObject method with the same signature on the Serializer instance stored in property Instance.</remarks>
      public static T Deserialize<T>(Stream stream)
      {
         return Instance.DeserializeObject<T>(stream);
      }

      /// <summary>
      /// Deserializes an object from an XmlDocument and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="document">The XmlDocument to read from.</param>
      /// <returns>Object of Type T.</returns>
      /// <remarks>Calls the DeserializeObject method with the same signature on the Serializer instance stored in property Instance.</remarks>
      public static T Deserialize<T>(XmlDocument document)
      {
         return Instance.DeserializeObject<T>(document);
      }

      /// <summary>
      /// Deserializes an object from a TextReader and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="reader">The TextReader to read from.</param>
      /// <returns>Object of Type T.</returns>
      /// <remarks>Calls the DeserializeObject method with the same signature on the Serializer instance stored in property Instance.</remarks>
      public static T Deserialize<T>(TextReader reader)
      {
         return Instance.DeserializeObject<T>(reader);
      }

      /// <summary>
      /// Deserializes an object from a XmlReader and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="reader">The XmlReader to read from.</param>
      /// <returns>Object of Type T.</returns>
      /// <remarks>Calls the DeserializeObject method with the same signature on the Serializer instance stored in property Instance.</remarks>
      /// <exception caption="" cref="SerializerException">Thrown if the root node cannot be found or if there is more than one root node.</exception>
      public static T Deserialize<T>(XmlReader reader)
      {
         return Instance.DeserializeObject<T>(reader);
      }

      /// <summary>
      /// Deserializes an object from a string and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="xml">The string containing valid XML.</param>
      /// <returns>Object of Type T.</returns>
      /// <remarks>Calls the DeserializeObject method with the same signature on the Serializer instance stored in property Instance.</remarks>
      public static T Deserialize<T>(string xml)
      {
         return Instance.DeserializeObject<T>(xml);
      }

      /// <summary>
      /// Deserializes an object from a file and returns it as Type T.
      /// </summary>
      /// <typeparam name="T">The Type being deserialized</typeparam>
      /// <param name="fileName">Name of the file to read.</param>
      /// <remarks>Calls the DeserializeObjectFromFile method on the Serializer instance stored in property Instance.</remarks>
      /// <returns>Object of Type T.</returns>
      /// <seealso cref="DeserializeObjectFromFile{T}"></seealso>
      public static T DeserializeFromFile<T>(string fileName)
      {
         return Instance.DeserializeObjectFromFile<T>(fileName);
      }

      /// <summary>
      /// Serializes an object and writes it to the supplied Stream.
      /// </summary>
      /// <typeparam name="T">The Type being Serialized</typeparam>
      /// <param name="stream">The Stream to write to.</param>
      /// <param name="obj">The object to serialize.</param>
      /// <remarks>Calls the SerializeObject method with the same signature on the Serializer instance stored in property Instance.</remarks>
      public static void Serialize<T>(Stream stream, T obj)
      {
         Instance.SerializeObject(stream, obj);
      }

      /// <summary>
      /// Serializes an object and writes it to the supplied TextWriter.
      /// </summary>
      /// <typeparam name="T">The Type being Serialized</typeparam>
      /// <param name="writer">The TextWriter to write to.</param>
      /// <param name="obj">The object to serialize.</param>
      /// <remarks>Calls the SerializeObject method with the same signature on the Serializer instance stored in property Instance.</remarks>
      public static void Serialize<T>(TextWriter writer, T obj)
      {
         Instance.SerializeObject(writer, obj);
      }

      /// <summary>
      /// Serializes an object and writes it to the supplied XmlWriter.
      /// </summary>
      /// <typeparam name="T">The Type being Serialized</typeparam>
      /// <param name="writer">The XmlWriter to write to.</param>
      /// <param name="obj">The object to serialize.</param>
      /// <remarks>Calls the SerializeObject method with the same signature on the Serializer instance stored in property Instance.</remarks>
      public static void Serialize<T>(XmlWriter writer, T obj)
      {
         Instance.SerializeObject(writer, obj);
      }

      /// <summary>
      /// Serializes an object and writes it to the supplied XmlDocument.
      /// </summary>
      /// <typeparam name="T">The Type being Serialized</typeparam>
      /// <param name="document">The XmlDocument to write to.</param>
      /// <param name="obj">The object to serialize.</param>
      /// <remarks>Calls the SerializeObject method with the same signature on the Serializer instance stored in property Instance.</remarks>
      public static void Serialize<T>(out XmlDocument document, T obj)
      {
         Instance.SerializeObject(out document, obj);
      }

      /// <summary>
      /// Serializes an object and writes it to a file with the specified file name.
      /// </summary>
      /// <typeparam name="T">The type being Serialized</typeparam>
      /// <param name="fileName">Name of the file to write to.</param>
      /// <param name="obj">The object to serialize.</param>
      /// <remarks>Calls the SerializeObjectToFile method on the Serializer instance stored in property Instance.</remarks>
      public static void SerializeToFile<T>(string fileName, T obj)
      {
         Instance.SerializeObjectToFile(fileName, obj);
      }

      #endregion
   }
}