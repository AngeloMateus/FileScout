using System;
using System.Threading;

namespace FileScout
{
    class Program
    {
        static void Main()
        {
            Console.Title = "FileScout";

            //Reads user input on it's own thread
            Input readInput = new Input();
            Thread readInputThread = new Thread(new ThreadStart(readInput.StartReading));


            //Watches for changes in filesystem and calls Display() when changes occur.
            new WatchFileSystem().CheckFiles();

            //Handles console display, directory sorting, path shortening, etc.
            ConsoleDisplay.Display();
            readInputThread.Start();

        }
    }
}
