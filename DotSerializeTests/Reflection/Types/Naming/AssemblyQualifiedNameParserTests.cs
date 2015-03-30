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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection.Types.Naming;

namespace Org.Edgerunner.DotSerialize.Tests.Reflection.Types.Naming
{
   [TestClass]
   public class AssemblyQualifiedNameParserTests : AssemblyQualifiedNameParser
   {
      [TestMethod]
      public void ReadMoreTextThanisPresentReturnsWhatIsPresent()
      {
         _Buffer = "this is a test";
         _Position = 0;
         Assert.AreEqual(_Buffer, ReadText(20));
      }

      [TestMethod]
      public void ReadExactTextReturnsAllText()
      {
         _Buffer = "this is a test";
         _Position = 0;
         Assert.AreEqual(_Buffer, ReadText(14));
      }

      [TestMethod]
      public void ReadPartialTextReturnsPartialText()
      {
         _Buffer = "this is a test";
         _Position = 0;
         Assert.AreEqual("this", ReadText(4));
      }

      [TestMethod]
      public void ReadTextPastLengthReturnsEmptyText()
      {
         _Buffer = "this is a test";
         _Position = 20;
         Assert.AreEqual(string.Empty, ReadText(4));
      }

      [TestMethod]
      public void ReadReturnsFirstCharacter()
      {
         _Buffer = "this is a test";
         _Position = 0;
         Assert.AreEqual('t', Read());
         Assert.AreEqual(1, _Position);
      }

      [TestMethod]
      public void ReadPastLengthReturnsNullCharacter()
      {
         _Buffer = "this is a test";
         _Position = 20;
         Assert.AreEqual(Eof, Read());
         Assert.AreEqual(20, _Position);
      }

      [TestMethod]
      public void PeekReturnsFirstCharacter()
      {
         _Buffer = "this is a test";
         _Position = 0;
         Assert.AreEqual('t', Peek());
         Assert.AreEqual(0, _Position);
      }

      [TestMethod]
      public void PeekPastLengthReturnsNullCharacter()
      {
         _Buffer = "this is a test";
         _Position = 20;
         Assert.AreEqual(Eof, Peek());
         Assert.AreEqual(20, _Position);
      }

      [TestMethod]
      public void BackMovesPositionBack()
      {
         _Buffer = "this is a test";
         _Position = 2;
         Back();
         Assert.AreEqual(1, _Position);
      }

      [TestMethod]
      public void BackAtZeroKeepsPositionAtZero()
      {
         _Buffer = "this is a test";
         _Position = 0;
         Back();
         Assert.AreEqual(0, _Position);
      }

      [TestMethod]
      public void ReadArrayDimensionsReturnsOne()
      {
         _Buffer = "System.String[], mscorlib";
         _Position = 13;
         var arrayDim = ReadArrayDimensions();
         Assert.AreEqual(1, arrayDim[0]);
         Assert.AreEqual(',', Peek());
         Assert.AreEqual(15, _Position);
      }

      [TestMethod]
      public void ReadArrayDimensionsReturnsThree()
      {
         _Buffer = "System.String[,,], mscorlib";
         _Position = 13;
         var arrayDim = ReadArrayDimensions();
         Assert.AreEqual(3, arrayDim[0]);
         Assert.AreEqual(',', Peek());
         Assert.AreEqual(17, _Position);
      }

      [TestMethod]
      [ExpectedException(typeof(ParserException))]
      public void ReadArrayDimensionsFromNonArrayThrowsParserException()
      {
         _Buffer = "System.String, mscorlib";
         _Position = 13;
         var dim = ReadArrayDimensions();
      }

      [TestMethod]
      [ExpectedException(typeof(ParserException))]
      public void ReadArrayDimensionsFromInvalidArrayThrowsParserException()
      {
         _Buffer = "System.String[foo,bar], mscorlib";
         _Position = 13;
         var dim = ReadArrayDimensions();
      }

      [TestMethod]
      public void ReadNonSpecialTextReturnsExpected()
      {
         _Buffer = "System.String[,,], mscorlib";
         _Position = 0;
         Assert.AreEqual("System.String", ReadNonSpecialText().ToString());
         Assert.AreEqual('[', Peek());
         Assert.AreEqual(13, _Position);
      }

      [TestMethod]
      public void ReadNonSpecialTextWithEscapeReturnsExpected()
      {
         _Buffer = @"System\,.String[,,], mscorlib";
         _Position = 0;
         Assert.AreEqual(@"System\,.String", ReadNonSpecialText().ToString());
         Assert.AreEqual('[', Peek());
         Assert.AreEqual(15, _Position);
      }

      [TestMethod]
      [ExpectedException(typeof(ParserException))]
      public void ReadNonSpecialTextAtEndOfFileThrowsException()
      {
         _Buffer = @"System.String[,,], mscorlib";
         _Position = 60;
         ReadNonSpecialText();
      }

      [TestMethod]
      public void ReadFourDigitVersionReturnsVersion()
      {
         _Buffer = @" Version=4.1.3.5";
         _Position = 0;
         var vers = ReadVersion();
         Assert.AreEqual(4, vers.Major);
         Assert.AreEqual(1, vers.Minor);
         Assert.AreEqual(3, vers.Build);
         Assert.AreEqual(5, vers.Revision);
      }

