using System;

namespace IO.IOProviders
{
    public sealed class ConsoleIOProvider : ITextIOProvider
    {
        public string Read() => Console.ReadLine();

        public void Write(string message) => Console.WriteLine(message);
    }
}