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
            File.WriteAllText( Path.GetDirectoryName( System.Reflection.Assembly.GetEntryAssembly().Location ) +
                Path.DirectorySeparatorChar + "fileScoutDir", "" );

            //Watches window properties
            WindowProperties wp = new WindowProperties();
            Thread windowPropertiesThread = new Thread( new ThreadStart( wp.Start ) );

            if (args.Length > 0)
            {
                Console.WriteLine( "Args: " + args[0] );
                if (Directory.Exists( args[0] ))
                {
                    State.currentPath = Path.GetFullPath( args[0] );
                }
                else
                {
                    Console.WriteLine( "Path does not exists." );
                    return;
                }
            }
            else
            {
                State.currentPath = Directory.GetCurrentDirectory();
            }

            //Watches for changes in filesystem and calls Display() when changes occur.
            new WatchFileSystem().CheckFiles();

            //Handles console display, directory sorting, path shortening, etc.
            ConsoleDisplay.Display();
            StartInputThread();
            windowPropertiesThread.Start();
        }

        //Reads user input on it's own thread
        public static void StartInputThread()
        {
            Input readInput = new Input();
            Thread readInputThread = new Thread( new ThreadStart( readInput.StartReading ) );
            readInputThread.SetApartmentState( ApartmentState.STA );
            readInputThread.Start();
        }
    }
}
