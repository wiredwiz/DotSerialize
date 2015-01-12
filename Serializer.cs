using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Org.Edgerunner.DotSerialize
{
   public static class Serializer
   {
      public static void Serialize<T>(Stream stream, T obj)
      {
         throw new NotImplementedException();
      }

      public static void Serialize<T>(TextWriter writer, T obj)
      {
         throw new NotImplementedException();
      }

      public static void Serialize<T>(XmlWriter writer, T obj)
      {
         throw new NotImplementedException();
      }

      public static void Serialize<T>(out XmlDocument document, T obj)
      {
         throw new NotImplementedException();
      }

      private static void SerializeToFile<T>(string filePath, T obj)
      {
         throw new NotImplementedException();
      }

      public static T Deserialize<T>(Stream stream)
      {
         throw new NotImplementedException();
      }

      public static T Deserialize<T>(XmlDocument document)
      {
         throw new NotImplementedException();
      }

      public static T Deserialize<T>(TextReader reader)
      {
         throw new NotImplementedException();
      }

      public static T Deserialize<T>(XmlReader reader)
      {
         throw new NotImplementedException();
      }

      public static T Deserialize<T>(string xml)
      {
         throw new NotImplementedException();
      }

      public static T DeserializeFromFile<T>(string filePath)
      {
         throw new NotImplementedException();
      }
   }
}