      [TestMethod]
      [ExpectedException(typeof(ParserException))]
      public void ReadOneDigitVersionThrowsException()
      {
         _Buffer = @" Version=4,";
         _Position = 0;
         ReadVersion();
      }

      [TestMethod]
      [ExpectedException(typeof(ParserException))]
      public void ReadVersionWithDigitBeyondMaxIntThrowsException()
      {
         _Buffer = @" Version=4.2147483648,";
         _Position = 0;
         ReadVersion();
      }

      [TestMethod]
      public void ReadTwoDigitVersionReturnsVersion()
      {
         _Buffer = @" Version=4.1";
         _Position = 0;
         var vers = ReadVersion();
         Assert.AreEqual(4, vers.Major);
         Assert.AreEqual(1, vers.Minor);
         Assert.AreEqual(-1, vers.Build);
         Assert.AreEqual(-1, vers.Revision);
      }

      [TestMethod]
      [ExpectedException(typeof(ParserException))]
      public void ReadNegativeVersionThrowsException()
      {
         _Buffer = @" Version=4.-1.0.0";
         _Position = 0;
         ReadVersion();
      }

      [TestMethod]
      [ExpectedException(typeof(ParserException))]
      public void ReadNonNumberVersionThrowsException()
      {
         _Buffer = @" Version=4.0.f.0";
         _Position = 0;
         ReadVersion();
      }

      [TestMethod]
      [ExpectedException(typeof(ParserException))]
      public void ReadNumberBeyondMaxIntThrowsException()
      {
         _Buffer = "2147483648";
         _Position = 0;
         ReadNumber();
      }

      [TestMethod]
      [ExpectedException(typeof(ParserException))]
      public void ReadInvalidNumberThrowsException()
      {
         _Buffer = "shoe";
         _Position = 0;
         ReadNumber();
      }

      [TestMethod]
      public void ReadNumberReturnsTwentyThree()
      {
         _Buffer = "23shoe";
         _Position = 0;
         var num = ReadNumber();
         Assert.AreEqual(23, num);
      }
      
      [TestMethod]
      public void ReadGenericStringListParsesProperly()
      {
         var aqn = Parse(typeof(List<string>).AssemblyQualifiedName);
         Assert.AreEqual("System.Collections.Generic.List", aqn.Type.Name);
         Assert.AreEqual(true, aqn.Type.IsGeneric);
         Assert.AreEqual(1, aqn.Type.SubTypes.Count);
         Assert.AreEqual("System.String", aqn.Type.SubTypes[0].Type.Name);
         Assert.AreEqual("mscorlib", aqn.Type.SubTypes[0].Assembly.Name);
         Assert.AreEqual("mscorlib", aqn.Assembly.Name);
         Assert.AreEqual("neutral", aqn.Culture);
         Assert.AreEqual("b77a5c561934e089", aqn.PublicKeyToken);
      }

      [TestMethod]
      public void ReadGenericIntStringDictionaryParsesProperly()
      {
         var aqn = Parse(typeof(Dictionary<int, string>).AssemblyQualifiedName);
         Assert.AreEqual("System.Collections.Generic.Dictionary", aqn.Type.Name);
         Assert.AreEqual(true, aqn.Type.IsGeneric);
         Assert.AreEqual(2, aqn.Type.SubTypes.Count);
         Assert.AreEqual("System.Int32", aqn.Type.SubTypes[0].Type.Name);
         Assert.AreEqual("mscorlib", aqn.Type.SubTypes[0].Assembly.Name);
         Assert.AreEqual("System.String", aqn.Type.SubTypes[1].Type.Name);
         Assert.AreEqual("mscorlib", aqn.Type.SubTypes[1].Assembly.Name);
         Assert.AreEqual("mscorlib", aqn.Assembly.Name);
         Assert.AreEqual("neutral", aqn.Culture);
         Assert.AreEqual("b77a5c561934e089", aqn.PublicKeyToken);
      }

      [TestMethod]
      public void FormatAssemblyQualifiedNameForIntStringDictionaryMatchesOrginalText()
      {
         var aqn = Parse(typeof(Dictionary<int, string>).AssemblyQualifiedName);
         Assert.AreEqual(_Buffer, aqn.ToString());
      }

      [TestMethod]
      public void FormatAssemblyQualifiedNameForStringListArrayWithOneDimensionMatchesOrginalText()
      {
         var aqn = Parse(typeof(List<string>[]).AssemblyQualifiedName);
         Assert.AreEqual(_Buffer, aqn.ToString());
      }

      [TestMethod]
      public void FormatAssemblyQualifiedNameForStringListArrayWithThreeDimensionsMatchesOrginalText()
      {
         var aqn = Parse(typeof(List<string>[,,]).AssemblyQualifiedName);
         Assert.AreEqual(_Buffer, aqn.ToString());
      }

      [TestMethod]
      public void FormatAssemblyQualifiedNameForStringListArrayOfArrayOfArrayMatchesOrginalText()
      {
         var aqn = Parse(typeof(List<string>[][][]).AssemblyQualifiedName);
         Assert.AreEqual(_Buffer, aqn.ToString());
      }
   }
}