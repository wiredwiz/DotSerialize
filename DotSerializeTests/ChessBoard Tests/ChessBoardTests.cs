using System;
using ApprovalTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;

namespace Org.Edgerunner.DotSerialize.Tests.Chessboard_Unit_Tests
{
   [TestClass]
   public class ChessBoardTests
   {
      private const string IntialBoardSerializesProperly_Approved =
         "ChessBoardTests.InitialBoardSerializesProperly.approved.xml";

      [TestCleanup]
      private void CleanUp()
      {
         Utilities.DeleteFile(IntialBoardSerializesProperly_Approved);
         //Utilities.DeleteFile(SerializeDogOmittingTypesResultsInProperOutput_Approved);
         //Utilities.DeleteFile(SerializeCatOmittingReferentialIntegrityResultsInProperOutput_Approved);
         //Utilities.DeleteFile(SerializeCatDECultureResultsInProperOutput_Approved);
         //Utilities.DeleteFile(SerializeCatWithoutMapResultsInProperOutput_Approved);
         //Utilities.DeleteFile(SerializeCatWithMap1ResultsInProperOutput_Approved);
         //Utilities.DeleteFile(SerializeCatWithMap2ResultsInProperOutput_Approved);
      }

      [TestInitialize]
      private void Setup()
      {
         Utilities.ExtractEmbeddedFile(IntialBoardSerializesProperly_Approved);
         //Utilities.ExtractEmbeddedFile(SerializeDogOmittingTypesResultsInProperOutput_Approved);
         //Utilities.ExtractEmbeddedFile(SerializeCatOmittingReferentialIntegrityResultsInProperOutput_Approved);
         //Utilities.ExtractEmbeddedFile(SerializeCatDECultureResultsInProperOutput_Approved);
         //Utilities.ExtractEmbeddedFile(SerializeCatWithoutMapResultsInProperOutput_Approved);
         //Utilities.ExtractEmbeddedFile(SerializeCatWithMap1ResultsInProperOutput_Approved);
         //Utilities.ExtractEmbeddedFile(SerializeCatWithMap2ResultsInProperOutput_Approved);
      }

      [TestMethod]
      public void InitialBoardSerializesProperly()
      {
         var board = new ChessBoard();
         var serializer = new Serializer();
         string xml = serializer.SerializeObject(board);
         Approvals.VerifyXml(xml);
      }

      [TestMethod]
      public void InitialBoardDeserializesProperly()
      {
         var board = new ChessBoard();
         var serializer = new Serializer() { Settings = new Settings { UseTypeConstructors = true } };
         string xml = serializer.SerializeObject(board);
         var board2 = serializer.DeserializeObject<ChessBoard>(xml);
         for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
               Assert.AreEqual(board.Position[i, j], board2.Position[i, j]);
      }
   }
}
