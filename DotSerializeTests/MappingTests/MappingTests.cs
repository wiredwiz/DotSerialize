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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Edgerunner.DotSerialize.Mapping;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;

namespace Org.Edgerunner.DotSerialize.Tests.MappingTests
{
   [TestClass]
   public class MappingTests : XmlClassMap<Cat>
   {
      [TestMethod]
      public void AddingTwoMapsResultsInTwoMaps()
      {
         Map(x => x.Name);
         Map(x => x.Breed);
         Assert.AreEqual(2, _Mappings.Count);
      }

      [TestMethod]
      public void ClassMappedWithNamespaceResultsInCorrectNamespace()
      {
         const string @namespace = "edgerunner.org";
         WithNamespace(@namespace);
         Assert.AreEqual(@namespace, _Namespace);
      }

      [TestMethod]
      public void ClassNamedFancyCatResultsInMatchingName()
      {
         const string fancyName = "FancyCat";
         Named(fancyName);
         Assert.AreEqual(fancyName, _RootNodeName);
      }

      [TestMethod]
      public void NodeAsAttributeResultsInAttribute()
      {
         var node = Map(x => x.Name).AsAttribute();
         Assert.IsTrue(node.Info.IsAttribute);
         Assert.AreEqual(1, _Mappings.Count);
      }

      [TestMethod]
      public void NodeOrderedAsThreeResultsOrderOfThree()
      {
         var node = Map(x => x.Name).OrderedAs(3);
         Assert.AreEqual(3, node.Info.Order);
         Assert.AreEqual(1, _Mappings.Count);
      }

      [TestMethod]
      public void NodeUsingNameResultsInMatchingName()
      {
         const string fancyName = "FancyName";
         const string name = "Name";
         var node = Map(x => x.Name).UsingName(fancyName);
         Assert.AreEqual(fancyName, node.Info.EntityName);
         Assert.AreEqual(name, node.Info.Name);
         Assert.AreEqual(1, _Mappings.Count);
      }
   }
}