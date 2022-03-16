using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
