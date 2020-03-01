using Chess.Moves;
using NUnit.Framework;
namespace Chess.Tests
{
    [TestFixture]
    public class MoveParserTests
    {
        private Board _whiteMoveBoard;
        private Board _blackMoveBoard;

        [SetUp]
        public void Init()
        {
            _whiteMoveBoard = new Board();
            _blackMoveBoard = new Board();
            _blackMoveBoard.MakeMove(new Move(_blackMoveBoard["e2"], new Position("e4"), _blackMoveBoard));
        }

        private static void ParseMove_CorrectNotation_ReturnExpectedMove(string moveString, string expectedFromPositionString, string expectedToPositionString, Board board)
        {
            var expectedMove = new Move(board[expectedFromPositionString], new Position(expectedToPositionString), board);
            var move = MoveParser.ParseMove(moveString, board);

            Assert.That(move.Equals(expectedMove));

        }

        [TestCase("Nf3", "g1", "f3")]
        [TestCase("e4", "e2", "e4")]
        [TestCase("e3", "e2", "e3")]

        public void ParseMove_CorrectNotationWhiteMoves_ReturnExpectedMove(string moveString, string expectedFromPositionString, string expectedToPositionString)
        {
            ParseMove_CorrectNotation_ReturnExpectedMove(moveString, expectedFromPositionString, expectedToPositionString, _whiteMoveBoard);
        }

        [TestCase("Nf6", "g8", "f6")]
        [TestCase("e5", "e7", "e5")]
        [TestCase("e6", "e7", "e6")]

        public void ParseMove_CorrectNotationBlackMoves_ReturnExpectedMove(string moveString, string expectedFromPositionString, string expectedToPositionString)
        {
            ParseMove_CorrectNotation_ReturnExpectedMove(moveString, expectedFromPositionString, expectedToPositionString, _blackMoveBoard);
        }
    }
}