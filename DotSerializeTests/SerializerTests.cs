using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;

namespace Org.Edgerunner.DotSerialize.Tests
{
   /// <summary>
   /// Summary description for SerializerTests
   /// </summary>
   [TestClass]
   public class SerializerTests
   {
      [TestMethod]
      public void SerializeDogResultsInProperFile()
      {
         var dog = new Dog("Fido", "Golden Retriever", true, null);
         var serializer = new Serializer();
         serializer.SerializeObjectToFile<Dog>("C:\\DogSerializeTest.xml", dog);
         
      }
   }
}
