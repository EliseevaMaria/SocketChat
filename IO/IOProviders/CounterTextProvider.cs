using System;
using System.Threading;

namespace IO.IOProviders
{
    public sealed class CounterTextProvider : ITextIOProvider
    {
        private int readingCounter = 0;
        public string Read()
        {
            if (this.readingCounter == 0)
                return "Counter_name" + this.readingCounter++;

            Thread.Sleep(3000);
            return this.readingCounter++.ToString();
        }

        public void Write(string message = null)
        {
            Console.WriteLine(message);
        }
    }
}
