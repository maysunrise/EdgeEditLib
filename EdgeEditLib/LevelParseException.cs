using System;

namespace EdgeEditLib
{
    public class LevelParseException : Exception
    {
        public LevelParseException()
        {
        }

        public LevelParseException(string message)
            : base(message)
        {
        }

        public LevelParseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
