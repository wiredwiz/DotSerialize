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
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Reflection.Types;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;

namespace Org.Edgerunner.DotSerialize.Tests
{
   [TestClass]
   public class TypeInspectorTests
   {
      public string BFN(string name)
      {
         return string.Format("<{0}>k__BackingField", name);
      }

      [TestMethod]
      public void InspectBoneReturnsInfoWithThreeTypeMembers()
      {
         var info = new TypeInspector().GetInfo(typeof(Bone));
         Assert.AreEqual(3, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(3, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectDogReturnsInfoWithTenTypeMembers()
      {
         var info = new TypeInspector().GetInfo(typeof(Dog));
         Assert.AreEqual(10, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(10, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectHazardousWidgetReturnsInfoWithSixTypeMembers()
      {
         var info = new TypeInspector().GetInfo(typeof(HazardousWidget));
         Assert.AreEqual("Widget", info.EntityName);
         Assert.AreEqual("acme.org", info.Namespace);
         Assert.AreEqual(6, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(6, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectInventoriedWidgetReturnsInfoWithEightTypeMembers()
      {
         var info = new TypeInspector().GetInfo(typeof(InventoriedWidget));
         Assert.AreEqual("InventoriedWidget", info.EntityName);
         Assert.AreEqual("acme.org", info.Namespace);
         Assert.AreEqual(8, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(8, info.MemberInfoByName.Count);
         int elements = 0;
         int attributes = 0;
         foreach (var item in info.MemberInfoByEntityName)
         {
            Assert.AreEqual(item.Value.Name == BFN("QuantityInInventory") ? 8 : 999, item.Value.Order);
            if (item.Value.IsAttribute)
               attributes++;
            else
               elements++;
         }
         Assert.AreEqual(7, attributes);
         Assert.AreEqual(1, elements);
      }

      [TestMethod]
      public void InspectListOfNullableIntReturnsCleanRootNodeName()
      {
         var info = new TypeInspector().GetInfo(typeof(List<int?>));
         Assert.AreEqual("List_int__", info.EntityName);
      }

      [TestMethod]
      public void InspectOwnerReturnsInfoWithRootNodeNamePetOwner()
      {
         var info = new TypeInspector().GetInfo(typeof(Owner));
         Assert.AreEqual("PetOwner", info.EntityName);
      }

      [TestMethod]
      public void InspectOwnerReturnsInfoWithSixTypeMembers()
      {
         var info = new TypeInspector().GetInfo(typeof(Owner));
         Assert.AreEqual(6, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(6, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectPersonReturnsInfoWithFiveTypeMember()
      {
         var info = new TypeInspector().GetInfo(typeof(Person));
         Assert.AreEqual(5, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(5, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectSellableWidgetReturnsInfoWithTenTypeMembers()
      {
         var info = new TypeInspector().GetInfo(typeof(SellableWidget));
         Assert.AreEqual("SaleWidget", info.EntityName);
         Assert.AreEqual("acme.net", info.Namespace);
         Assert.AreEqual(10, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(10, info.MemberInfoByName.Count);
      }

      [TestMethod]
      public void InspectWidgetReturnsInfoWithSevenTypeMembers()
      {
         var info = new TypeInspector().GetInfo(typeof(Widget));
         Assert.AreEqual("Widget", info.EntityName);
         Assert.AreEqual("acme.org", info.Namespace);
         Assert.AreEqual(7, info.MemberInfoByEntityName.Count);
         Assert.AreEqual(7, info.MemberInfoByName.Count);
      }
   }
}