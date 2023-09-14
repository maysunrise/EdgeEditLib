using System;
using System.IO;

namespace EdgeEditLib
{
    public static class ParseHelper
    {

        // Skipping bytes
        public static void Skip(this Stream stream, int bytes)
        {
            stream.Seek(bytes, SeekOrigin.Current);
        }

        public static Vec ReadVec(this BinaryReader reader)
        {
            return new Vec(
                reader.ReadInt16(),
                reader.ReadInt16(),
                reader.ReadInt16()
                );
        }

        public static void Write(this BinaryWriter writer, Vec vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
        }
    }
}
