using System;
using System.Diagnostics;
using System.Threading;
using System.IO;

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

            if (args.Length > 0)
            {
                Console.WriteLine( "Args: " + args[0] );
                if (Directory.Exists( args[0] ))
                {
                    State.currentPath = Path.GetFullPath(args[0]);
                }
                else
                {
                    Console.WriteLine("Path does not exists.");
                    return;
                }
            }
            else
            {
                    State.currentPath = Environment.GetFolderPath( Environment.SpecialFolder.UserProfile );
            }


            //Watches for changes in filesystem and calls Display() when changes occur.
            new WatchFileSystem().CheckFiles();

            //Handles console display, directory sorting, path shortening, etc.
            ConsoleDisplay.Display();
            readInputThread.Start();
        }
    }
}
