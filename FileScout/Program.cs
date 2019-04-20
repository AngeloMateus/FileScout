using System;
using System.Threading;

namespace FileScout
{
    class Program
    {
        static void Main( string[] args )
        {
            new WatchFileSystem();
            ConsoleDisplay.Display();
            Console.Title = "File Scout";

            Input readInput = new Input();
            Thread readInputThread = new Thread( new ThreadStart( readInput.StartReading ) );
            readInputThread.Start();
        }
    }
}
