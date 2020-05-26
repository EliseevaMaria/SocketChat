using System;

namespace IO.IOProviders
{
    public interface ITextIOProvider
    {
        string Read();

        void Write(string message = null);
    }
}
