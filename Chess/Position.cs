using System;
using System.Collections.Generic;

namespace Chess
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
            return (Row << 3) + Column;
        }

        public byte Forward(byte distance, bool color) {
            return Forward(Row, distance, color);
        }

        public Position Behind(bool color) {
            return Before(!color);
        }
        public Position Before(bool color) {
            return new Position(Forward(1, color), Column);
        }

        public static byte Forward(byte row, byte distance, bool color) { 
            if (color)
            {
                return (byte)(row + distance);
            }
            return (byte)(row - distance);
        }

        public static bool operator ==(Position a, Position b) => a.Row == b.Row && a.Column == a.Column;
        public static bool operator !=(Position a, Position b) => !(a == b);

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

        public static List<Position> Diagonal(Position position1, Position position2) 
        {
            var rowDistance = Math.Abs(position1.Row - position2.Row);
            var columnDistance = Math.Abs(position1.Column - position2.Column);
            if (rowDistance != columnDistance) 
            {
                throw new ArgumentException();
            }
            
            var rowDistanceSign = (byte) Math.Sign(position1.Row - position2.Row);
            var columnDistanceSign = (byte) Math.Sign(position1.Row - position2.Row);
            var result = new List<Position>();
            var row = (byte)(position1.Row - rowDistanceSign);
            var column = (byte)(position1.Column - columnDistanceSign); 
            while (row != position2.Row) 
            {
                result.Add(new Position(row, column));
                row -= rowDistanceSign;
                column -= columnDistanceSign;
            }
            return result;
        }

        public static byte GetPawnRow(bool color) { 
            if (color) {
                return (byte)(MinRow+1);
            }

            return (byte)(MaxRow-1);
        }
    }
}