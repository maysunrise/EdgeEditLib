using System;
using System.Collections.Generic;
using System.IO;

namespace EdgeEditLib.Blocks
{
    public class OtherCube
    {
        public Vec position_trigger;
        public short moving_block_sync; // id of a looped moving block to sync to. -1 for no sync.

        // if moving_block_sync == -2 (then it's a dark cube)
        public byte darkcube_radius_x;
        public byte darkcube_radius_y;
        public short darkcube_moving_block_sync;
        // endif
        //public short key_event_count;
        public Vec position_cube;
        public List<KeyEvent> key_events = new List<KeyEvent>(); // interesting that this isn't immediately after its length...

        public OtherCube()
        {

        }

        public OtherCube(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            position_trigger = reader.ReadVec();
            moving_block_sync = reader.ReadInt16();
            if (moving_block_sync == -2)
            {
                darkcube_radius_x = reader.ReadByte();
                darkcube_radius_y = reader.ReadByte();
                darkcube_moving_block_sync = reader.ReadInt16();
            }
            int key_event_count = reader.ReadInt16();
            position_cube = reader.ReadVec();

            for (int i = 0; i < key_event_count; i++)
            {
                KeyEvent keyEvent = new KeyEvent();

                keyEvent.time_offset = reader.ReadInt16();
                keyEvent.direction = reader.ReadByte();
                keyEvent.event_type = reader.ReadByte();

                key_events.Add(keyEvent);
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(position_trigger);
            writer.Write(moving_block_sync);
            if (moving_block_sync == -2)
            {
                writer.Write(darkcube_radius_x);
                writer.Write(darkcube_radius_y);
                writer.Write(darkcube_moving_block_sync);
            }
            writer.Write((short)key_events.Count);
            writer.Write(position_cube);
            for (int i = 0; i < key_events.Count; i++)
            {
                writer.Write(key_events[i].time_offset);
                writer.Write(key_events[i].direction);
                writer.Write(key_events[i].event_type);
            }
        }

        public struct KeyEvent
        {
            public short time_offset;
            public byte direction;
            // direction == 0: west
            // direction == 1: east
            // direction == 2: north
            // direction == 3: south
            // assuming, as earlier, north to be -Y in blockspace, top-right in screenspace
            public byte event_type;
            // event_type == 0: key down
            // event_type == 1: key up

            public KeyEvent(short timeOffset, byte moveDirection, byte eventType)
            {
                time_offset = timeOffset;
                direction = moveDirection;
                event_type = eventType;
            }
        }

    }
}
