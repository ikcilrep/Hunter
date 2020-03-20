using System.Linq;
using Chess.Moves;
using Chess.Pieces;
using NUnit.Framework;

namespace Chess.Tests
{
    [TestFixture]
    public class BishopTests
    {
        private static Board _board;

        [SetUp]
        public void Init()
        {
            _board = new Board();
            _board.MakeMove(new Move(_board["e2"], new Position("e4"), _board));
            _board.MakeMove(new Move(_board["e7"], new Position("e5"), _board));
            _board.MakeMove(new Move(_board["d2"], new Position("d4"), _board));
            _board.MakeMove(new Move(_board["d7"], new Position("d5"), _board));
            _board.MakeMove(new Move(_board["c1"], new Position("g5"), _board));
            _board.MakeMove(new Move(_board["c7"], new Position("c6"), _board));
            _board.MakeMove(new Move(_board["f1"], new Position("c4"), _board));
            _board.MakeMove(new Move(_board["a7"], new Position("a6"), _board));
        }

        [TestCase("c4", new string[] { "Bb5", "Bxa6", "Bxd5", "Bb3", "Bd3", "Be2", "Bf1" })]
        [TestCase("g5", new string[] { "Bf6", "Be7", "Bxd8", "Bh4", "Bh6", "Bf4", "Be3", "Bd2", "Bc1" })]
        public void PossibleMoves_ReturnCorrectMoves(string bishopPositionString, string[] expectedPossibleMoves)
        {
            var bishop = (Bishop)_board[bishopPositionString];
            var possibleMoves = bishop.PossibleMoves(_board).Select(m => m.ToString()).ToHashSet();
            var expectedPossibleMovesSet = expectedPossibleMoves.ToHashSet();
            Assert.That(possibleMoves.SetEquals(expectedPossibleMovesSet));
        }


        [TestCase("c4", "Bxa6")]
        [TestCase("c4", "Bb3")]
        [TestCase("g5", "Bxd8")]
        [TestCase("g5", "Bf6")]

        public void IsMovePossible_CorrectPossibleMove_ReturnTrue(string bishopPositionString, string moveString)
        {
            var bishop = (Bishop)_board[bishopPositionString];
            var move = MoveParser.ParseMove(moveString, _board);
            Assert.IsTrue(bishop.IsMovePossible(move));
        }
    }
}