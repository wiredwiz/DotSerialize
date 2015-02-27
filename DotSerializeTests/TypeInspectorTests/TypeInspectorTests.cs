using System;
using System.Collections.Generic;
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
      public void InspectOwnerReturnsInfoWithRootNodeNameDogOwner()
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
   }
}
