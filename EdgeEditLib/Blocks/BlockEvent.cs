using System;
using System.Collections.Generic;
using System.IO;

namespace EdgeEditLib.Blocks
{
    public class BlockEvent
    {
        public byte type;
        public short block_id;
        public short payload;
        /*
                type == 0:
                        affects moving_platforms[block_id]
                        payload == 0:
                                traverse all waypoints
                        payload != 0:
                                traverse `payload` waypoints.
                type == 1:
                        affects bumpers[block_id]
                        payload == 0:
                                if bumper is running, stop it
                                else fire it once
                        payload == 1:
                                start the bumper and enable looping
                type == 2:
                        triggers achievements.
                        block_id is the achievement ID
                        payload is additional metadata that varies between different achievements.
                type == 3:
                        affects buttons[block_id]
                        payload == 0:
                                enable the button (pop it up)
                        payload == 1:
                                disable the button
        */
        public BlockEvent()
        {

        }

        public BlockEvent(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            type = reader.ReadByte();
            block_id = reader.ReadInt16();
            payload = reader.ReadInt16();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(type);
            writer.Write(block_id);
            writer.Write(payload);
        }
    }
}
