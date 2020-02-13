using System;
using System.Collections.Generic;

namespace Chess
{
    public static class Positions
    {
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

        // Returns positions on diagonal from position1 to position2, exluding position1.
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
            result.Add(position2);
            return result;
        }

        public static byte GetPawnRow(bool color) { 
            if (color) {
                return (byte)(Position.MinRow+1);
            }

            return (byte)(Position.MaxRow-1);
        }    
        public static byte Forward(byte row, byte distance, bool color) { 
            if (color)
            {
                return (byte)(row + distance);
            }
            return (byte)(row - distance);
        }


    }
}
