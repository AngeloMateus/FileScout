using System;
using System.Threading;

namespace FileScout
{
    class Program
    {
        static void Main( string[] args )
        {
            new WatchFileSystem();
            DisplayFiles.Display();
            Console.Title = "File Scout";

            ReadInput readInput = new ReadInput();
            Thread readInputThread = new Thread( new ThreadStart( readInput.StartReading ) );
            readInputThread.Start();


        }
    }
}
