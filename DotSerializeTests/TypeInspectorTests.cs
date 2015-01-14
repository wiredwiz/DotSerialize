using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotSerializeTests
{
   [TestClass]
   public class TypeInspectorTests
   {
      [TestMethod]
      public void InspectDogReturnsInfoWithFiveTypeMembers()
      {
         var info = new Org.Edgerunner.DotSerialize.Reflection.TypeInspector().GetInfo(typeof(DataClasses.Dog));
         Assert.AreEqual<int>(5, info.MemberInfoByEntityName.Count);
         Assert.AreEqual<int>(5, info.MemberInfoByName.Count);
      }
   }
}
