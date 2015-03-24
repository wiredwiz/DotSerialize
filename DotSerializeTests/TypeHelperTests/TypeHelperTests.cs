using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Edgerunner.DotSerialize.Utilities;

namespace Org.Edgerunner.DotSerialize.Tests.TypeHelperTests
{
   [TestClass]
   public class TypeHelperTests
   {
      [TestMethod]
      public void IsPrimitiveStringEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(string)));
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(String)));
      }

      [TestMethod]
      public void IsPrimitiveCharEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(char)));
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(Char)));
      }

      [TestMethod]
      public void IsPrimitiveBooleanEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(bool)));
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(Boolean)));
      }

      [TestMethod]
      public void IsPrimitiveIntEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(int)));
      }

      [TestMethod]
      public void IsPrimitiveUintEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(uint)));
      }

      [TestMethod]
      public void IsPrimitiveInt16EvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(Int16)));
      }

      [TestMethod]
      public void IsPrimitiveInt32EvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(Int32)));
      }

      [TestMethod]
      public void IsPrimitiveInt64EvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(Int64)));
      }

      [TestMethod]
      public void IsPrimitiveUint16EvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(UInt16)));
      }

      [TestMethod]
      public void IsPrimitiveUint32EvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(UInt32)));
      }

      [TestMethod]
      public void IsPrimitiveUint64EvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(UInt64)));
      }

      [TestMethod]
      public void IsPrimitiveShortEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(short)));
      }

      [TestMethod]
      public void IsPrimitiveLongEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(long)));
      }

      [TestMethod]
      public void IsPrimitiveUshortEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(ushort)));
      }

      [TestMethod]
      public void IsPrimitiveUlongEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(ulong)));
      }

      [TestMethod]
      public void IsPrimitiveSingleEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(Single)));
      }
      
      [TestMethod]
      public void IsPrimitiveDoubleEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(double)));
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(Double)));
      }

      [TestMethod]
      public void IsPrimitiveDecimalEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(decimal)));
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(Decimal)));
      }

      [TestMethod]
      public void IsPrimitiveFloatEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(float)));
      }

      [TestMethod]
      public void IsPrimitiveDateTimeEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(DateTime)));
      }

      [TestMethod]
      public void IsPrimitiveByteEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(byte)));
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(Byte)));
      }

      [TestMethod]
      public void IsPrimitiveSByteEvaluatesTrue()
      {
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(sbyte)));
         Assert.IsTrue(TypeHelper.IsPrimitive(typeof(SByte)));
      }
   }
}
