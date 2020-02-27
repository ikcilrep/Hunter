namespace Chess
{
    public class Position
    {
        public static byte MaxRow { get => 7; }
        public static byte MaxColumn { get => 7; }
        public static byte MinRow { get => 0; }
        public static byte MinColumn { get => 0; }

        public byte Row { get; }
        public byte Column { get; }

        private bool IsPositionRight => Row <= MaxRow && Row >= MinRow && Column <= MaxColumn && Column >= MinColumn;
        public Position(byte row, byte column)
        {
            Row = row;
            Column = column;
            if (!IsPositionRight)
            {
                throw new System.ArgumentException();
            }
        }

        public Position(string notation)
        {
            if (notation.Length != 2)
            {
                throw new System.FormatException();
            }
            Column = ParseColumn(notation[0]);
            Row = ParseRow(notation[1]);
        }

        public static byte ParseColumn(char letter)
        {
            if (letter >= 'a' && letter <= 'h')
            {
                return (byte)(letter - 97);
            }
            throw new System.FormatException();
        }

        public static byte ParseRow(char digit)
        {
            if (digit >= '1' && digit <= '8')
            {
                return (byte)(byte.Parse(digit.ToString()) - 1);
            }
            throw new System.FormatException();
        }

        public override bool Equals(object obj)
        {
            if (obj is Position)
            {
                return ((Position)obj) == this;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Row << 3) + Column;
        }

        public byte Forward(byte distance, bool color)
        {
            return Positions.Forward(Row, distance, color);
        }


        public Position Behind(bool color)
        {
            return Before(!color);
        }

        public Position Before(bool color)
        {
            return new Position(Forward(1, color), Column);
        }

        public static bool operator ==(Position a, Position b) => a.Row == b.Row && a.Column == b.Column;
        public static bool operator !=(Position a, Position b) => !(a == b);

        public Position GoInDirectionOf(byte column, byte distance)
        {
            if (column < Column)
            {
                return new Position(Row, (byte)(Column - distance));
            }
            return new Position(Row, (byte)(Column + distance));
        }

        public override string ToString()
        {
            return (char)(97 + Column) + (Row + 1).ToString();
        }


    }
}