using EdgeEditLib.Blocks;
using System;
using System.Collections.Generic;

namespace EdgeEditLib
{
    // Original .h file by Will Kirkby  https://github.com/Mygod/EDGE/blob/master/EdgeTool/Core/Level/EdgeBin.h
    // Archived .h file https://web.archive.org/web/20230911145950/https://raw.githubusercontent.com/Mygod/EDGE/master/EdgeTool/Core/Level/EdgeBin.h

    // P.S. thanks to Will Kirkby for this header and Mygod for saving this header because original website removed and not archived

    /// <summary>
    /// Entire level structure. Original struct written by Will Kirkby
    /// </summary>
    public class Level
    {
        public LevelHeader header;
        public sbyte size_z;
        public ushort size_x;
        public ushort size_y;
        public ushort unknown_short_1;  // size_x + size_y
        public ushort unknown_short_2;  // size_x + size_y + (2 * size_z)
        public ushort unknown_short_3;  // (unknown_short_1 + 9) / 10
        public ushort unknown_short_4;  // (unknown_short_2 + 9) / 10
        public byte unknown_byte_1;    // 10
        public ushort unknown_short_5;  // size_y - 1
        public ushort unknown_short_6;  // 0
        public byte[] legacy_minimap;   // length = ((unknown_short_3 * unknown_short_4) + 7) / 8
                                        //public byte[] collision_map;    // length = size_z * (((size_x * size_y) + 7) / 8)
        public Map3D Map;

        public Vec spawn_point;
        public short zoom;
        // if zoom < 0
        public ushort value;
        public bool value_is_angle;    // value is field of view if false
                                       // endif
        public Vec exit_point;

        //public ushort moving_platform_count;
        public List<MovingPlatform> moving_platforms = new List<MovingPlatform>();

        //public ushort bumper_count;
        public List<Bumper> bumpers = new List<Bumper>();

        //public ushort falling_platform_count;
        public List<FallingPlatform> falling_platforms = new List<FallingPlatform>();

        //public ushort checkpoint_count;
        public List<Checkpoint> checkpoints = new List<Checkpoint>();

        //public ushort camera_trigger_count;
        public List<CameraTrigger> camera_triggers = new List<CameraTrigger>();

        // Must be equal to count in header
        //public ushort prism_count;
        public List<Prism> prisms = new List<Prism>();

        [Obsolete]
        public ushort fans_count; // deprecated, presumably would have been followed by fan *fans;

        //public ushort block_event_count;
        public List<BlockEvent> block_events = new List<BlockEvent>();

        //public ushort button_count;
        public List<Button> buttons = new List<Button>();

        //public ushort othercube_count;
        public List<OtherCube> other_cubes = new List<OtherCube>();

        //public ushort resizer_count;
        public List<Resizer> resizers = new List<Resizer>();

        [Obsolete]
        public ushort mini_blocks_count; // deprecated, presumably would have been followed by mini_block *mini_blocks;

        public byte theme;
        public byte music_j2me;
        public byte music;

        public bool IsModifed;

        public bool HasBeenModifed()
        {
            return IsModifed;
        }
    }
}
