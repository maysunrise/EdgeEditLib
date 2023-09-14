using System;
using System.Collections.Generic;
using System.IO;

namespace EdgeEditLib.Blocks
{
    public class Button
    {
        public byte visibility;
        // visibility == 0: invisible
        // visibility == 1: visible, solid
        // visibility == 2: visible, ghosted
        public byte disable_count;
        // after a button has been pressed `disable_count` times, it cannot be re-enabled by an event.
        // disable_count = 0 can be re-enabled as many times as you like.
        public byte mode;
        // mode == 0: reverses the event when the button is released
        // mode == 1: event is permanent, button stays up when released
        // mode == 2: event is permanent, button stays down when released
        public short parent_id;
        public bool sequence_in_order;
        public byte siblings_count;
        public bool is_moving;
        // if is_moving
        public short moving_platform_id;
        // else
        public Vec position;
        // endif
        //public short event_count;
        public List<short> events = new List<short>();
        /*
                this is a tricky one so I feel it needs to be explained.
                a standalone button uses
                        visibility
                        disable_count
                        mode
                        is_moving (and related position system)
                        event_count
                        events

                a button sequence uses all of those and more.
                a button sequence consists of multiple buttons which when all pressed will activate a collection of events.
                a button sequence can insist that the buttons be pressed in a particular order.
                a button sequence has a 'parent' button and a series of 'child' buttons.
                only the parent trigger should have events tied to it.
                the parent and children should all have these properties:
                        sequence_in_order = <true/false>
                        mode = 2
                the parent has these properties:
                        parent_id = -1 (default)
                        siblings_count = <number of child buttons>
                        events = <array of event ids>
                the children have these properties:
                        parent_id = <index of the parent in the buttons array>
                        siblings_count = 0
        */

        public Button()
        {

        }

        public Button(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            visibility = reader.ReadByte();
            disable_count = reader.ReadByte();
            mode = reader.ReadByte();
            parent_id = reader.ReadInt16();
            sequence_in_order = reader.ReadBoolean();
            siblings_count = reader.ReadByte();
            is_moving = reader.ReadBoolean();
            if (is_moving)
            {
                moving_platform_id = reader.ReadInt16();
            }
            else
            {
                position = reader.ReadVec();
            }
            int event_count = reader.ReadInt16();
            for (int i = 0; i < event_count; i++)
            {
                events.Add(reader.ReadInt16());
            }
            //Console.WriteLine(visibility);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(visibility);
            writer.Write(disable_count);
            writer.Write(mode);
            writer.Write(parent_id);
            writer.Write(sequence_in_order);
            writer.Write(siblings_count);
            writer.Write(is_moving);

            if (is_moving)
            {
                writer.Write(moving_platform_id);
            }
            else
            {
                writer.Write(position);
            }

            writer.Write((short)events.Count);

            for (int i = 0; i < events.Count; i++)
            {
                writer.Write(events[i]);
            }
        }
    }
}
