using System;
using System.IO;
using System.Xml;
using ApprovalTests;
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
      protected const string _SerializeDogResultsInProperFile = "SerializerTests.SerializeDogResultsInProperFile.approved.xml";

      [TestInitialize]
      private void Setup()
      {
         Utilities.ExtractEmbeddedFile(_SerializeDogResultsInProperFile);
      }
      [TestCleanup]
      private void CleanUp()
      {
         Utilities.DeleteFile(_SerializeDogResultsInProperFile);
      }

      [TestMethod]
      public void SerializeDogResultsInProperFile()
      {
         var dog = new Dog("Fido", "Golden Retriever", true, null);
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(dog);
         Approvals.VerifyXml(xml);
      }
   }
}
