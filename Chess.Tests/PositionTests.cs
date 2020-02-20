using NUnit.Framework;
using System;
using Chess;
namespace Chess.Tests
{
    public class PositionTests 
    {
        [TestCase("a4")]
        [TestCase("c7")]
        [TestCase("d5")]
        [TestCase("h8")]
        [TestCase("a1")]
        public void ToString_CorrectNotation_ReturnSameField(string notation)
        {
            Assert.AreEqual(new Position(notation).ToString(), notation);
        }

        [TestCase("a9")]
        [TestCase("z3")]
        [TestCase("y9")]
        [TestCase("h0")]
        [TestCase(";1")]
        public void Constructor_IncorrectNotation_ThrowsArgumentException(string notation)
        {
            Assert.Throws<ArgumentException>(() => new Position(notation));
        }
    }
}