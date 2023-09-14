
namespace EdgeEditLib
{
    /// <summary>
    /// Only header of the level (Title, id, etc)
    /// </summary>
    public struct LevelHeader
    {
        public int level_id;

        public int title_length;

        public string title;

        public int[] time_thresholds; // size 5

        public short prism_count;

        public string GetTitle()
        {
            return title;
        }

        public int PrismCount()
        {
            return prism_count;
        }

        public override string ToString()
        {
            string s = "";
            s += "Id: " + level_id + "\n";
            s += "Title Length: " + title_length + "\n";
            s += "Title: " + title + "\n";
            s += "S Plus Time: " + time_thresholds[0] + "\n";
            s += "S Time: " + time_thresholds[1] + "\n";
            s += "A Time: " + time_thresholds[2] + "\n";
            s += "B Time: " + time_thresholds[3] + "\n";
            s += "C Time: " + time_thresholds[4] + "\n";
            s += "Prisms: " + prism_count + "\n";
            return s;
        }
    }
}
