using System;
using System.Collections.Generic;

namespace ChessMind
{
    public class Position
    {
        public static byte MaxRow {get => 7;} 
        public static byte MaxColumn {get => 7;} 
        public static byte MinRow {get => 0;} 
        public static byte MinColumn {get => 0;} 

        public byte Row { get; }
        public byte Column { get; }


        public Position(byte row, byte column)
        {
            if (row > MaxRow || column > MaxColumn) {
                throw new System.ArgumentException("Coordinates out of range.");
            }
            Row = row;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            if (obj is Position) {
                return ((Position)obj) == this;
            }
            return false; 
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Position a, Position b) => a.Row == b.Row && a.Column == a.Column;
        public static bool operator !=(Position a, Position b) => !(a == b);

        private bool InRange(byte value, byte min, byte max) => value >= min && value <= max;
        public static HashSet<Position> Range(Position position1, Position position2)
        {
            byte minRow = Math.Min(position1.Row, position2.Row);
            byte maxRow = Math.Max(position1.Row, position2.Row);
            byte minColumn = Math.Min(position1.Column, position2.Column);
            byte maxColumn = Math.Max(position1.Column, position2.Column);
            var result = new HashSet<Position>();
            for (var row = minRow; row <= maxRow; row++) {
                for (var column = minColumn; column <= maxColumn; column++)
                {
                    result.Add(new Position(row, column));
                }
            }
            return result;
        }
    }
}