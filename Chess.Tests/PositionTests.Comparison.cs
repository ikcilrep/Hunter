using NUnit.Framework;
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
        public void EqualsOperator_SameCoordinatesPositions_ReturnsTrue(string notation)
        {
            var position1 = new Position(notation);
            var position2 = new Position(notation);
            Assert.IsTrue(position1 == position2);
        }

        [TestCase("a4", "a3")]
        [TestCase("c7", "d7")]
        [TestCase("d5", "b1")]
        [TestCase("h8", "c8")]
        public void EqualsOperator_DifferentCoordinatesPositions_ReturnsFalse(string notation1, string notation2)
        {
            var position1 = new Position(notation1);
            var position2 = new Position(notation2);
            Assert.IsFalse(position1 == position2);
        }
    }
}