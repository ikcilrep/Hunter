using NUnit.Framework;
using System;
namespace Chess.Tests
{
    [TestFixture]
    public partial class PositionTests
    {
        [TestCase("a4")]
        [TestCase("c7")]
        [TestCase("d5")]
        [TestCase("h8")]
        [TestCase("a1")]
        public void ToString_CorrectNotation_ReturnSameField(string positionString)
        {
            Assert.AreEqual(new Position(positionString).ToString(), positionString);
        }

        [TestCase("a4", 0, 3)]
        [TestCase("c7", 2, 6)]
        [TestCase("d5", 3, 4)]
        [TestCase("h8", 7, 7)]
        [TestCase("a1", 0, 0)]
        public void ToString_CorrectNotation_ReturnCorrectCoordinates(string positionString, int column, int row)
        {
            var position = new Position(positionString);
            Assert.AreEqual(position.Row, row);
            Assert.AreEqual(position.Column, column);
        }

        [TestCase("a9")]
        [TestCase("z3")]
        [TestCase("y9")]
        [TestCase("h0")]
        [TestCase(";1")]
        public void Constructor_IncorrectNotation_ThrowsArgumentException(string positionString)
        {
            Assert.Throws<ArgumentException>(() => new Position(positionString));
        }

        [TestCase("ab4")]
        [TestCase("c71")]
        public void Constructor_IncorrectNotation_ThrowsFormatException(string positionString)
        {
            Assert.Throws<FormatException>(() => new Position(positionString));
        }

        [TestCase(0, 3)]
        [TestCase(2, 6)]
        [TestCase(3, 4)]
        [TestCase(7, 7)]
        [TestCase(0, 0)]
        public void Constructor_CorrectCoordinates_ReturnGivenCoordinates(int row, int column)
        {
            var position = new Position((byte)row, (byte)column);
            Assert.AreEqual(position.Row, (byte)row);
            Assert.AreEqual(position.Column, (byte)column);
        }

        [TestCase(0, 9)]
        [TestCase(2, 27)]
        [TestCase(44, 124)]
        [TestCase(12, 3)]
        [TestCase(8, 8)]
        public void Constructor_IncorrectCoordinates_ThrowsArgumentException(int row, int column)
        {
            Assert.Throws<ArgumentException>(() => new Position((byte)row, (byte)column));
        }


    }
}