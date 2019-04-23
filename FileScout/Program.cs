using System;
using System.Diagnostics;
using System.Threading;

namespace FileScout
{
    class Program
    {
        static void Main( string[] args )
        {
            Console.Title = "File Scout";

            Input readInput = new Input();
            Thread readInputThread = new Thread( new ThreadStart( readInput.StartReading ) );
            ConsoleDisplay.Display();
            readInputThread.Start();
            new WatchFileSystem();
        }
    }
}
