using System;

namespace LevelModel.DTO
{
    public class InvalidDataException : Exception
    {

        public InvalidDataException(string message) : base(message) { }

        public InvalidDataException(string message, Exception ex) : base(message, ex) { }

    }
}
