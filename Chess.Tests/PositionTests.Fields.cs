using System.Linq;
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Chess.Tests
{
    [TestFixture]
    public partial class PositionTests
    {
        [TestCase("e2", Board.White, 2, 3)]
        [TestCase("a1", Board.White, 7, 7)]
        [TestCase("h8", Board.Black, 3, 4)]
        [TestCase("a2", Board.Black, 1, 0)]
        public void Forward_CorrectSituation_ReturnExpectedRow(string positionString, bool color, int distance, int expectedRow)
        {
            var position = new Position(positionString);
            Assert.AreEqual((byte)expectedRow, position.Forward((byte)distance, color));
        }

        [TestCase("e2", 7, 3, "h2")]
        [TestCase("d6", 0, 3, "a6")]
        [TestCase("e2", 6, 0, "e2")]
        [TestCase("h6", 1, 7, "a6")]
        public void GoInDirectionOf_CorrectSituation_ReturnExpectedPosition(string positionString, int column, int distance, string expectedPositionString)
        {
            var position = new Position(positionString);
            var expectedPosition = new Position(expectedPositionString);
            Assert.AreEqual(expectedPositionString, position.GoInDirectionOf((byte)column, (byte)distance).ToString());
        }

        [TestCase("h1", "a8", new string[] { "g2", "f3", "e4", "d5", "c6", "b7", "a8" })]
        [TestCase("a8", "h1", new string[] { "b7", "c6", "d5", "e4", "f3", "g2", "h1" })]
        [TestCase("f1", "h3", new string[] { "g2", "h3" })]
        [TestCase("h3", "f1", new string[] { "g2", "f1" })]
        [TestCase("d4", "d4", new string[] { })]
        public void Diagonal_CorrectSituation_ReturnExpectedPositions(string positionFromString, string positionToString, string[] expectedPositionsStrings)
        {
            var from = new Position(positionFromString);
            var to = new Position(positionToString);
            var diagonalPositions = Positions.Diagonal(from, to);
            Assert.AreEqual(diagonalPositions.Select(p => p.ToString()).ToArray(), expectedPositionsStrings);
        }

        [TestCase("f2", "b2")]
        [TestCase("d4", "b5")]
        [TestCase("e3", "g6")]
        public void Diagonal_IncorrectSituation_ThrowsArgumentException(string positionFromString, string positionToString)
        {
            var from = new Position(positionFromString);
            var to = new Position(positionToString);
            Assert.Throws<ArgumentException>(() => Positions.Diagonal(from, to));
        }

        [TestCase("f2", "c4", new string[] { "f2", "e2", "d2", "c2", "f3", "e3", "d3", "c3", "f4", "e4", "d4", "c4" })]
        [TestCase("d5", "c6", new string[] { "d5", "c5", "d6", "c6" })]
        [TestCase("a1", "a1", new string[] { "a1" })]
        public void Range_CorrectSituation_ReturnExpectedSetOfPositions(string corner1PositionString, string corner2PositionString, string[] expectedSetOfPositions)
        {
            var corner1 = new Position(corner1PositionString);
            var corner2 = new Position(corner2PositionString);
            var range = Positions.Range(corner1, corner2).Select(p => p.ToString()).ToHashSet();
            var expectedRange = new HashSet<String>(expectedSetOfPositions).ToHashSet();
            Assert.That(range.SetEquals(expectedRange));

        }


    }
}
