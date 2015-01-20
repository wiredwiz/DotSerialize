using System;
using System.Text;
using System.Collections.Generic;
using DotSerializeTests.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Edgerunner.DotSerialize;

namespace DotSerializeTests
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
