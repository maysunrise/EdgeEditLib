using System;
using System.Collections.Generic;
using System.IO;

namespace EdgeEditLib.Blocks
{
    public class CameraTrigger
    {
        public Vec position;
        public short zoom;
        public byte radius_x;
        public byte radius_y;
        // if zoom == -1
        public bool reset;
        public short start_delay;
        public short duration;
        public short value;
        public bool single_use;
        public bool value_is_angle; // value is field of view if false
                                    // endif

        public CameraTrigger()
        {

        }

        public CameraTrigger(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            position = reader.ReadVec();
            zoom = reader.ReadInt16();
            radius_x = reader.ReadByte();
            radius_y = reader.ReadByte();
            if (zoom == -1)
            {
                reset = reader.ReadBoolean();
                start_delay = reader.ReadInt16();
                duration = reader.ReadInt16();
                value = reader.ReadInt16();
                single_use = reader.ReadBoolean();
                value_is_angle = reader.ReadBoolean();
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(position);
            writer.Write(zoom);
            writer.Write(radius_x);
            writer.Write(radius_y);
            if (zoom == -1)
            {
                writer.Write(reset);
                writer.Write(start_delay);
                writer.Write(duration);
                writer.Write(value);
                writer.Write(single_use);
                writer.Write(value_is_angle);
            }
        }
    }
}
