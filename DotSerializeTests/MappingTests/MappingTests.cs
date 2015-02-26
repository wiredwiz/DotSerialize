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
         Assert.AreEqual(2, this._Mappings.Count);
      }

      [TestMethod]
      public void NodeAsAttributeResultsInAttribute()
      {
         var node = Map(x => x.Name).AsAttribute();
         Assert.IsTrue(node.Info.IsAttribute);
         Assert.AreEqual(1, this._Mappings.Count);
      }

      [TestMethod]
      public void NodeUsingNameResultsInMatchingName()
      {
         const string fancyName = "FancyName";
         const string name = "Name";
         var node = Map(x => x.Name).UsingName(fancyName);
         Assert.AreEqual(fancyName, node.Info.EntityName);
         Assert.AreEqual(name, node.Info.Name);
         Assert.AreEqual(1, this._Mappings.Count);
      }

      [TestMethod]
      public void NodeOrderedAsThreeResultsOrderOfThree()
      {
         var node = Map(x => x.Name).OrderedAs(3);
         Assert.AreEqual(3, node.Info.Order);
         Assert.AreEqual(1, this._Mappings.Count);
      }

      [TestMethod]
      public void ClassMappedWithNamespaceResultsInCorrectNamespace()
      {
         const string @namespace = "edgerunner.org";
         this.WithNamespace(@namespace);
         Assert.AreEqual(@namespace, this._Namespace);
      }

      [TestMethod]
      public void ClassNamedFancyCatResultsInMatchingName()
      {
         const string fancyName = "FancyCat";
         this.Named(fancyName);
         Assert.AreEqual(fancyName, this._RootNodeName);
      }
   }
}
