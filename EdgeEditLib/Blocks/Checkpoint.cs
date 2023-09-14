using System;
using System.Collections.Generic;
using System.IO;

namespace EdgeEditLib.Blocks
{
    /// <summary>
    /// Rectangular trigger that saves player progress
    /// </summary>
    public class Checkpoint
    {
        public Vec position;
        public short respawn_z;
        public byte radius_x;
        public byte radius_y;

        public Checkpoint()
        {

        }

        public Checkpoint(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            position = reader.ReadVec();
            respawn_z = reader.ReadInt16();
            radius_x = reader.ReadByte();
            radius_y = reader.ReadByte();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(position);
            writer.Write(respawn_z);
            writer.Write(radius_x);
            writer.Write(radius_y);
        }
    }
}
