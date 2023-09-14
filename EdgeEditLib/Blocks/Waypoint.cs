using System;

namespace EdgeEditLib.Blocks
{
    /// <summary>
    /// Waypoint for moving platforms
    /// </summary>
    public class Waypoint
    {
        public Vec position;
        public short travel_time;
        public short pause_time;

        public Waypoint()
        {

        }

        public Waypoint(Vec pos, short travelTime = 0, short pauseTime = 0)
        {
            position = pos;
            travel_time = travelTime;
            pause_time = pauseTime;
        }

        public Waypoint(int x, int y, int z, short travelTime = 0, short pauseTime = 0)
        {
            position = new Vec(x, y, z);
            travel_time = travelTime;
            pause_time = pauseTime;
        }
    }
}
