namespace ChessMind
{
    public class Position
    {
        public static byte MaxRow {get => 7;} 
        public static byte MaxColumn {get => 7;} 
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
    }
}