using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotSerializeTests.DataTypes;

namespace DotSerializeTests
{
   [TestClass]
   public class TypeInspectorTests
   {
      [TestMethod]
      public void InspectDogReturnsInfoWithFiveTypeMembers()
      {
         var info = new Org.Edgerunner.DotSerialize.Reflection.TypeInspector().GetInfo(typeof(Dog));
         Assert.AreEqual<int>(5, info.MemberInfoByEntityName.Count);
         Assert.AreEqual<int>(5, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectOwnerReturnsInfoWithThreeTypeMembers()
      {
         var info = new Org.Edgerunner.DotSerialize.Reflection.TypeInspector().GetInfo(typeof(Owner));
         Assert.AreEqual<int>(3, info.MemberInfoByEntityName.Count);
         Assert.AreEqual<int>(3, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectOwnerReturnsInfoWithRootNodeNameDogOwner()
      {
         var info = new Org.Edgerunner.DotSerialize.Reflection.TypeInspector().GetInfo(typeof(Owner));
         Assert.AreEqual<string>("DogOwner", info.EntityName);
      }

      [TestMethod]
      public void InspectPersonReturnsInfoWithOneTypeMember()
      {
         var info = new Org.Edgerunner.DotSerialize.Reflection.TypeInspector().GetInfo(typeof(Person));
         Assert.AreEqual<int>(1, info.MemberInfoByEntityName.Count);
         Assert.AreEqual<int>(1, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectListOfNullableIntReturnsCleanRootNodeName()
      {
         var info = new Org.Edgerunner.DotSerialize.Reflection.TypeInspector().GetInfo(typeof(List<int?>));
         Assert.AreEqual<string>("List_int__", info.EntityName);
      }

      [TestMethod]
      public void InspectBoneReturnsInfoWithThreeTypeMembers()
      {
         var info = new Org.Edgerunner.DotSerialize.Reflection.TypeInspector().GetInfo(typeof(Bone));
         Assert.AreEqual<int>(3, info.MemberInfoByEntityName.Count);
         Assert.AreEqual<int>(3, info.MemberInfoByName.Count);
      }
   }
}
