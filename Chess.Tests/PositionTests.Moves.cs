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
        public void Forward_CorrectSituation_ReturnsExpectedRow(string positionString, bool color, int distance, int expectedRow) {
            var position = new Position(positionString);
            Assert.AreEqual((byte)expectedRow, position.Forward((byte) distance, color));
        }  
    }
}
