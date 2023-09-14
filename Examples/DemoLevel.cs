using System;
using EdgeEditLib;
using EdgeEditLib.Blocks;

class Program
{
	static void Main(string[] args)
	{
		// CHANGE IT TO YOUR GAME PATH
		GenerateDemoLevel("C:\\Program Files (x86)\\Steam\\steamapps\\common\\EDGE");
	}

	// This method generates a playable demo level for Edge from the code
	// Use this as a starting point to learn library
	public static void GenerateDemoLevel(string gamePath)
	{
		LevelParser.SetGamePath(gamePath);

		// Creating level with size 16x16x4
		Level level = LevelParser.Create(16, 16, 4, "demo level");

		// Init level manipulator, contains a set of methods to make editing easier
		// by default this has flag dontUseStatic = true
		// because level compilation is not currently supported
		LevelManipulate manipulate = new LevelManipulate(level, true);

		// Specifies some initial data such as theme (light or dark), music and time thresholds
		manipulate.SetHeaderData(0, 3, 20, 30, 40, 50, 60);

		// Sets the player spawn and finish at specified position
		manipulate.SetSpawnPoint(8, 8, 6);
		manipulate.SetFinishPoint(14, 2, 1);

		// Creates some simple level geometry, SetCuboid() allows you to create blocks at specifed area
		manipulate.SetCuboid(0, 0, 0, 15, 15, 0, true);
		manipulate.SetCuboid(12, 4, 0, 6, 6, 0, false);
		manipulate.SetBlockAt(6, 0, 1, true);
		manipulate.SetBlockAt(6, 1, 2, true);
		for (int i = 0; i < 4; i++)
		{
			manipulate.SetBlockAt(2, 12, i, true);
		}

		// Falling platforms
		manipulate.CreateFallingPlatform(8, 6, 1, 10);
		manipulate.CreateFallingPlatform(8, 5, 1, 10);
		manipulate.CreateFallingPlatform(8, 4, 1, 10);

		// Prisms
		manipulate.CreatePrism(5, 2, 1);
		manipulate.CreatePrism(5, 3, 1);
		manipulate.CreatePrism(5, 4, 1);

		// Bumper
		manipulate.CreateBumper(10, 1, 1,
			new Bumper.BumperSide(10, 10),
			new Bumper.BumperSide(10, 10),
			new Bumper.BumperSide(10, 10),
			new Bumper.BumperSide(10, 10),
			true
			);

		// Resizers (shrink/grow)
		manipulate.CreateResizer(4, 12, 1, true);
		manipulate.CreateResizer(4, 13, 1, false);

		// Two camera triggers, located on a stair made of blocks
		manipulate.CreateCameraTrigger(6, 1, 2, 1, 1, -1, false, 0, 30, false, 100, false);
		manipulate.CreateCameraTrigger(6, 1, 1, 1, 1, -1, true, 0, 30);

		// Two moving platforms, waypoints uses as path along which they move
		Waypoint[] waypoints = new Waypoint[4];
		waypoints[0] = new Waypoint(0, 0, 2, 25, 5);
		waypoints[1] = new Waypoint(4, 0, 2, 25, 5);
		waypoints[2] = new Waypoint(4, 4, 2, 25, 5);
		waypoints[3] = new Waypoint(0, 4, 2, 25, 5);
		MovingPlatform platform = manipulate.CreateMovingPlatform(waypoints, true, 1, false);

		Waypoint[] waypoints2 = new Waypoint[4];
		waypoints2[0] = new Waypoint(0, 2, 3, 5, 5);
		waypoints2[1] = new Waypoint(0, 4, 3, 5, 10);
		waypoints2[2] = new Waypoint(0, 3, 3, 5, 5);
		waypoints2[3] = new Waypoint(0, 2, 4, 5, 5);
		MovingPlatform platform2 = manipulate.CreateMovingPlatform(waypoints2, true, 1, false);

		// Buttons always use block events(one or more) according to this:
		// [Button] -> [BlockEvent] -> [Block]
		// Event block can control moving platforms, bumpers, buttons and achievements
		BlockEvent testEvent = manipulate.CreateEvent(0, manipulate.GetBlockId(platform), 0);
		manipulate.CreateButton(1, 1, 1, 1, 0, 0, testEvent, -1);

		BlockEvent testEvent2 = manipulate.CreateEvent(0, manipulate.GetBlockId(platform2), 0);
		manipulate.CreateButton(1, 3, 1, 1, 0, 0, testEvent2, -1);

		// Other cube is a hologram or dark cube.
		// Key events used as if the player movements were recorded and replayed (Is key down/up, time, direction)
		OtherCube.KeyEvent[] keyEvents = new OtherCube.KeyEvent[5];

		keyEvents[0] = new OtherCube.KeyEvent(100, 0, 0);
		keyEvents[1] = new OtherCube.KeyEvent(120, 0, 1);
		keyEvents[2] = new OtherCube.KeyEvent(160, 2, 0);
		keyEvents[3] = new OtherCube.KeyEvent(190, 2, 1);
		keyEvents[4] = new OtherCube.KeyEvent(8000, 0, 1);
		manipulate.CreateOtherCube(new Vec(4, 8, 2), new Vec(3, 2, 1), -2, keyEvents, 8, 8, 0);

		// Saves the level and adds it to the list of standard levels in the game menu (be sure to specify the path to the main game folder)
		LevelParser.SaveAndExport(level, "demolevel", LevelType.Standard);

		Console.WriteLine($"Level saved as {level.header.title}");
		Console.ReadKey();

		// Some notes
		// Z for some blocks is not constant. Z = 2 for one block and Z = 2 for another may differ by one unit
		// Finish point will not render correctly in the game if you spawn this before creating the geometry. Not critical
		// Each "create" method return block instance
	}
}