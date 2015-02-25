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
using ApprovalTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;
using Org.Edgerunner.DotSerialize.Tests.Maps;

// ReSharper disable InconsistentNaming

namespace Org.Edgerunner.DotSerialize.Tests.SerializerTests
{
   /// <summary>
   ///    Summary description for SerializerTests
   /// </summary>
   [TestClass]
   public class SerializerTests
   {
      private const string SerializeDogResultsInProperOutput_Approved =
         "SerializerTests.SerializeDogResultsInProperOutput.approved.xml";

      private const string SerializeCatWithoutMapResultsInProperOutput_Approved =
         "SerializerTests.SerializeCatWithoutMapResultsInProperOutput.approved.xml";

      private const string SerializeCatWithMap1ResultsInProperOutput_Approved =
         "SerializerTests.SerializeCatWithMap1ResultsInProperOutput.approved.xml";

      private const string SerializeCatWithMap2ResultsInProperOutput_Approved =
         "SerializerTests.SerializeCatWithMap2ResultsInProperOutput.approved.xml";

      [TestInitialize]
      private void Setup()
      {
         Utilities.ExtractEmbeddedFile(SerializeDogResultsInProperOutput_Approved);
         Utilities.ExtractEmbeddedFile(SerializeCatWithoutMapResultsInProperOutput_Approved);
         Utilities.ExtractEmbeddedFile(SerializeCatWithMap1ResultsInProperOutput_Approved);
         Utilities.ExtractEmbeddedFile(SerializeCatWithMap2ResultsInProperOutput_Approved);
      }

      [TestCleanup]
      private void CleanUp()
      {
         Utilities.DeleteFile(SerializeDogResultsInProperOutput_Approved);
         Utilities.DeleteFile(SerializeCatWithoutMapResultsInProperOutput_Approved);
         Utilities.DeleteFile(SerializeCatWithMap1ResultsInProperOutput_Approved);
         Utilities.DeleteFile(SerializeCatWithMap2ResultsInProperOutput_Approved);
      }

      [TestMethod]
      public void SerializeDogResultsInProperOutput()
      {
         var dog = new Dog("Fido", "Golden Retriever", true, null);
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(dog);
         Approvals.VerifyXml(xml);
      }

      [TestMethod]
      public void SerializeCatWithoutMapResultsInProperOutput()
      {
         var cat = new Cat { Name = "Puss", Breed = "Saimese", Selfish = true, Age = 3 };
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(cat);
         Approvals.VerifyXml(xml);
      }

      [TestMethod]
      public void SerializeCatWithMap1ResultsInProperOutput()
      {
         var cat = new Cat { Name = "Puss", Breed = "Saimese", Selfish = true, Age = 3 };
         var serializer = new Serializer();
         serializer.RegisterClassMap<CatMap1>();
         string xml = serializer.SerializeObject(cat);
         Approvals.VerifyXml(xml);
      }

      [TestMethod]
      public void SerializeCatWithMap2ResultsInProperOutput()
      {
         var cat = new Cat { Name = "Puss", Breed = "Saimese", Selfish = true, Age = 3 };
         var serializer = new Serializer();
         serializer.RegisterClassMap<CatMap2>();
         string xml = serializer.SerializeObject(cat);
         Approvals.VerifyXml(xml);
      }
   }
}