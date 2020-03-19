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
            _board.MakeMove(new Move(_board["a7"], new Position("a6"), _board));
            _board.MakeMove(new Move(_board["h2"], new Position("h4"), _board));
            _board.MakeMove(new Move(_board["a6"], new Position("a5"), _board));
            _board.MakeMove(new Move(_board["b2"], new Position("b4"), _board));
            _board.MakeMove(new Move(_board["a5"], new Position("a4"), _board));
            _board.MakeMove(new Move(_board["b4"], new Position("b5"), _board));
            _board.MakeMove(new Move(_board["a4"], new Position("a3"), _board));
            _board.MakeMove(new Move(_board["b5"], new Position("b6"), _board));
            _board.MakeMove(new Move(_board["b8"], new Position("c6"), _board));
            _board.MakeMove(new Move(_board["b6"], new Position("c7"), _board));
            _board.MakeMove(new Move(_board["a8"], new Position("b8"), _board));
            _board.MakeMove(new Move(_board["h4"], new Position("h5"), _board));
            _board.MakeMove(new Move(_board["g7"], new Position("g5"), _board));
        }

        [TestCase("Nxd5", "c3", "d5")]
        [TestCase("exd5", "e4", "d5")]
        [TestCase("e5", "e4", "e5")]
        [TestCase("Bb5", "f1", "b5")]

        public static void ParseMove_CorrectNotation_ReturnExpectedMove(string moveString,
                                                                        string expectedFromPositionString,
                                                                        string expectedToPositionString)
        {

            var expectedMove = new Move(_board[expectedFromPositionString], new Position(expectedToPositionString), _board);
            var move = MoveParser.ParseMove(moveString, _board);

            Assert.That(move.Equals(expectedMove));
        }


        [TestCase("hxg6", "h5", "g6")]
        public static void ParseMove_CorrectNotation_ReturnExpectedEnPassant(string moveString,
                                                                             string expectedFromPositionString,
                                                                             string expectedToPositionString)
        {

            var expectedMove = new EnPassant((Pawn)_board[expectedFromPositionString], new Position(expectedToPositionString), _board);
            var move = MoveParser.ParseMove(moveString, _board);
            Assert.That(move.Equals(expectedMove));

        }

        [TestCase("cxd8=Q", "c7", "d8", 'Q')]
        [TestCase("cxb8=N", "c7", "b8", 'N')]
        public static void ParseMove_CorrectNotation_ReturnExpectedPromotion(string moveString,
                                                                             string expectedFromPositionString,
                                                                             string expectedToPositionString,
                                                                             char expectedPromotedPawn)
        {
            var pawn = _board[expectedFromPositionString];
            Assert.That(pawn is Pawn);

            var expectedMove = new Promotion((Pawn)_board[expectedFromPositionString], PieceParser.ParsePiece(expectedPromotedPawn, pawn.Color), new Position(expectedToPositionString), _board);
            var move = MoveParser.ParseMove(moveString, _board);
            Assert.That(move is Promotion);
            var promotion = (Promotion)move;

            Assert.That(expectedMove.To == promotion.To);
            Assert.That(expectedMove.Piece == promotion.Piece);
            Assert.That(expectedMove.Board == promotion.Board);
            Assert.That(expectedMove.PromotedPawn.GetType() == promotion.PromotedPawn.GetType());
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