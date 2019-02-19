using System;

namespace WpfCaliburn.Toramp
{
    public class ParsingException : Exception
    {
        public readonly string Code;

        public ParsingException(string message) : base(message)
        {
        }

        public ParsingException(string message, string code) : base(message)
        {
            Code = code;
        }
    }
}