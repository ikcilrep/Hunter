using NUnit.Framework;
using Chess;
namespace Chess.Tests
{
    public class Position_ToStringTests 
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
    }
}