using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace FileScout
{

    class ConsoleDisplay
    {
        public static string currentPath = Directory.GetCurrentDirectory();
        public static string[] files;
        private static string[] currentFileList;
        private static string parentDirectory;
        private static StreamWriter writer;
        private static readonly int topPadding = 5;

        static ConsoleDisplay()
        {
            writer = new StreamWriter( Console.OpenStandardOutput() );
            writer.AutoFlush = false;
        }


        //Display Current Dir files and folders and selected file
        public static void Display()
        {
            Console.Clear();

            Console.CursorVisible = false;
            try
            {
                //Sort and combine Direcotries and files
                files = CombineArrays( currentPath );

                //Output the Current Path

                Console.SetCursorPosition( 0, Console.WindowTop + 5 );
                //Output all files and folders
                for (int i = 0; i < files.Length; i++)
                {
                    if (Cursor.cursorPosY == i)
                    {
                        FileAttributes attr = File.GetAttributes( files[i] );
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            writer.WriteLine( " -> > " + ShortenFileName( Path.GetFileNameWithoutExtension( files[i] ), 26 ) + Path.GetExtension( files[i] ) + "\\" );
                        }
                        else
                        {
                            writer.WriteLine( " -> {0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[i] ), 26 )
                                + Path.GetExtension( files[i] ), CalculateFileSize( files[i] ) );
                        }
                    }
                    else if (Cursor.cursorPosY != i)
                    {
                        FileAttributes attr = File.GetAttributes( files[i] );
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            writer.WriteLine( "    > " + ShortenFileName( Path.GetFileNameWithoutExtension( files[i] ), 26 ) + Path.GetExtension( files[i] ) + "\\" );

                        }
                        else
                        {
                            writer.WriteLine( "    {0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[i] ), 26 )
                                + Path.GetExtension( files[i] ), CalculateFileSize( files[i] ) );
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine( "Access Denied" );
                Console.ResetColor();
                Thread.Sleep( 460 );
                SetCursorToPreviousPosition();
                currentPath = Directory.GetParent( currentPath ).ToString();
                Display();
            }
            writer.Flush();

            Console.SetCursorPosition( 0, Cursor.cursorPosY );

            ClearBlock( Console.WindowTop, Console.WindowTop + 5 );
            Console.SetCursorPosition( 0, Console.WindowTop );
            WriteCurrentPath();
        }

        private static void SetCursorToPreviousPosition()
        {
            parentDirectory = Directory.GetParent( ConsoleDisplay.currentPath ).ToString();
            currentFileList = CombineArrays( parentDirectory );
            for (int i = 0; i < currentFileList.Length; i++)
            {
                if (string.Compare( ConsoleDisplay.currentPath, currentFileList[i] ) == 0)
                {
                    Cursor.cursorPosY = i;
                }
            }
        }

        private static string[] CombineArrays( string path )
        {
            string[] directories = Directory.GetDirectories( path );
            string[] files = Directory.GetFiles( path );

            //Array.Sort( files, new AlphaNumComparer() );
            Array.Sort( directories, new AlphaNumComparer() );

            string[] combinedDirectory = new string[directories.Length + files.Length];
            Array.Copy( directories, combinedDirectory, directories.Length );
            Array.Copy( files, 0, combinedDirectory, directories.Length, files.Length );
            return combinedDirectory;
        }

        public static void ClearLine( int posY )
        {
            Console.SetCursorPosition( 0, posY );
            Console.Write( new string( ' ', Console.WindowWidth ) );
        }
        public static void ClearBlock( int start, int end )
        {
            while (start != end)
            {
                Console.SetCursorPosition( 0, start );
                Console.Write( new string( ' ', Console.WindowWidth ) );
                start++;
            }
        }

        public static void MoveDown()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition( 0, Cursor.cursorPosY + topPadding - 1 );

            FileAttributes attr = File.GetAttributes( files[Cursor.cursorPosY - 1] );
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                writer.WriteLine( "    > " + ShortenFileName( Path.GetFileName( files[Cursor.cursorPosY - 1] ), 26 ) + "\\" );
            }
            else
            {
                writer.WriteLine( "    {0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[Cursor.cursorPosY - 1] ), 26 )
                    + Path.GetExtension( files[Cursor.cursorPosY - 1] ), CalculateFileSize( files[Cursor.cursorPosY - 1] ) );
            }
            Console.SetCursorPosition( 0, Cursor.cursorPosY + topPadding - 1 );

            attr = File.GetAttributes( files[Cursor.cursorPosY] );
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                writer.WriteLine( " -> > " + ShortenFileName( Path.GetFileName( files[Cursor.cursorPosY] ), 26 ) + "\\" );
            }
            else
            {
                writer.WriteLine( " -> {0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[Cursor.cursorPosY] ), 26 )
                    + Path.GetExtension( files[Cursor.cursorPosY] ), CalculateFileSize( files[Cursor.cursorPosY] ) );
            }
            writer.Flush();
            ClearBlock( Console.WindowTop, Console.WindowTop + 5 );
            Console.SetCursorPosition( 0, Console.WindowTop );
            WriteCurrentPath();
        }

        public static void MoveUp()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition( 0, Cursor.cursorPosY + topPadding + 1 );

            FileAttributes attr = File.GetAttributes( files[Cursor.cursorPosY + 1] );
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                writer.WriteLine( "    > " + ShortenFileName( Path.GetFileName( files[Cursor.cursorPosY + 1] ), 26 ) + "\\" );
            }
            else
            {
                writer.WriteLine( "    {0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[Cursor.cursorPosY + 1] ), 26 )
                    + Path.GetExtension( files[Cursor.cursorPosY + 1] ), CalculateFileSize( files[Cursor.cursorPosY + 1] ) );
            }
            writer.Flush();
            Console.SetCursorPosition( 0, Cursor.cursorPosY + topPadding );

            attr = File.GetAttributes( files[Cursor.cursorPosY] );
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                writer.WriteLine( " -> > " + ShortenFileName( Path.GetFileName( files[Cursor.cursorPosY] ), 26 ) + "\\" );
            }
            else
            {
                writer.WriteLine( " -> {0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[Cursor.cursorPosY] ), 26 )
                    + Path.GetExtension( files[Cursor.cursorPosY] ), CalculateFileSize( files[Cursor.cursorPosY] ) );
            }
            writer.Flush();
            Console.SetCursorPosition( 0, Cursor.cursorPosY );
            ClearBlock( Console.WindowTop, Console.WindowTop + 5 );
            Console.SetCursorPosition( 0, Console.WindowTop );
            WriteCurrentPath();
        }

        private static string CalculateFileSize( string file )
        {
            string[] sizeSuffixes = { " b", " KB", " MB", " GB", " TB", " PB", " EB" };
            double fileSize = (double)new FileInfo( file ).Length;

            //byte=0, Kb=1, Mb=2 ...
            int magnitude = (int)Math.Log( fileSize, 1000 );

            decimal adjustedSize = (decimal)fileSize / (1L << (magnitude * 10));

            if (magnitude < 0)
            {
                magnitude = 0;
            }

            return string.Format( "{0,0}{1,-8}", Math.Round( adjustedSize, 2 ), sizeSuffixes[magnitude] );
        }
        private static void WriteCurrentPath()
        {
            string shortenedPath = ShortenPath( currentPath );
            writer.Write( "\n\n   " + shortenedPath + "\n   " );

            for (int i = 0; i < shortenedPath.Length ; i++)
            {
                writer.Write( " " );
            }
            writer.Write( "\n\n" );
            writer.Flush();
        }

        private static string ShortenFileName( string path, int maxLength )
        {
            if (path.Length > maxLength)
                return path.Substring( 0, maxLength ) + "~";
            else
                return path;
        }

        private static string ShortenPath( string path )
        {
            if (path.Length > 45)
            {
                path = path.Substring(0,3) +"..."+path.Substring(path.Length - 45);
                return path;
            }
            else
            return path;
        }

    }
}

