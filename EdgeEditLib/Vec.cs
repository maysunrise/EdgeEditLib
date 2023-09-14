
namespace EdgeEditLib
{
    /// <summary>
    /// <para>Basically vector [X, Y, Z]</para>
    /// <para>Z axis used for up or down in Edge</para>
    /// </summary>
    public struct Vec
    {
        public short X;
        public short Y;
        public short Z;

        public Vec(short x, short y, short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec(int x, int y, int z)
        {
            X = (short)x;
            Y = (short)y;
            Z = (short)z;
        }

        public bool EqualsXYZ(int x, int y, int z)
        {
            return X == x && Y == y && Z == z;
        }

        public short[] ToArray()
        {
            return new short[3] { X, Y, Z };
        }

        public override string ToString()
        {
            return $"[{X}, {Y}, {Z}]";
        }
    }
}
