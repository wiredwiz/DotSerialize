using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;

namespace Org.Edgerunner.DotSerialize.Tests
{
   [TestClass]
   public class TypeInspectorTests
   {
      [TestMethod]
      public void InspectDogReturnsInfoWithTenTypeMembers()
      {
         var info = new Reflection.TypeInspector().GetInfo(typeof(Dog));
         Assert.AreEqual<int>(10, info.MemberInfoByEntityName.Count);
         Assert.AreEqual<int>(10, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectOwnerReturnsInfoWithSixTypeMembers()
      {
         var info = new Reflection.TypeInspector().GetInfo(typeof(Owner));
         Assert.AreEqual<int>(6, info.MemberInfoByEntityName.Count);
         Assert.AreEqual<int>(6, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectOwnerReturnsInfoWithRootNodeNamePetOwner()
      {
         var info = new Reflection.TypeInspector().GetInfo(typeof(Owner));
         Assert.AreEqual<string>("PetOwner", info.EntityName);
      }

      [TestMethod]
      public void InspectPersonReturnsInfoWithFiveTypeMember()
      {
         var info = new Reflection.TypeInspector().GetInfo(typeof(Person));
         Assert.AreEqual<int>(5, info.MemberInfoByEntityName.Count);
         Assert.AreEqual<int>(5, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectListOfNullableIntReturnsCleanRootNodeName()
      {
         var info = new Reflection.TypeInspector().GetInfo(typeof(List<int?>));
         Assert.AreEqual<string>("List_int__", info.EntityName);
      }

      [TestMethod]
      public void InspectBoneReturnsInfoWithThreeTypeMembers()
      {
         var info = new Reflection.TypeInspector().GetInfo(typeof(Bone));
         Assert.AreEqual<int>(3, info.MemberInfoByEntityName.Count);
         Assert.AreEqual<int>(3, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectWidgetReturnsInfoWithSevenTypeMembers()
      {
         var info = new Reflection.TypeInspector().GetInfo(typeof(Widget));
         Assert.AreEqual("Widget", info.EntityName);
         Assert.AreEqual("acme.org", info.Namespace);
         Assert.AreEqual(7, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(7, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectHazardousWidgetReturnsInfoWithSixTypeMembers()
      {
         var info = new Reflection.TypeInspector().GetInfo(typeof(HazardousWidget));
         Assert.AreEqual("Widget", info.EntityName);
         Assert.AreEqual("acme.org", info.Namespace);
         Assert.AreEqual(6, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(6, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectSellableWidgetReturnsInfoWithTenTypeMembers()
      {
         var info = new Reflection.TypeInspector().GetInfo(typeof(SellableWidget));
         Assert.AreEqual("SaleWidget", info.EntityName);
         Assert.AreEqual("acme.net", info.Namespace);
         Assert.AreEqual(10, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(10, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectInventoriedWidgetReturnsInfoWithEightTypeMembers()
      {
         var info = new Reflection.TypeInspector().GetInfo(typeof(InventoriedWidget));
         Assert.AreEqual("InventoriedWidget", info.EntityName);
         Assert.AreEqual("acme.org", info.Namespace);
         Assert.AreEqual(8, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(8, info.MemberInfoByName.Count);
         int elements = 0;
         int attributes = 0;
         foreach (var item in info.MemberInfoByEntityName)
         {
            Assert.AreEqual(999, item.Value.Order);
            if (item.Value.IsAttribute)
               attributes++;
            else
               elements++;
         }
         Assert.AreEqual(7, attributes);
         Assert.AreEqual(1, elements);
      }
   }
}
