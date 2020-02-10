namespace ChessMind
{
    public class Position 
    {
        public byte Row { get; }
        public byte Column { get; }


        public Position(byte row, byte column)
        {
            if (row > 7 || column > 7) {
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