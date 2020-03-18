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
        private Board _whiteMoveBoard;
        private Board _blackMoveBoard;

        [SetUp]
        public void Init()
        {
            _whiteMoveBoard = new Board();
            _whiteMoveBoard.MakeMove(new Move(_whiteMoveBoard["e2"], new Position("e4"), _whiteMoveBoard));
            _whiteMoveBoard.MakeMove(new Move(_whiteMoveBoard["d7"], new Position("d5"), _whiteMoveBoard));
            _whiteMoveBoard.MakeMove(new Move(_whiteMoveBoard["b1"], new Position("c3"), _whiteMoveBoard));
            _whiteMoveBoard.MakeMove(new Move(_whiteMoveBoard["g8"], new Position("f6"), _whiteMoveBoard));
            _whiteMoveBoard.MakeMove(new Move(_whiteMoveBoard["h2"], new Position("h4"), _whiteMoveBoard));
            _whiteMoveBoard.MakeMove(new Move(_whiteMoveBoard["a7"], new Position("a6"), _whiteMoveBoard));
            _whiteMoveBoard.MakeMove(new Move(_whiteMoveBoard["h4"], new Position("h5"), _whiteMoveBoard));
            _whiteMoveBoard.MakeMove(new Move(_whiteMoveBoard["g7"], new Position("g5"), _whiteMoveBoard));

            _blackMoveBoard = new Board();
            _blackMoveBoard.MakeMove(new Move(_blackMoveBoard["e2"], new Position("e4"), _blackMoveBoard));
            _blackMoveBoard.MakeMove(new Move(_blackMoveBoard["d7"], new Position("d5"), _blackMoveBoard));
            _blackMoveBoard.MakeMove(new Move(_blackMoveBoard["b1"], new Position("c3"), _blackMoveBoard));
            _blackMoveBoard.MakeMove(new Move(_blackMoveBoard["g8"], new Position("f6"), _blackMoveBoard));
            _blackMoveBoard.MakeMove(new Move(_blackMoveBoard["f1"], new Position("d3"), _blackMoveBoard));
        }

        private static void ParseMove_CorrectNotation_ReturnExpectedMove(string moveString,
                                                                         string expectedFromPositionString,
                                                                         string expectedToPositionString,
                                                                         bool isEnPassant,
                                                                         Board board)
        {

            IMove expectedMove;
            if (isEnPassant)
            {
                expectedMove = new EnPassant((Pawn)board[expectedFromPositionString], new Position(expectedToPositionString), board);
            }
            else
            {
                expectedMove = new Move(board[expectedFromPositionString], new Position(expectedToPositionString), board);
            }
            var move = MoveParser.ParseMove(moveString, board);

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
            Assert.Throws<FormatException>(() => MoveParser.ParseMove(moveString, _whiteMoveBoard));
        }

        [TestCase("Nd5")]
        [TestCase("Nxb5")]
        [TestCase("Na6")]
        [TestCase("d5")]
        public void ParseMove_IncorrectMove_ThrowArgumentException(string moveString)
        {
            Assert.Throws<ArgumentException>(() => MoveParser.ParseMove(moveString, _whiteMoveBoard));
        }

        [TestCase("Nxd5", "c3", "d5", false)]
        [TestCase("exd5", "e4", "d5", false)]
        [TestCase("e5", "e4", "e5", false)]
        [TestCase("Bb5", "f1", "b5", false)]
        [TestCase("hxg6", "h5", "g6", true)]

        public void ParseMove_CorrectNotationWhiteMoves_ReturnExpectedMove(string moveString, string expectedFromPositionString, string expectedToPositionString, bool isEnPassant)
        {
            ParseMove_CorrectNotation_ReturnExpectedMove(moveString, expectedFromPositionString, expectedToPositionString, isEnPassant, _whiteMoveBoard);
        }


        [TestCase("Nxe4", "f6", "e4")]
        [TestCase("dxe4", "d5", "e4")]
        [TestCase("d4", "d5", "d4")]
        [TestCase("Bg4", "c8", "g4")]


        public void ParseMove_CorrectNotationBlackMoves_ReturnExpectedMove(string moveString, string expectedFromPositionString, string expectedToPositionString)
        {
            ParseMove_CorrectNotation_ReturnExpectedMove(moveString, expectedFromPositionString, expectedToPositionString, false, _blackMoveBoard);
        }

        [TestCase("Nxd5")]
        [TestCase("exd5")]
        [TestCase("e5")]
        [TestCase("Bb5")]
        [TestCase("hxg6")]


        public void ParseMove_CorrectNotation_ConvertsBackToSameStringRepresentation(string moveString)
        {
            var move = MoveParser.ParseMove(moveString, _whiteMoveBoard);
            Assert.AreEqual(moveString, move.ToString());
        }
    }
}