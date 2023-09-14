using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EdgeEditLib
{
    // Inspiration https://github.com/Mygod/EDGE/blob/master/EdgeTool/Core/Level/Space.cs
    /// <summary>
    /// A list of Map2D is stored here, each Map2D representing a vertical slice of the level layer. For example "this[0]" is always ground
    /// </summary>
    public class Map3D : List<Map2D>
    {
        public readonly ushort Width;
        public readonly ushort Length;
        public readonly ushort Height;
        public Map3D(int width, int length, int height)
        {
            Width = (ushort)width;
            Length = (ushort)length;
            Height = (ushort)height;
            for (var z = 0; z < Height; z++) Add(new Map2D(Width, Length));
        }

        public Map3D(BinaryReader reader, int width, int length, int height)
        {
            Width = (ushort)width;
            Length = (ushort)length;
            Height = (ushort)height;
            for (var z = 0; z < Height; z++)
            {
                Add(new Map2D(reader, Width, Length));
            }
        }

        /// <summary>
        /// Returns the highest point at specified position
        /// </summary>
        public int GetHeightAt(int x, int y)
        {
            for (var z = Height - 1; z >= 0; z--)
            {
                if (this[x, y, z])
                {
                    return z;
                }
            }
            return 0;
        }

        /// <summary>
        /// Returns true if y is 0. Useful for checking for a half block
        /// </summary>
        public bool IsGroundLevel(int y)
        {
            return y == 0;
        }

        // Z is height
        /// <summary>
        /// Returns true if a block exists at specified position
        /// </summary>
        public bool this[int x, int y, int z]
        {
            get { return IsInBounds(x, y, z) && this[z][x, y]; }
            set { this[z][x, y] = value; }
        }

        /// <summary>
        /// Returns true if position is in the bounds of the level
        /// </summary>
        public bool IsInBounds(int x, int y, int z)
        {
            return x >= 0 && y >= 0 && z >= 0 && x < Width && y < Length && z < Height;
        }

        public void Write(BinaryWriter writer)
        {
            for (var z = 0; z < Height; z++)
            {
                this[z].Write(writer);
            }
        }
    }
}
