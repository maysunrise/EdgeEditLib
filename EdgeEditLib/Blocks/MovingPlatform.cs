using System;
using System.Collections.Generic;
using System.IO;

namespace EdgeEditLib.Blocks
{
    /// <summary>
    /// Moving platforms use waypoints to define their path
    /// </summary>
    public class MovingPlatform
    {
        public byte auto_start;
        // auto_start == 0: inactive
        // auto_start == 2: active (or any value that is not zero)
        public byte loop_start_index;
        public short clones; // deprecated, do not use
        public bool full_block;
        //public byte waypoints_count;

        public List<Waypoint> waypoints;

        public MovingPlatform()
        {
            waypoints = new List<Waypoint>();
        }

        public MovingPlatform(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            auto_start = reader.ReadByte();
            loop_start_index = reader.ReadByte();
            clones = reader.ReadInt16();
            full_block = reader.ReadBoolean();
            byte waypoints_count = reader.ReadByte();
            waypoints = new List<Waypoint>();
            for (int i = 0; i < waypoints_count; i++)
            {
                Waypoint waypoint = new Waypoint();

                waypoint.position = reader.ReadVec();
                waypoint.travel_time = reader.ReadInt16();
                waypoint.pause_time = reader.ReadInt16();
                waypoints.Add(waypoint);
            }
        }
        public void Write(BinaryWriter writer)
        {
            writer.Write(auto_start);
            writer.Write(loop_start_index);
            writer.Write(clones);
            writer.Write(full_block);
            writer.Write((byte)waypoints.Count);
            for (int i = 0; i < waypoints.Count; i++)
            {
                writer.Write(waypoints[i].position);
                writer.Write(waypoints[i].travel_time);
                writer.Write(waypoints[i].pause_time);
            }
        }

        // Helper methods
        public Waypoint GetWaypointAt(int index)
        {
            if (index >= 0 && index < waypoints.Count)
            {
                return waypoints[index];
            }
            return null;
        }

        public bool IsActiveOnStart()
        {
            return auto_start != 0;
        }

        public bool IsHalfBlock()
        {
            return !full_block;
        }

    }
}
