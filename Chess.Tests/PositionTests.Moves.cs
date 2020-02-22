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

        [TestCase("e2", 7, 3, "h2")]
        [TestCase("d6", 0, 3, "a6")]
        [TestCase("e2", 6, 0, "e2")]
        [TestCase("h6", 1, 7, "a6")]
        public void GoInDirectionOf_CorrectSituation_ReturnsExpectedPosition(string positionString, int column, int distance, string expectedPositionString) {
            var position = new Position(positionString);
            var expectedPosition = new Position(expectedPositionString); 
            Assert.AreEqual(expectedPositionString, position.GoInDirectionOf((byte) column, (byte) distance).ToString());
        }
    }
}
