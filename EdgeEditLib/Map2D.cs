using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace EdgeEditLib
{
    /// <summary>
    /// Stores a bit array containing data about vertical slice of the layer
    /// </summary>
    public class Map2D
    {
        public readonly ushort Width;

        public readonly ushort Length;

        private BitArray data;

        public Map2D(int width, int length)
        {
            Width = (ushort)width;
            Length = (ushort)length;
            data = new BitArray(GetBytes(Width, Length) << 3);
        }

        public Map2D(BinaryReader reader, int width, int length)
        {
            Width = (ushort)width;
            Length = (ushort)length;
            data = new BitArray(reader.ReadBytes(GetBytes(Width, Length)));
        }

        public int Bytes => GetBytes(Width, Length);

        private int GetBytes(int width, int length)
        {
            return (width * length + 7) >> 3;
        }

        private int GetPosition(int x, int y)
        {
            int pos = y * Width + x, posBit = pos & 7;  // posBase = pos & ~7 = pos - posBit
            return pos + 7 - posBit - posBit;           // return posBase + (7 - posBit);
        }

        public bool this[int x, int y]
        {
            get { return data[GetPosition(x, y)]; }
            set { data[GetPosition(x, y)] = value; }
        }

        public void Write(BinaryWriter writer)
        {
            byte[] array = new byte[Bytes];
            data.CopyTo(array, 0);
            writer.Write(array);
        }
    }
}
