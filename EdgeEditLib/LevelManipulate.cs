using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeEditLib.Blocks;

namespace EdgeEditLib
{
    /// <summary>
    /// A set of methods for level editing
    /// </summary>
    public class LevelManipulate
    {
        public Level Level;

        public bool DontUseStatic;

        public LevelManipulate() { }

        /// <summary>
        /// Prepares the level for data editing. Set dontUseStatic=true to disable static geometry (Performance may be bad on large levels)
        /// </summary>
        public LevelManipulate(Level level, bool dontUseStatic)
        {
            SetLevel(level, dontUseStatic);
        }

        /// <summary>
        /// Prepares the level for data editing. Set dontUseStatic=true to disable static geometry (Performance may be bad on large levels)
        /// </summary>
        public void SetLevel(Level level, bool dontUseStatic)
        {
            DontUseStatic = dontUseStatic;
            Level = level;
        }

        // Static geometry edit

        /// <summary>
        /// Places one static block or air at specified position
        /// </summary>
        public void SetBlockAt(int x, int y, int z, bool solid)
        {
            // Uses moving platform as static block, does not require compiling but slower
            if (DontUseStatic)
            {
                // If exists then remove
                if (Level.Map[x, y, z])
                {
                    for (int i = 0; i < Level.moving_platforms.Count; i++)
                    {
                        if (Level.moving_platforms[i].waypoints[0].position.EqualsXYZ(x, y, z + 1))
                        {
                            Level.moving_platforms.RemoveAt(i);
                            break;
                        }
                    }
                }
                if (solid)
                {
                    MovingPlatform platform = new MovingPlatform();
                    platform.waypoints.Add(new Waypoint(x, y, z + 1)); // WHY +1???
                    platform.loop_start_index = 0;
                    platform.auto_start = 0;
                    platform.clones = 0;
                    if (z != 0)
                    {
                        platform.full_block = true;
                    }
                    Level.moving_platforms.Add(platform);
                }
                //Level.moving_platform_count = (ushort)Level.moving_platforms.Count;
            }
            Level.Map[x, y, z] = solid;
        }

