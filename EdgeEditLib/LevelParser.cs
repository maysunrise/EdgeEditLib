using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using EdgeEditLib.Blocks;

namespace EdgeEditLib
{
    /// <summary>
    /// Edge level parser. Use this to read or write .bin files
    /// </summary>
    public static class LevelParser
    {
        public static string GamePath = "";

        private const string Ext = ".bin";

        public static void SetGamePath(string gamePath)
        {
            GamePath = gamePath;
        }

        /// <summary>
        /// Returns level data by name of the file. Contains all level data
        /// </summary>
        public static Level LoadByName(string levelFileName)
        {
            if (GamePath.Length == 0)
            {
                throw new LevelParseException("To load by name, specify path to the game folder");
            }
            return LoadFromFile(Path.Combine(GamePath, "levels", levelFileName + Ext));
        }

        /// <summary>
        /// Returns level data from the file. Contains all level data
        /// </summary>
        public static Level LoadFromFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }
            Level level = new Level();

            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII, false))
                {
                    // Level header
                    level.header = ParseHeader(reader);

                    // Level Height
                    level.size_z = reader.ReadSByte();
                    // Level Width
                    level.size_x = reader.ReadUInt16();
                    // Level Depth
                    level.size_y = reader.ReadUInt16();

                    // Unknown values
                    level.unknown_short_1 = reader.ReadUInt16();
                    level.unknown_short_2 = reader.ReadUInt16();
                    level.unknown_short_3 = reader.ReadUInt16();
                    level.unknown_short_4 = reader.ReadUInt16();
                    level.unknown_byte_1 = reader.ReadByte();
                    level.unknown_short_5 = reader.ReadUInt16();
                    level.unknown_short_6 = reader.ReadUInt16();

                    // Legacy minimap
                    // deprecated, this was probably used in j2me version of the game?
                    int legacyMinimapLength = ((level.unknown_short_3 * level.unknown_short_4) + 7) / 8;
                    int b1 = reader.ReadByte();
                    int b2 = reader.ReadByte();
                    if ((int)(b1 + b2) == 414)
                    {
                        level.IsModifed = true;
                    }
                    stream.Skip(legacyMinimapLength - 2);
                    // Static geometry like walls and floor
                    level.Map = new Map3D(reader, level.size_x, level.size_y, level.size_z);
                    // Player spawn point
                    level.spawn_point = reader.ReadVec();

                    // Camera zoom at start
                    level.zoom = reader.ReadInt16();

                    // Finish point
                    level.exit_point = reader.ReadVec();

                    // Moving platforms
                    ushort moving_platform_count = reader.ReadUInt16();
                    level.moving_platforms = new List<MovingPlatform>();

                    for (int i = 0; i < moving_platform_count; i++)
                    {
                        MovingPlatform movingPlatform = new MovingPlatform(reader);
                        level.moving_platforms.Add(movingPlatform);
                    }

                    // Bumpers
                    ushort bumper_count = reader.ReadUInt16();
                    level.bumpers = new List<Bumper>();
                    for (int i = 0; i < bumper_count; i++)
                    {
                        Bumper bumper = new Bumper(reader);
                        level.bumpers.Add(bumper);
                    }

                    // Falling platforms
                    ushort falling_platform_count = reader.ReadUInt16();
                    level.falling_platforms = new List<FallingPlatform>();
                    for (int i = 0; i < falling_platform_count; i++)
                    {
                        FallingPlatform fallingPlatform = new FallingPlatform(reader);
                        level.falling_platforms.Add(fallingPlatform);
                    }

                    // Checkpoint triggers
                    ushort checkpoint_count = reader.ReadUInt16();
                    level.checkpoints = new List<Checkpoint>();
                    for (int i = 0; i < checkpoint_count; i++)
                    {
                        Checkpoint checkpoint = new Checkpoint(reader);
                        level.checkpoints.Add(checkpoint);
                    }

                    // Camera triggers
                    ushort camera_trigger_count = reader.ReadUInt16();
                    level.camera_triggers = new List<CameraTrigger>();
                    for (int i = 0; i < camera_trigger_count; i++)
                    {
                        CameraTrigger cameraTrigger = new CameraTrigger(reader);
                        level.camera_triggers.Add(cameraTrigger);
                    }

                    // Prisms
                    ushort prism_count = reader.ReadUInt16();
                    if (prism_count != level.header.prism_count)
                    {
                        //throw new LevelParseException("level.prism_count and level.header.prism_count not equal");
                    }
                    level.prisms = new List<Prism>();
                    for (int i = 0; i < prism_count; i++)
                    {
                        Prism prism = new Prism(reader);
                        level.prisms.Add(prism);
                    }

                    // Fans, deprecated
                    level.fans_count = reader.ReadUInt16();

                    // Block events
                    ushort block_event_count = reader.ReadUInt16();
                    level.block_events = new List<BlockEvent>();
                    for (int i = 0; i < block_event_count; i++)
                    {
                        BlockEvent blockEvent = new BlockEvent(reader);
                        level.block_events.Add(blockEvent);
                    }

                    // Buttons
                    ushort button_count = reader.ReadUInt16();
                    level.buttons = new List<Button>();
                    for (int i = 0; i < button_count; i++)
                    {
                        Button button = new Button(reader);
                        level.buttons.Add(button);
                    }

                    // Other cubes (Dark Cube and hologram for example)
                    ushort othercube_count = reader.ReadUInt16();
                    level.other_cubes = new List<OtherCube>();
                    for (int i = 0; i < othercube_count; i++)
                    {
                        OtherCube otherCube = new OtherCube(reader);
                        level.other_cubes.Add(otherCube);
                    }

                    // Resizers
                    ushort resizer_count = reader.ReadUInt16();
                    level.resizers = new List<Resizer>();
                    for (int i = 0; i < resizer_count; i++)
                    {
                        Resizer resizer = new Resizer(reader);
                        level.resizers.Add(resizer);
                    }

                    // Mini blocks, deprecated
                    level.mini_blocks_count = reader.ReadUInt16();

                    // Color theme for the level
                    level.theme = reader.ReadByte();

                    // Music Id in j2me version
                    level.music_j2me = reader.ReadByte();

                    // Music Id in modern version
                    level.music = reader.ReadByte();
                }
            }

            return level;
        }

        /// <summary>
        /// Writes the level file to game folder and adds it to menu (Experimental function)
        /// </summary>
        public static void SaveAndExport(Level level, string levelFileName, LevelType levelType = LevelType.Standard)
        {
            if (GamePath.Length == 0)
            {
                throw new LevelParseException("To export specify path to the game folder");
            }
            SaveToFile(level, Path.Combine(GamePath, "levels", levelFileName + Ext));
            XmlHelper.AddLevelToList(levelFileName, levelType);
        }

        /// <summary>
        /// Writes the level to a file
        /// </summary>
        public static void SaveToFile(Level level, string fileName)
        {
            if (level == null)
            {
                throw new LevelParseException("You cannot save an null level");
            }
            level.IsModifed = true;

            level.unknown_short_1 = (ushort)(level.size_x + level.size_y);
            level.unknown_short_2 = (ushort)(level.size_x + level.size_y + (2 * level.size_z));
            level.unknown_short_3 = (ushort)((level.unknown_short_1 + 9) / 10);
            level.unknown_short_4 = (ushort)((level.unknown_short_2 + 9) / 10);
            level.unknown_short_5 = (ushort)(level.size_y - 1);
            level.unknown_short_6 = 0;
            level.unknown_byte_1 = 10; // its always 10, idk why

            using (Stream stream = File.Open(fileName, FileMode.OpenOrCreate))
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII, false))
                {
                    WriteHeader(writer, level.header);

                    // Level Height
                    writer.Write(level.size_z);
                    // Level Width
                    writer.Write(level.size_x);
                    // Level Depth
                    writer.Write(level.size_y);

                    writer.Write(level.unknown_short_1);
                    writer.Write(level.unknown_short_2);
                    writer.Write(level.unknown_short_3);
                    writer.Write(level.unknown_short_4);
                    writer.Write(level.unknown_byte_1);
                    writer.Write(level.unknown_short_5);
                    writer.Write(level.unknown_short_6);

                    // Dummy bytes for legacy minimap (this should help to run the level correctly in j2me, not tested)
                    byte[] lgm = new byte[((level.unknown_short_3 * level.unknown_short_4) + 7) / 8];
                    // I reserved this two bytes to detect custom levels
                    lgm[0] = 252;
                    lgm[1] = 162;
                    writer.Write(lgm);

                    // Static geometry
                    level.Map.Write(writer);

                    // Player spawn point
                    writer.Write(level.spawn_point);

                    // Camera zoom at start
                    writer.Write(level.zoom);

                    // Finish point
                    writer.Write(level.exit_point);

                    // Moving platforms
                    writer.Write((ushort)level.moving_platforms.Count);
                    for (int i = 0; i < level.moving_platforms.Count; i++)
                    {
                        level.moving_platforms[i].Write(writer);
                    }

                    // Bumpers
                    writer.Write((ushort)level.bumpers.Count);
                    for (int i = 0; i < level.bumpers.Count; i++)
                    {
                        level.bumpers[i].Write(writer);
                    }

                    // Falling platforms
                    writer.Write((ushort)level.falling_platforms.Count);
                    for (int i = 0; i < level.falling_platforms.Count; i++)
                    {
                        level.falling_platforms[i].Write(writer);
                    }

                    // Checkpoint trigger
                    writer.Write((ushort)level.checkpoints.Count);
                    for (int i = 0; i < level.checkpoints.Count; i++)
                    {
                        level.checkpoints[i].Write(writer);
                    }

                    // Camera trigger
                    writer.Write((ushort)level.camera_triggers.Count);
                    for (int i = 0; i < level.camera_triggers.Count; i++)
                    {
                        level.camera_triggers[i].Write(writer);
                    }

                    // Prisms
                    writer.Write((ushort)level.prisms.Count);
                    for (int i = 0; i < level.prisms.Count; i++)
                    {
                        level.prisms[i].Write(writer);
                    }

                    // Fans
                    level.fans_count = 0;
                    writer.Write(level.fans_count);

                    // Block events
                    writer.Write((ushort)level.block_events.Count);
                    for (int i = 0; i < level.block_events.Count; i++)
                    {
                        level.block_events[i].Write(writer);
                    }

                    // Buttons
                    writer.Write((ushort)level.buttons.Count);
                    for (int i = 0; i < level.buttons.Count; i++)
                    {
                        level.buttons[i].Write(writer);
                    }

                    // Other cubes
                    writer.Write((ushort)level.other_cubes.Count);
                    for (int i = 0; i < level.other_cubes.Count; i++)
                    {
                        level.other_cubes[i].Write(writer);
                    }

                    // Resizers
                    writer.Write((ushort)level.resizers.Count);
                    for (int i = 0; i < level.resizers.Count; i++)
                    {
                        level.resizers[i].Write(writer);
                    }

                    // Mini blocks
                    level.mini_blocks_count = 0;
                    writer.Write(level.mini_blocks_count);

                    // Color theme
                    writer.Write(level.theme);

                    // Music Id in j2me
                    writer.Write(level.music_j2me);

                    // Music Id in modern version
                    writer.Write(level.music);
                }
            }
        }

        /// <summary>
        /// Creates a new empty level with a given size and returns instance for modification
        /// </summary>
        public static Level Create(int width, int length, int height, string title, int level_id = 300, int[] time_thresholds = null)
        {
            Level level = new Level();

            level.header.title = title;
            level.header.title_length = title.Length;
            level.header.prism_count = 0;
            level.header.level_id = level_id;

            level.IsModifed = true;

            if (time_thresholds != null)
            {
                level.header.time_thresholds = time_thresholds;
            }
            else
            {
                level.header.time_thresholds = new int[5];
                level.header.time_thresholds[0] = 10;
                level.header.time_thresholds[1] = 15;
                level.header.time_thresholds[2] = 20;
                level.header.time_thresholds[3] = 25;
                level.header.time_thresholds[4] = 30;
            }
            level.size_z = (sbyte)height;
            level.size_x = (ushort)width;
            level.size_y = (ushort)length;

            level.Map = new Map3D(width, length, height);

            return level;
        }

        /// <summary>
        /// Returns level header from the file. Useful for level selection
        /// </summary>
        public static LevelHeader ParseHeader(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new LevelParseException("Level not exists");
            }

            LevelHeader header = new LevelHeader();
            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII, false))
                {
                    header = ParseHeader(reader);
                }
            }
            return header;
        }

        /// <summary>
        /// Returns level header from BinaryReader. Useful for level selection
        /// </summary>
        public static LevelHeader ParseHeader(BinaryReader reader)
        {
            LevelHeader header = new LevelHeader();
            header.time_thresholds = new int[5];

            header.level_id = reader.ReadInt32(); // Level unique id
            header.title_length = reader.ReadInt32(); // Length of title

            char[] titleChars = reader.ReadChars(header.title_length);
            header.title = new string(titleChars); // Level title

            header.time_thresholds[0] = reader.ReadUInt16(); // S Plus Time
            header.time_thresholds[1] = reader.ReadUInt16(); // S Time
            header.time_thresholds[2] = reader.ReadUInt16(); // A Time
            header.time_thresholds[3] = reader.ReadUInt16(); // B Time
            header.time_thresholds[4] = reader.ReadUInt16(); // C Time

            header.prism_count = reader.ReadInt16(); // Prism count

            return header;
        }

        private static void WriteHeader(BinaryWriter writer, LevelHeader header)
        {
            writer.Write(header.level_id);
            writer.Write(header.title_length);
            writer.Write(ASCIIEncoding.ASCII.GetBytes(header.title));

            for (int i = 0; i < header.time_thresholds.Length; i++)
            {
                writer.Write((ushort)header.time_thresholds[i]);
            }

            writer.Write(header.prism_count);
        }
    }
}
