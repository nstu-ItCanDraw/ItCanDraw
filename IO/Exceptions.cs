using System;
using System.Runtime.Serialization;

namespace IO
{
    [Serializable]
    public class BadFileExtensionError : Exception
    {
        public BadFileExtensionError(string message) : base(message)
        { }

        protected BadFileExtensionError(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        { }
    }

    [Serializable]
    public class BadFileError : Exception
    {
        public BadFileError(string message) : base(message)
        { }

        protected BadFileError(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        { }
    }
}
