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
        public void EqualsOperator_SameCoordinatesPositions_ReturnTrue(string positionString)
        {
            var position1 = new Position(positionString);
            var position2 = new Position(positionString);
            Assert.IsTrue(position1 == position2);
        }

        [TestCase("a4", "a3")]
        [TestCase("c7", "d7")]
        [TestCase("d5", "b1")]
        [TestCase("h8", "c8")]
        public void EqualsOperator_DifferentCoordinatesPositions_ReturnFalse(string positionString1, string positionString2)
        {
            var position1 = new Position(positionString1);
            var position2 = new Position(positionString2);
            Assert.IsFalse(position1 == position2);
        }
    }
}