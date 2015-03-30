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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
   }
}