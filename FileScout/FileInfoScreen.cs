using System;
using System.IO;

namespace FileScout
{
    class FileInfoScreen
    {
        //Displays a new screen with file information
        public FileInfoScreen( String file )
        {
            Console.Clear();
            Console.Write( new string( '=', Console.WindowWidth / 2 - 10 ) );
            Console.Write( "  File Information  " );
            Console.Write( new string( '=', Console.WindowWidth / 2 - 10 ) );
            Console.SetCursorPosition( 0, 10 );
            Console.Write( new string( '=', Console.WindowWidth ) );
            Console.SetCursorPosition( 0, 4 );


            FileAttributes attr = File.GetAttributes( file );
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Console.SetCursorPosition( 0, Console.WindowTop + 4 );
                Console.WriteLine( "{0,-21}{1,0}", "  Directory Name: ", Path.GetFileName( file ) );
                Console.WriteLine( "{0,-21}{1,0}", "  Full Path: ", file );
                Console.WriteLine( "{0,-21}{1,0}", "  Size: ", "Calculating..." );
                Console.SetCursorPosition( 0, Console.WindowTop + 6 );

                string folderSize = Tools.DisplayFolderSize( file );
                Console.WriteLine( "{0,-21}{1,0}", "  Size: ", folderSize + "       " );

            }
            else
            {
                Console.SetCursorPosition( 0, Console.WindowTop + 4 );
                Console.WriteLine( "{0,-21}{1,0}", "  File Name: ", Path.GetFileName( file ) );
                Console.WriteLine( "{0,-21}{1,0}", "  Full Path: ", file );
                Console.WriteLine( "{0,-21}{1,0}", "  Size: ", "Calculating..." );
                Console.SetCursorPosition( 0, Console.WindowTop + 6 );
                
                string fileSize = Tools.DisplayFileSize( file );
                Console.WriteLine( "{0,-21}{1,0}", "  Size: ", fileSize + "       " );
            }

            Console.ReadKey( true );
            ConsoleDisplay.Display();
        }
    }
}
