using System;
using System.Linq;
using Chess.Moves;
using Chess.Pieces;
using NUnit.Framework;
namespace Chess.Tests
{
    [TestFixture]
    public class MoveParserTests
    {
        private static Board _board;

        [SetUp]
        public void Init()
        {
            _board = new Board();
            _board.MakeMove(new Move(_board["e2"], new Position("e4"), _board));
            _board.MakeMove(new Move(_board["d7"], new Position("d5"), _board));
            _board.MakeMove(new Move(_board["b1"], new Position("c3"), _board));
            _board.MakeMove(new Move(_board["g8"], new Position("f6"), _board));
            _board.MakeMove(new Move(_board["h2"], new Position("h4"), _board));
            _board.MakeMove(new Move(_board["a7"], new Position("a6"), _board));
            _board.MakeMove(new Move(_board["h4"], new Position("h5"), _board));
            _board.MakeMove(new Move(_board["g7"], new Position("g5"), _board));
        }

        [TestCase("Nxd5", "c3", "d5", false)]
        [TestCase("exd5", "e4", "d5", false)]
        [TestCase("e5", "e4", "e5", false)]
        [TestCase("Bb5", "f1", "b5", false)]
        [TestCase("hxg6", "h5", "g6", true)]

        public static void ParseMove_CorrectNotation_ReturnExpectedMove(string moveString,
                                                                                 string expectedFromPositionString,
                                                                                 string expectedToPositionString,
                                                                                 bool isEnPassant)
        {

            IMove expectedMove;
            if (isEnPassant)
            {
                expectedMove = new EnPassant((Pawn)_board[expectedFromPositionString], new Position(expectedToPositionString), _board);
            }
            else
            {
                expectedMove = new Move(_board[expectedFromPositionString], new Position(expectedToPositionString), _board);
            }
            var move = MoveParser.ParseMove(moveString, _board);

            Assert.That(move.Equals(expectedMove));
        }

        [TestCase("Axd5")]
        [TestCase("Nwd5")]
        [TestCase("abcd")]
        [TestCase("ee")]
        [TestCase("e41")]
        [TestCase("Bb53")]
        public void ParseMove_IncorrectNotation_ThrowFormatException(string moveString)
        {
            Assert.Throws<FormatException>(() => MoveParser.ParseMove(moveString, _board));
        }

        [TestCase("Nd5")]
        [TestCase("Nxb5")]
        [TestCase("Na6")]
        [TestCase("d5")]
        public void ParseMove_IncorrectMove_ThrowArgumentException(string moveString)
        {
            Assert.Throws<ArgumentException>(() => MoveParser.ParseMove(moveString, _board));
        }

        [TestCase("Nxd5")]
        [TestCase("exd5")]
        [TestCase("e5")]
        [TestCase("Bb5")]
        [TestCase("hxg6")]


        public void ParseMove_CorrectNotation_ConvertsBackToSameStringRepresentation(string moveString)
        {
            var move = MoveParser.ParseMove(moveString, _board);
            Assert.AreEqual(moveString, move.ToString());
        }
    }
}