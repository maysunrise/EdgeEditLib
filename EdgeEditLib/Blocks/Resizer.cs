using System;
using System.Collections.Generic;
using System.IO;


namespace EdgeEditLib.Blocks
{
    public class Resizer
    {
        public Vec position;
        public bool visible;
        public byte direction;

        public Resizer()
        {

        }

        public Resizer(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            position = reader.ReadVec();
            visible = reader.ReadBoolean();
            direction = reader.ReadByte();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(position);
            writer.Write(visible);
            writer.Write(direction);
        }
    }
}
