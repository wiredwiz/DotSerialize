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
using System.Globalization;
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

      private const string SerializeDogOmittingTypesResultsInProperOutput_Approved =
         "SerializerTests.SerializeDogOmittingTypesResultsInProperOutput.approved.xml";

      private const string SerializeCatDECultureResultsInProperOutput_Approved =
         "SerializerTests.SerializeCatDECultureResultsInProperOutput.approved.xml";

      private const string SerializeCatWithoutMapResultsInProperOutput_Approved =
         "SerializerTests.SerializeCatWithoutMapResultsInProperOutput.approved.xml";

      private const string SerializeCatOmittingReferentialIntegrityResultsInProperOutput_Approved =
         "SerializerTests.SerializeCatOmittingReferentialIntegrityResultsInProperOutput.approved.xml";

      private const string SerializeCatWithMap1ResultsInProperOutput_Approved =
         "SerializerTests.SerializeCatWithMap1ResultsInProperOutput.approved.xml";

      private const string SerializeCatWithMap2ResultsInProperOutput_Approved =
         "SerializerTests.SerializeCatWithMap2ResultsInProperOutput.approved.xml";

      [TestCleanup]
      private void CleanUp()
      {
         Utilities.DeleteFile(SerializeDogResultsInProperOutput_Approved);
         Utilities.DeleteFile(SerializeDogOmittingTypesResultsInProperOutput_Approved);
         Utilities.DeleteFile(SerializeCatOmittingReferentialIntegrityResultsInProperOutput_Approved);
         Utilities.DeleteFile(SerializeCatDECultureResultsInProperOutput_Approved);
         Utilities.DeleteFile(SerializeCatWithoutMapResultsInProperOutput_Approved);
         Utilities.DeleteFile(SerializeCatWithMap1ResultsInProperOutput_Approved);
         Utilities.DeleteFile(SerializeCatWithMap2ResultsInProperOutput_Approved);
      }

      [TestMethod]
      public void DeserializeCatWithoutMapSucceeds()
      {
         var cat = new Cat { Name = "Puss", Breed = "Saimese", Selfish = true, BirthDate = new DateTime(2000, 12, 5) };
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(cat);
         var cat2 = serializer.DeserializeObject<Cat>(xml);
         Assert.AreEqual(cat, cat2);
      }

      [TestMethod]
      public void DeserializeCatWithoutMapAndNoNameSucceeds()
      {
         var cat = new Cat { Name = string.Empty, Breed = "Saimese", Selfish = true, BirthDate = new DateTime(2000, 12, 5) };
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(cat);
         var cat2 = serializer.DeserializeObject<Cat>(xml);
         Assert.AreEqual(cat, cat2);
      }

      [TestMethod]
      public void DeserializeMaintainsReferentialIntegrity()
      {
         var owner = new Owner("Joe", "J", "Smith") { BirthDate = new DateTime(1970, 3, 20) };
         var cat = new Cat(owner) { Name = "Puss", Breed = "Saimese", Selfish = true, BirthDate = new DateTime(2000, 12, 5) };
         owner.Pets = new Pet[] { cat };
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(cat);
         cat = serializer.DeserializeObject<Cat>(xml);
         Assert.AreSame(cat, cat.Owner.Pets[0]);
      }

      [TestMethod]
      public void SerializeCatDECultureResultsInProperOutput()
      {
         var cat = new Cat
                   {
                      Name = "Puss",
                      Breed = "Saimese",
                      Selfish = true,
                      BirthDate = new DateTime(2000, 12, 5),
                      Height = 30.48,
                      Weight = 2.26796
                   };
         var serializer = new Serializer
                          {
                             Settings =
                             {
                                DisableReferentialIntegrity = true,
                                OmitTypeWhenPossible = true,
                                Culture = CultureInfo.GetCultureInfo("de-DE")
                             }
                          };
         string xml = serializer.SerializeObject(cat);
         Approvals.VerifyXml(xml);
      }

      [TestMethod]
      public void SerializeCatDisablingReferentialIntegrityResultsInProperOutput()
      {
         var cat = new Cat
                   {
                      Name = "Puss",
                      Breed = "Saimese",
                      Selfish = true,
                      BirthDate = new DateTime(2000, 12, 5),
                      Height = 30.48,
                      Weight = 2.26796
                   };
         var serializer = new Serializer { Settings = { DisableReferentialIntegrity = true, OmitTypeWhenPossible = true } };
         string xml = serializer.SerializeObject(cat);
         Approvals.VerifyXml(xml);
      }

      [TestMethod]
      public void SerializeCatWithMap1ResultsInProperOutput()
      {
         var cat = new Cat { Name = "Puss", Breed = "Saimese", Selfish = true, BirthDate = new DateTime(2000, 12, 5) };
         var serializer = new Serializer();
         serializer.RegisterClassMap<CatMap1>();
         string xml = serializer.SerializeObject(cat);
         Approvals.VerifyXml(xml);
      }

      [TestMethod]
      public void SerializeCatWithMap2ResultsInProperOutput()
      {
         var cat = new Cat { Name = "Puss", Breed = "Saimese", Selfish = true, BirthDate = new DateTime(2000, 12, 5) };
         var serializer = new Serializer();
         serializer.RegisterClassMap<CatMap2>();
         string xml = serializer.SerializeObject(cat);
         Approvals.VerifyXml(xml);
      }

      [TestMethod]
      public void SerializeCatWithoutMapResultsInProperOutput()
      {
         var cat = new Cat { Name = "Puss", Breed = "Saimese", Selfish = true, BirthDate = new DateTime(2000, 12, 5) };
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(cat);
         Approvals.VerifyXml(xml);
      }

      [TestMethod]
      public void SerializeDogOmittingTypesResultsInProperOutput()
      {
         var owner = new Owner("Joe", "J", "Smith") { BirthDate = new DateTime(1970, 3, 20) };
         var mother = new Owner("Jane", "M", "Smith") { BirthDate = new DateTime(1951, 6, 12) };
         var father = new Owner("John", "K", "Smith") { BirthDate = new DateTime(1948, 4, 16) };
         var child1 = new Owner("Simon", "P", "Smith") { BirthDate = new DateTime(2000, 3, 22) };
         var child2 = new Owner("Sally", "P", "Smith") { BirthDate = new DateTime(2001, 1, 1) };
         owner.Father = father;
         owner.Mother = mother;
         father.Children = new List<Person> { owner };
         mother.Children = new List<Person> { owner };
         owner.Children = new List<Person> { child1, child2 };
         child1.Father = owner;
         child2.Father = owner;
         var dog = new Dog("Fido", "Golden Retriever", true, owner)
                   {
                      BirthDate = new DateTime(2003, 5, 10),
                      Collar = DogCollarFactory.GetCollar(20, true)
                   };
         owner.Pets = new Pet[] { dog };
         child1.Pets = new Pet[] { dog };
         child2.Pets = new Pet[] { dog };
         var serializer = new Serializer { Settings = { OmitTypeWhenPossible = true } };
         string xml = serializer.SerializeObject(dog);
         Approvals.VerifyXml(xml);
      }

      [TestMethod]
      public void SerializeDogResultsInProperOutput()
      {
         var owner = new Owner("Joe", "J", "Smith") { BirthDate = new DateTime(1970, 3, 20) };
         var mother = new Owner("Jane", "M", "Smith") { BirthDate = new DateTime(1951, 6, 12) };
         var father = new Owner("John", "K", "Smith") { BirthDate = new DateTime(1948, 4, 16) };
         var child1 = new Owner("Simon", "P", "Smith") { BirthDate = new DateTime(2000, 3, 22) };
         var child2 = new Owner("Sally", "P", "Smith") { BirthDate = new DateTime(2001, 1, 1) };
         owner.Father = father;
         owner.Mother = mother;
         father.Children = new List<Person> { owner };
         mother.Children = new List<Person> { owner };
         owner.Children = new List<Person> { child1, child2 };
         child1.Father = owner;
         child2.Father = owner;
         var dog = new Dog("Fido", "Golden Retriever", true, owner)
                   {
                      BirthDate = new DateTime(2003, 5, 10),
                      Collar = DogCollarFactory.GetCollar(20, true)
                   };
         owner.Pets = new Pet[] { dog };
         child1.Pets = new Pet[] { dog };
         child2.Pets = new Pet[] { dog };
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(dog);
         Approvals.VerifyXml(xml);
      }

      [TestMethod]
      public void SerializePrimitiveTypesTesterDeserializesCorrectly1()
      {
         var prim = new PrimitiveTypesTester
         {
            BooleanDat = true,
            StringDat = "test",
            ByteDat = Convert.ToByte(true),
            SbyteDat = Convert.ToSByte(-22),
            CharDat = 'h',
            DecimalDat = (decimal)1.3,
            SingleDat = (Single)423.323,
            DoubleDat = 1.563423,
            Int16Dat = 16,
            Int32Dat = -32,
            Int64Dat = Int64.MaxValue,
            UInt16Dat = 16,
            UInt32Dat = 32,
            UInt64Dat = 64,
            DateTimeDat = new DateTime(2015, 1, 1)
         };
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(prim);
         var newPrim = serializer.DeserializeObject<PrimitiveTypesTester>(xml);
         Assert.AreEqual(prim.StringDat, newPrim.StringDat);
         Assert.AreEqual(prim.ByteDat, newPrim.ByteDat);
         Assert.AreEqual(prim.SbyteDat, newPrim.SbyteDat);
         Assert.AreEqual(prim.CharDat, newPrim.CharDat);
         Assert.AreEqual(prim.DecimalDat, newPrim.DecimalDat);
         Assert.AreEqual(prim.SingleDat, newPrim.SingleDat);
         Assert.AreEqual(prim.DoubleDat, newPrim.DoubleDat);
         Assert.AreEqual(prim.Int16Dat, newPrim.Int16Dat);
         Assert.AreEqual(prim.Int32Dat, newPrim.Int32Dat);
         Assert.AreEqual(prim.Int64Dat, newPrim.Int64Dat);
         Assert.AreEqual(prim.UInt16Dat, newPrim.UInt16Dat);
         Assert.AreEqual(prim.UInt32Dat, newPrim.UInt32Dat);
         Assert.AreEqual(prim.UInt64Dat, newPrim.UInt64Dat);
         Assert.AreEqual(prim.DateTimeDat, newPrim.DateTimeDat);
         Assert.AreEqual(prim.BooleanDat, newPrim.BooleanDat);
      }

      [TestMethod]
      public void SerializePrimitiveTypesTesterDeserializesCorrectly2()
      {
         var prim = new PrimitiveTypesTester
         {
            BooleanDat = true,
            StringDat = string.Empty,
            ByteDat = Convert.ToByte(true),
            SbyteDat = Convert.ToSByte(-22),
            CharDat = Char.MinValue,
            DecimalDat = (decimal)1.3,
            SingleDat = 0,
            DoubleDat = 1.563423,
            Int16Dat = 16,
            Int32Dat = -32,
            Int64Dat = Int64.MaxValue,
            UInt16Dat = 16,
            UInt32Dat = 32,
            UInt64Dat = 64,
            DateTimeDat = new DateTime(2015, 1, 1)
         };
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(prim);
         var newPrim = serializer.DeserializeObject<PrimitiveTypesTester>(xml);
         Assert.AreEqual(prim.StringDat, newPrim.StringDat);
         Assert.AreEqual(prim.ByteDat, newPrim.ByteDat);
         Assert.AreEqual(prim.SbyteDat, newPrim.SbyteDat);
         Assert.AreEqual(prim.CharDat, newPrim.CharDat);
         Assert.AreEqual(prim.DecimalDat, newPrim.DecimalDat);
         Assert.AreEqual(prim.SingleDat, newPrim.SingleDat);
         Assert.AreEqual(prim.DoubleDat, newPrim.DoubleDat);
         Assert.AreEqual(prim.Int16Dat, newPrim.Int16Dat);
         Assert.AreEqual(prim.Int32Dat, newPrim.Int32Dat);
         Assert.AreEqual(prim.Int64Dat, newPrim.Int64Dat);
         Assert.AreEqual(prim.UInt16Dat, newPrim.UInt16Dat);
         Assert.AreEqual(prim.UInt32Dat, newPrim.UInt32Dat);
         Assert.AreEqual(prim.UInt64Dat, newPrim.UInt64Dat);
         Assert.AreEqual(prim.DateTimeDat, newPrim.DateTimeDat);
         Assert.AreEqual(prim.BooleanDat, newPrim.BooleanDat);
      }

      [TestMethod]
      public void SerializePrimitiveTypesTesterDeserializesCorrectly3()
      {
         var prim = new PrimitiveTypesTester
         {
            BooleanDat = false,
            StringDat = string.Empty,
            ByteDat = Convert.ToByte(false),
            SbyteDat = sbyte.MinValue,
            CharDat = Char.MinValue,
            DecimalDat = decimal.MinValue,
            SingleDat = Single.MinValue,
            DoubleDat = double.MinValue,
            Int16Dat = Int16.MinValue,
            Int32Dat = Int32.MinValue,
            Int64Dat = Int64.MinValue,
            UInt16Dat = UInt16.MinValue,
            UInt32Dat = UInt32.MinValue,
            UInt64Dat = UInt64.MinValue,
            DateTimeDat = DateTime.MinValue
         };
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(prim);
         var newPrim = serializer.DeserializeObject<PrimitiveTypesTester>(xml);
         Assert.AreEqual(prim.StringDat, newPrim.StringDat);
         Assert.AreEqual(prim.ByteDat, newPrim.ByteDat);
         Assert.AreEqual(prim.SbyteDat, newPrim.SbyteDat);
         Assert.AreEqual(prim.CharDat, newPrim.CharDat);
         Assert.AreEqual(prim.DecimalDat, newPrim.DecimalDat);
         Assert.AreEqual(prim.SingleDat, newPrim.SingleDat);
         Assert.AreEqual(prim.DoubleDat, newPrim.DoubleDat);
         Assert.AreEqual(prim.Int16Dat, newPrim.Int16Dat);
         Assert.AreEqual(prim.Int32Dat, newPrim.Int32Dat);
         Assert.AreEqual(prim.Int64Dat, newPrim.Int64Dat);
         Assert.AreEqual(prim.UInt16Dat, newPrim.UInt16Dat);
         Assert.AreEqual(prim.UInt32Dat, newPrim.UInt32Dat);
         Assert.AreEqual(prim.UInt64Dat, newPrim.UInt64Dat);
         Assert.AreEqual(prim.DateTimeDat, newPrim.DateTimeDat);
         Assert.AreEqual(prim.BooleanDat, newPrim.BooleanDat);
      }

      [TestMethod]
      public void SerializePrimitiveTypesTesterDeserializesCorrectly4()
      {
         var prim = new PrimitiveTypesTester
         {
            BooleanDat = true,
            StringDat = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis accumsan nunc",
            ByteDat = Convert.ToByte(true),
            SbyteDat = sbyte.MaxValue,
            CharDat = Char.MaxValue,
            DecimalDat = decimal.MaxValue,
            SingleDat = Single.MaxValue,
            DoubleDat = double.MaxValue,
            Int16Dat = Int16.MaxValue,
            Int32Dat = Int32.MaxValue,
            Int64Dat = Int64.MaxValue,
            UInt16Dat = UInt16.MaxValue,
            UInt32Dat = UInt32.MaxValue,
            UInt64Dat = UInt64.MaxValue,
            DateTimeDat = DateTime.MaxValue
         };
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(prim);
         var newPrim = serializer.DeserializeObject<PrimitiveTypesTester>(xml);
         Assert.AreEqual(prim.StringDat, newPrim.StringDat);
         Assert.AreEqual(prim.ByteDat, newPrim.ByteDat);
         Assert.AreEqual(prim.SbyteDat, newPrim.SbyteDat);
         Assert.AreEqual(prim.CharDat, newPrim.CharDat);
         Assert.AreEqual(prim.DecimalDat, newPrim.DecimalDat);
         Assert.AreEqual(prim.SingleDat, newPrim.SingleDat);
         Assert.AreEqual(prim.DoubleDat, newPrim.DoubleDat);
         Assert.AreEqual(prim.Int16Dat, newPrim.Int16Dat);
         Assert.AreEqual(prim.Int32Dat, newPrim.Int32Dat);
         Assert.AreEqual(prim.Int64Dat, newPrim.Int64Dat);
         Assert.AreEqual(prim.UInt16Dat, newPrim.UInt16Dat);
         Assert.AreEqual(prim.UInt32Dat, newPrim.UInt32Dat);
         Assert.AreEqual(prim.UInt64Dat, newPrim.UInt64Dat);
         Assert.AreEqual(prim.DateTimeDat, newPrim.DateTimeDat);
         Assert.AreEqual(prim.BooleanDat, newPrim.BooleanDat);
      }

      [TestInitialize]
      private void Setup()
      {
         Utilities.ExtractEmbeddedFile(SerializeDogResultsInProperOutput_Approved);
         Utilities.ExtractEmbeddedFile(SerializeDogOmittingTypesResultsInProperOutput_Approved);
         Utilities.ExtractEmbeddedFile(SerializeCatOmittingReferentialIntegrityResultsInProperOutput_Approved);
         Utilities.ExtractEmbeddedFile(SerializeCatDECultureResultsInProperOutput_Approved);
         Utilities.ExtractEmbeddedFile(SerializeCatWithoutMapResultsInProperOutput_Approved);
         Utilities.ExtractEmbeddedFile(SerializeCatWithMap1ResultsInProperOutput_Approved);
         Utilities.ExtractEmbeddedFile(SerializeCatWithMap2ResultsInProperOutput_Approved);
      }
   }
}