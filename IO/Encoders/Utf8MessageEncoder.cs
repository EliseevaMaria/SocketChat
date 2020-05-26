using System.Text;

namespace IO.Encoders
{
    public sealed class Utf8MessageEncoder : IMessageEncoder
    {
        public byte[] GetByteMessage(string message) => Encoding.UTF8.GetBytes(message);

        public string GetStringMessage(byte[] buffer, int count) => Encoding.UTF8.GetString(buffer, 0, count);
    }
}