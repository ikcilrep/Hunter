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
            return Positions.Forward(Row, distance, color);
        }

        public Position Behind(bool color) {
            return Before(!color);
        }
        public Position Before(bool color) {
            return new Position(Forward(1, color), Column);
        }

        public static bool operator ==(Position a, Position b) => a.Row == b.Row && a.Column == a.Column;
        public static bool operator !=(Position a, Position b) => !(a == b);


    }
}