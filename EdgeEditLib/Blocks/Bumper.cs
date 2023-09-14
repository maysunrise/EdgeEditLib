using System;
using System.Collections.Generic;
using System.IO;

namespace EdgeEditLib.Blocks
{
    public class Bumper
    {

        public bool enabled;
        public Vec position;
        public BumperSide north; // assuming north as -Y in blockspace, top-right in screenspace
        public BumperSide east;
        public BumperSide south;
        public BumperSide west;

        public Bumper()
        {
        }

        public Bumper(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            enabled = reader.ReadBoolean();
            position = reader.ReadVec();
            north = new BumperSide(reader);
            east = new BumperSide(reader);
            south = new BumperSide(reader);
            west = new BumperSide(reader);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(enabled);
            writer.Write(position);
            north.Write(writer);
            east.Write(writer);
            south.Write(writer);
            west.Write(writer);
        }

        public struct BumperSide
        {
            public short start_delay;
            public short pulse_rate;

            public BumperSide(short startDelay, short pulseRate)
            {
                start_delay = startDelay;
                pulse_rate = pulseRate;
            }

            public BumperSide(BinaryReader reader)
            {
                start_delay = reader.ReadInt16();
                pulse_rate = reader.ReadInt16();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write(start_delay);
                writer.Write(pulse_rate);
            }

        }
    }
}
