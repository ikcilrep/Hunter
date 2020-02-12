﻿namespace Chess
{
    public class Move
    {

        public Piece Piece { get; }
        public Position To { get; }
        public bool IsCapture { get; }
        public bool IsEnPassant { get; }



        public Move(Piece piece, Position to, Board board)
        {
            Piece = piece;
            To = to;
            IsEnPassant = piece is Pawn && Pawn.IsEnPassant(this, board);
            IsCapture = IsEnPassant || board.IsTherePieceOfColor(To, !Piece.Color);
        } 
        
        public Move(Piece piece, Position to, bool isCapture)
        {
            Piece = piece;
            To = to;
            IsEnPassant = false;
            IsCapture = isCapture;
        } 

    }
}