using System;
using System.Collections.Generic;
using System.IO;

namespace EdgeEditLib.Blocks
{
    public class FallingPlatform
    {
        public Vec position;
        public short float_time;

        public FallingPlatform()
        {

        }

        public FallingPlatform(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            position = reader.ReadVec();
            float_time = reader.ReadInt16();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(position);
            writer.Write(float_time);
        }
    }
}
