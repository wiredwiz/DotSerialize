using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;
using Org.Edgerunner.DotSerialize.Tests.TypeSerializers;
// ReSharper disable InconsistentNaming

namespace Org.Edgerunner.DotSerialize.Tests.TypeSerializerTests
{
   [TestClass]
   public class CustomTypeSerializerTests
   {
      private const string SerializeDogResultsInExpectedOutput_Approved =
         "CustomTypeSerializerTests.SerializeDogResultsInExpectedOutput.approved.xml";

      [TestCleanup]
      private void CleanUp()
      {
         Utilities.DeleteFile(SerializeDogResultsInExpectedOutput_Approved);
      }

      [TestMethod]
      public void SerializeDogResultsInExpectedOutput()
      {
         var dog = new Dog("Fido", "Golden Retriever", true, null)
         {
            BirthDate = new DateTime(2003, 5, 10),
            Collar = DogCollarFactory.GetCollar(20, true)
         };
         var serializer = new Serializer();
         serializer.RegisterType<Dog>().ToTypeSerializer<DogSerializer>().WithConstructorArgument("title", "King");
         string xml = serializer.SerializeObject(dog);
         Approvals.VerifyXml(xml);
      }

      [TestInitialize]
      private void Setup()
      {
         Utilities.ExtractEmbeddedFile(SerializeDogResultsInExpectedOutput_Approved);
      }
   }
}
