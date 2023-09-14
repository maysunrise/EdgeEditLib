using System;
using System.Collections.Generic;
using System.IO;

namespace EdgeEditLib.Blocks
{
    public class Prism
    {
        public Vec position;
        public byte energy; // deprecated, do not use

        public Prism()
        {

        }

        public Prism(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            position = reader.ReadVec();
            energy = reader.ReadByte();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(position);
            writer.Write(energy);
        }
    }
}
