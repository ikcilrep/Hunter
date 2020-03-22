using System.Linq;
using Chess.Moves;
using Chess.Pieces;
using NUnit.Framework;

namespace Chess.Tests
{
    [TestFixture]
    public class KnightTests
    {
        private static Board _board;

        [SetUp]
        public void Init()
        {
            _board = new Board();
            _board.MakeMove(new Move(_board["g1"], new Position("f3"), _board));
            _board.MakeMove(new Move(_board["e7"], new Position("e5"), _board));
            _board.MakeMove(new Move(_board["b1"], new Position("c3"), _board));
            _board.MakeMove(new Move(_board["d7"], new Position("d5"), _board));
        }

        [TestCase("f3", new string[] { "Nxe5", "Nd4", "Ng5", "Nh4", "Ng1"})]
        [TestCase("c3", new string[] { "Nxd5", "Ne4", "Nb5", "Na4", "Nb1"})]
        public void PossibleMoves_ReturnCorrectMoves(string knightPositionString, string[] expectedPossibleMoves)
        {
            var knight = (Knight)_board[knightPositionString];
            var possibleMoves = knight.PossibleMoves(_board).Select(m => m.ToString()).ToHashSet();
            var expectedPossibleMovesSet = expectedPossibleMoves.ToHashSet();
            Assert.That(possibleMoves.SetEquals(expectedPossibleMovesSet));
        }


        [TestCase("f3", "Nxe5")]
        [TestCase("f3", "Nd4")]
        [TestCase("c3", "Nxd5")]
        [TestCase("c3", "Ne4")]


        public void IsMovePossible_PossibleMove_ReturnTrue(string knightPositionString, string moveString)
        {
            var knight = (Knight)_board[knightPositionString];
            var move = MoveParser.ParseMove(moveString, _board);
            Assert.IsTrue(knight.IsMovePossible(move));
        }

    }
}