using System;

namespace ArdalisRating
{
    class ConsoleLogger : ILogger
    {
        public void Information(string message)
        {
            Console.WriteLine(message);
        }
    }
}