        /// <summary>
        /// Creates a cuboid between two positions
        /// </summary>
        public void SetCuboid(int x1, int y1, int z1, int x2, int y2, int z2, bool solid)
        {
            int minX = Math.Min(x1, x2);
            int minY = Math.Min(y1, y2);
            int minZ = Math.Min(z1, z2);

            int maxX = Math.Max(x1, x2);
            int maxY = Math.Max(y1, y2);
            int maxZ = Math.Max(z1, z2);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        SetBlockAt(x, y, z, solid);
                    }
                }
            }
        }

        /// <summary>
        /// Sets player spawn
        /// </summary>
        public void SetSpawnPoint(int x, int y, int z)
        {
            Level.spawn_point = new Vec(x, y, z);
        }

        /// <summary>
        /// The point that marks the end of the level. Z axis cannot be lower than 1
        /// </summary>
        public void SetFinishPoint(int x, int y, int z)
        {
            // For some reason, setting Z to 0 causes game crash
            if (z <= 0)
            {
                Level.exit_point = new Vec(x, y, 1);
                return;
            }
            Level.exit_point = new Vec(x, y, z);

            if (DontUseStatic)
            {
                ClearFinishArea(x, y, z);
            }
        }

        // Moving platform edit
        public MovingPlatform CreateMovingPlatform(Waypoint[] waypoints, bool fullBlock, int startWaypointIndex = 0, bool autoStart = true)
        {
            MovingPlatform platform = new MovingPlatform();
            platform.clones = 0;

            platform.waypoints.AddRange(waypoints);

            platform.loop_start_index = (byte)startWaypointIndex;
            platform.auto_start = (byte)(autoStart ? 2 : 0);
            platform.full_block = fullBlock;
            Level.moving_platforms.Add(platform);

            return platform;
        }

        public void RemoveMovingPlatform(short index)
        {
            Level.moving_platforms.RemoveAt(index);
        }

        // Falling platform edit
        public short CreateFallingPlatform(int x, int y, int z, short floatTime = 20)
        {
            FallingPlatform platform = new FallingPlatform();
            platform.position = new Vec(x, y, z);
            platform.float_time = floatTime;
            Level.falling_platforms.Add(platform);

            return (short)Level.falling_platforms.Count;
        }

        public void RemoveFallingPlatform(short index)
        {
            Level.falling_platforms.RemoveAt(index);
        }

        // Bumper edit
        public short CreateBumper(int x, int y, int z,
            Bumper.BumperSide north,
            Bumper.BumperSide east,
            Bumper.BumperSide south,
            Bumper.BumperSide west,
            bool autoStart = true)
        {
            Bumper bumper = new Bumper();
            bumper.position = new Vec(x, y, z);
            bumper.enabled = autoStart;
            bumper.north = north;
            bumper.east = east;
            bumper.south = south;
            bumper.west = west;
            Level.bumpers.Add(bumper);

            return (short)Level.bumpers.Count;
        }

        public void RemoveBumper(short index)
        {
            Level.bumpers.RemoveAt(index);
        }

        // Prism edit
        public short CreatePrism(int x, int y, int z)
        {
            Prism prism = new Prism();
            prism.position = new Vec(x, y, z);
            Level.prisms.Add(prism);

            Level.header.prism_count = (short)Level.prisms.Count;

            return (short)Level.prisms.Count;
        }

        public void RemovePrism(short index)
        {
            Level.prisms.RemoveAt(index);
            Level.header.prism_count = (short)Level.prisms.Count;
        }

        // Checkpoint edit
        public short CreateCheckpoint(int x, int y, int z, int sizeX, int sizeY, int respawn_height = 10)
        {
            Checkpoint checkpoint = new Checkpoint();
            checkpoint.position = new Vec(x, y, z);
            checkpoint.radius_x = (byte)sizeX;
            checkpoint.radius_y = (byte)sizeY;
            checkpoint.respawn_z = (short)respawn_height;
            Level.checkpoints.Add(checkpoint);

            return (short)Level.checkpoints.Count;
        }

        public void RemoveCheckpoint(short index)
        {
            Level.checkpoints.RemoveAt(index);
        }

        // Resizer edit
        public short CreateResizer(int x, int y, int z, bool shrink, bool visible = true)
        {
            Resizer resizer = new Resizer();
            resizer.position = new Vec(x, y, z);
            resizer.visible = visible;
            resizer.direction = (byte)(shrink ? 0 : 1);
            Level.resizers.Add(resizer);

            return (short)Level.resizers.Count;
        }

        public void RemoveResizer(short index)
        {
            Level.resizers.RemoveAt(index);
        }

        // Button edit
        // For complex behavior, initialize the button manually
        public Button CreateButton(Button button)
        {
            Level.buttons.Add(button);

            return button;
        }

        public Button CreateButton(int x, int y, int z,
            int visibilityMode = 1,
            int disableCount = 0,
            int mode = 0, BlockEvent blockEvent = null, int moving_platform_id = -1)
        {
            BlockEvent[] events = new BlockEvent[1];
            events[0] = blockEvent;
            return CreateButton(x, y, z, visibilityMode, disableCount, mode, events, moving_platform_id);
        }

        // Simple button, no parent/sequences feature
        public Button CreateButton(int x, int y, int z,
        int visibilityMode = 1,
        int disableCount = 0,
        int mode = 0, BlockEvent[] events = null, int moving_platform_id = -1
            )
        {
            Button button = new Button();
            button.visibility = (byte)visibilityMode;
            button.disable_count = (byte)disableCount;
            button.mode = (byte)mode;

            // very important set to -1,
            // by default all values ​​are 0 in buttons
            // but this will lead to the fact that
            // all buttons connect to first button
            button.parent_id = -1;

            if (moving_platform_id != -1)
            {
                button.is_moving = true;
                button.moving_platform_id = (short)moving_platform_id;
            }
            else
            {
                button.position = new Vec(x, y, z);
            }

            foreach (BlockEvent ev in events)
            {
                short eventId = GetBlockId(ev);
                button.events.Add(eventId);
            }

            Level.buttons.Add(button);

            return button;
        }

        public void RemoveButton(short index)
        {
            Level.buttons.RemoveAt(index);
        }

        // Camera trigger edit
        public short CreateCameraTrigger(int x, int y, int z, int sizeX, int sizeY,
            int zoom = 0,
            bool resetCamera = false,
            int startDelay = 0,
            int duration = 1,
            bool singleUse = false,
            int fov = 60,
            bool zoomMode = false)
        {
            CameraTrigger trigger = new CameraTrigger();
            trigger.position = new Vec(x, y, z);
            trigger.radius_x = (byte)sizeX;
            trigger.radius_y = (byte)sizeY;
            trigger.zoom = (short)zoom;

            if (zoom == -1)
            {
                trigger.reset = resetCamera;
                trigger.start_delay = (short)startDelay;
                trigger.duration = (short)duration;
                trigger.value = (short)fov;
                trigger.single_use = singleUse;
                trigger.value_is_angle = !zoomMode;
            }

            Level.camera_triggers.Add(trigger);

            return (short)Level.camera_triggers.Count;
        }

        public void RemoveCameraTrigger(short index)
        {
            Level.camera_triggers.RemoveAt(index);
        }

        // Block events
        public BlockEvent CreateEvent(byte type, short block_id, short payload)
        {
            BlockEvent blockEvent = new BlockEvent();
            blockEvent.type = type;
            blockEvent.block_id = block_id;
            blockEvent.payload = payload;
            Level.block_events.Add(blockEvent);

            return blockEvent;
        }

        public void RemoveEvent(short index)
        {
            Level.block_events.RemoveAt(index);
        }
        // Other cube
        public OtherCube CreateOtherCube(
            Vec cubePosition,
            Vec triggerPosition,
            short movingBlockSync,
            OtherCube.KeyEvent[] keyEvents,
            byte radiusX = 0,
            byte radiusY = 0,
            short darkCubeMovingBlockSync = 0
            )
        {
            OtherCube otherCube = new OtherCube();
            otherCube.position_cube = cubePosition;
            otherCube.position_trigger = triggerPosition;
            otherCube.moving_block_sync = movingBlockSync;
            otherCube.key_events.AddRange(keyEvents);
            if (movingBlockSync == -2)
            {
                otherCube.darkcube_radius_x = radiusX;
                otherCube.darkcube_radius_y = radiusY;
                otherCube.darkcube_moving_block_sync = darkCubeMovingBlockSync;
            }

            Level.other_cubes.Add(otherCube);

            return otherCube;
        }

        public void RemoveOtherCube(short index)
        {
            Level.other_cubes.RemoveAt(index);
        }


        public void SetHeaderData(byte theme, byte music, int SPlusTime, int STime, int ATime, int BTime, int CTime, byte musicId_j2me = 0)
        {
            Level.theme = theme;
            Level.music = music;
            Level.header.time_thresholds[0] = SPlusTime;
            Level.header.time_thresholds[1] = STime;
            Level.header.time_thresholds[2] = ATime;
            Level.header.time_thresholds[3] = BTime;
            Level.header.time_thresholds[4] = CTime;
        }

        // Get Id by instance
        // i hate repeating code, but I'm too lazy to do it differently
        public short GetBlockId(MovingPlatform block)
        {
            return (short)Level.moving_platforms.IndexOf(block);
        }
        public short GetBlockId(FallingPlatform block)
        {
            return (short)Level.falling_platforms.IndexOf(block);
        }
        public short GetBlockId(Bumper block)
        {
            return (short)Level.bumpers.IndexOf(block);
        }
        public short GetBlockId(Prism block)
        {
            return (short)Level.prisms.IndexOf(block);
        }
        public short GetBlockId(Button block)
        {
            return (short)Level.buttons.IndexOf(block);
        }
        public short GetBlockId(Resizer block)
        {
            return (short)Level.resizers.IndexOf(block);
        }
        public short GetBlockId(Checkpoint block)
        {
            return (short)Level.checkpoints.IndexOf(block);
        }
        public short GetBlockId(BlockEvent block)
        {
            return (short)Level.block_events.IndexOf(block);
        }
        public short GetBlockId(OtherCube block)
        {
            return (short)Level.other_cubes.IndexOf(block);
        }


        // Clears the finish area of ​​blocks to avoid rendering issues in not static mode
        private void ClearFinishArea(int x, int y, int z)
        {
            for (int x1 = -1; x1 <= 1; x1++)
            {
                for (int y1 = -1; y1 <= 1; y1++)
                {
                    for (int i = 0; i < Level.moving_platforms.Count; i++)
                    {
                        if (Level.moving_platforms[i].waypoints[0].position.EqualsXYZ(x + x1, y + y1, z))
                        {
                            Level.moving_platforms.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }
    }
}
