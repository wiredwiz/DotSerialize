using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Org.Edgerunner.DotSerialize
{
   public class Serializer
   {
      private static Serializer _Instance;
      public static Serializer Instance
      {
         get { return _Instance ?? (_Instance = CreateSerializer()); }
         set { _Instance = value; }
      }

      private static Serializer CreateSerializer()
      {
         throw new NotImplementedException();
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
