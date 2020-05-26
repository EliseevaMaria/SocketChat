using System;

namespace IO.Encoders
{
    public interface IMessageEncoder
    {
        byte[] GetByteMessage(string message);

        string GetStringMessage(byte[] buffer, int count);
    }
}
