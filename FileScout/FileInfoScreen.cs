using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace FileScout
{

    class FileInfoScreen
    {

        //Displays a new screen with file information
        public FileInfoScreen( String file )
        {
            State.activeScreen = (int)State.screens.INFO;
            Console.Clear();
            Console.Write( new string( '=', Console.WindowWidth / 2 - 10 ) );
            Console.Write( "  File Information  " );
            Console.Write( new string( '=', Console.WindowWidth / 2 - 10 ) );
            Console.SetCursorPosition( 0, 11 );
            Console.Write( new string( '=', Console.WindowWidth ) );
            Console.SetCursorPosition( 0, 4 );

            FileAttributes attr = File.GetAttributes( file );
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Console.SetCursorPosition( 0, Console.WindowTop + 4 );
                Console.WriteLine( "{0,-21}{1,0}", "  Directory Name: ", Path.GetFileName( file ) );
                Console.WriteLine( "{0,-21}{1,0}", "  Full Path: ", file );
                Console.WriteLine( "{0,-21}{1,0}", "  Size: ", "Calculating..." );

                Thread fsThread = new Thread(() => Tools.DisplayFolderSize(file));
                fsThread.Start();

                Console.WriteLine( "{0,-21}{1,0}", "  Last Modified: ", Directory.GetLastWriteTime( file ) );
                Console.WriteLine( "{0,-21}{1,0}", "  Created: ", Directory.GetCreationTime( file ) );
            }
            else
            {
                Console.SetCursorPosition( 0, Console.WindowTop + 4 );
                Console.WriteLine( "{0,-21}{1,0}", "  File Name: ", Path.GetFileName( file ) );
                Console.WriteLine( "{0,-21}{1,0}", "  Full Path: ", file );
                Console.WriteLine( "{0,-21}{1,0}", "  Size: ", "Calculating..." );
                Console.SetCursorPosition( 0, Console.WindowTop + 6 );

                string fileSize = Tools.DisplayFileSize( file );
                Console.WriteLine( "{0,-21}{1,0}", "  Size: ", fileSize + "       \n" );
                Console.WriteLine( "{0,-21}{1,0}", "  Last Modified: ", File.GetLastWriteTime( file ) );
                Console.WriteLine( "{0,-21}{1,0}", "  Created: ", File.GetCreationTime( file ) );
            }

            Console.SetCursorPosition( 0, 14 );
            Console.WriteLine( "  (c)opy full path to clipboard" );
            ConsoleKeyInfo key = Console.ReadKey( true );

            if (key.KeyChar == 'c')
            {
                Clipboard.SetData( DataFormats.Text, (Object)file );
            }

            ConsoleDisplay.Display();
        }
    }
}
