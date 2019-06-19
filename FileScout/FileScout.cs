using System;
using System.IO;
using System.Threading;

namespace FileScout
{
    class FileScout
    {
        [STAThread]
        static void Main( string[] args )
        {
            Console.Title = "FileScout";



            //Reads user input on it's own thread
            Input readInput = new Input();
            Thread readInputThread = new Thread( new ThreadStart( readInput.StartReading ) );
            readInputThread.SetApartmentState( ApartmentState.STA );

            State.currentPath = Environment.GetFolderPath( Environment.SpecialFolder.UserProfile );

            //Watches for changes in filesystem and calls Display() when changes occur.
            new WatchFileSystem().CheckFiles();

            //Handles console display, directory sorting, path shortening, etc.
            ConsoleDisplay.Display();
            readInputThread.Start();
        }
    }
}
