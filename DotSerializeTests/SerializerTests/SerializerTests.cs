using System;
using ApprovalTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;

namespace Org.Edgerunner.DotSerialize.Tests.SerializerTests
{
   /// <summary>
   /// Summary description for SerializerTests
   /// </summary>
   [TestClass]
   public class SerializerTests
   {
      private const string SerializeDogResultsInProperFile_Approved = "SerializerTests.SerializeDogResultsInProperFile.approved.xml";

      [TestInitialize]
      private void Setup()
      {
         Utilities.ExtractEmbeddedFile(SerializeDogResultsInProperFile_Approved);
      }

      [TestCleanup]
      private void CleanUp()
      {
         Utilities.DeleteFile(SerializeDogResultsInProperFile_Approved);
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
