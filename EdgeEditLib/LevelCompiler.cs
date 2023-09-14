using System;
using EdgeEditLib.Blocks;

namespace EdgeEditLib
{
    public static class LevelCompiler
    {
        /// <summary>
        /// The static geometry of the level must be compiled as a 3D model.
        /// By default its creates moving platforms as static blocks. 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="fastMode">Defines the compilation mode. If true then the geometry will be created using with moving platforms</param>
        public static Level Compile(Level level, bool fastMode = true)
        {
            if (fastMode)
            {
                for (int z = 0; z < level.Map.Height; z++)
                {
                    for (int x = 0; x < level.Map.Width; x++)
                    {
                        for (int y = 0; y < level.Map.Length; y++)
                        {
                            if (level.Map[x, y, z])
                            {
                                MovingPlatform platform = new MovingPlatform();
                                platform.full_block = true;
                                platform.waypoints.Add(new Waypoint(new Vec(x, y, z + 1)));
                                platform.loop_start_index = 0;
                                platform.auto_start = 0;
                                platform.clones = 0;
                                if (z == 0)
                                {
                                    platform.full_block = false;
                                }
                                level.moving_platforms.Add(platform);
                            }
                        }
                    }
                }
                //level.moving_platform_count = (ushort)level.moving_platforms.Count;
            }
            else
            {
                // TO-DO ESOModel generator based on TwoTribes engine
                throw new NotImplementedException("This compiler mode not implemented yet. Use fast mode");
            }
            return level;
        }
    }
}
