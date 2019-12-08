using System;
using System.IO;
using System.Threading;

namespace FileScout
{

    class ConsoleDisplay
    {
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

        //Display Current Directory, files and folders and selected file
        public static void Display()
        {
            State.activeScreen = (int)State.screens.FILESYSTEM;
            Console.Clear();
            Console.SetCursorPosition( 0, 0 );
            Console.CursorVisible = false;

            try
            {
                //Sort and combine Directories and files
                files = CombineArrays( State.currentPath );

                if (files.Length > Console.WindowHeight)
                    Console.SetBufferSize( Console.WindowWidth, files.Length + Console.WindowHeight );

                Console.SetCursorPosition( 0, Console.WindowTop + 5 );

                if (Tools.IsEmpty( files ))
                    writer.WriteLine( "    (empty)" );
                //Output all files and folders
                for (int i = 0; i < files.Length; i++)
                {
                    string copiedIndicator;
                    if (Tools.selectionRegister.Contains( files[i] )) { copiedIndicator = "C"; } else { copiedIndicator = " "; };
                    if (State.cursorPosY == i)
                    {
                        FileAttributes attr = File.GetAttributes( files[i] );
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            writer.WriteLine( " ->" + copiedIndicator + "> " + ShortenFileName( Path.GetFileName( files[i] ), 26 ) + "\\" );
                            State.selectedFile = Path.GetFullPath( files[i] );
                        }
                        else
                        {
                            writer.WriteLine( " ->" + copiedIndicator + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[i] ), 26 )
                                + Path.GetExtension( files[i] ), Tools.DisplayFileSize( files[i] ) );
                            State.selectedFile = Path.GetFullPath( files[i] );
                        }
                    }
                    else if (State.cursorPosY != i)
                    {
                        FileAttributes attr = File.GetAttributes( files[i] );
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            writer.WriteLine( "   " + copiedIndicator + "> " + ShortenFileName( Path.GetFileName( files[i] ), 26 ) + "\\" );
                        }
                        else
                        {
                            writer.WriteLine( "   " + copiedIndicator + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[i] ), 26 )
                                + Path.GetExtension( files[i] ), Tools.DisplayFileSize( files[i] ) );
                        }
                    }
                }
                WatchFileSystem.RefreshWatcherPath();
            }
            catch (UnauthorizedAccessException)
            {
                Console.Clear();
                SetCursorToPreviousPosition();
                State.currentPath = Directory.GetParent( State.currentPath ).ToString();
                Display();
                Tools.DisplayError( new Exception( "Access Denied" ) );
            }

            writer.Flush();
            Console.SetCursorPosition( 0, State.cursorPosY );

            //Clear top of window and Write Current Directory
            ClearBlock( Console.WindowTop, Console.WindowTop + 5 );
            Console.SetCursorPosition( 0, Console.WindowTop );
            WriteCurrentPath();

            if (Console.BufferWidth > 59) { new ConsoleDisplayChild(); }
            //new FilePreview();
        }

        private static void SetCursorToPreviousPosition()
        {
            parentDirectory = Directory.GetParent( State.currentPath ).ToString();
            currentFileList = CombineArrays( parentDirectory );
            for (int i = 0; i < currentFileList.Length; i++)
            {
                if (string.Compare( State.currentPath, currentFileList[i] ) == 0)
                {
                    State.cursorPosY = i;
                }
            }
        }

        public static string[] CombineArrays( string path )
        {
            string[] directories = Directory.GetDirectories( path );
            string[] files = Directory.GetFiles( path );

            Array.Sort( files, new AlphaNumComparer() );
            Array.Sort( directories, new AlphaNumComparer() );

            string[] combinedDirectory = new string[directories.Length + files.Length];
            Array.Copy( directories, combinedDirectory, directories.Length );
            Array.Copy( files, 0, combinedDirectory, directories.Length, files.Length );
            return combinedDirectory;
        }

        //Replaces specified line with empty spaces
        public static void ClearLine( int posY )
        {
            Console.SetCursorPosition( 0, posY );
            Console.Write( new string( ' ', Console.WindowWidth ) );
        }

        //Replaces specified lines with empty spaces
        public static void ClearBlock( int start, int end )
        {
            while (start != end)
            {
                Console.SetCursorPosition( 0, start );
                Console.Write( new string( ' ', Console.WindowWidth ) );
                start++;
            }
        }

        public static void RedrawSelectedFile()
        {
            Console.SetCursorPosition( 0, topPadding + State.cursorPosY );
            FileAttributes attr = File.GetAttributes( State.selectedFile );

            string s;
            if (Tools.selectionRegister.Contains( State.selectedFile )) { s = "C"; } else { s = " "; };

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                writer.WriteLine( " ->" + s + "> " + ShortenFileName( Path.GetFileName( State.selectedFile ), 26 ) + "\\" );
                State.selectedFile = Path.GetFullPath( State.selectedFile );
            }
            else
            {
                writer.WriteLine( " ->" + s + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( State.selectedFile ), 26 )
                    + Path.GetExtension( State.selectedFile ), Tools.DisplayFileSize( State.selectedFile ) );
                State.selectedFile = Path.GetFullPath( State.selectedFile );
            }
            writer.Flush();
        }

        public static void MoveDown()
        {
            Console.CursorVisible = false;
            string s;
            if (Tools.selectionRegister.Contains( State.selectedFile )) { s = "C"; } else { s = " "; };

            FileAttributes attr = File.GetAttributes( files[State.cursorPosY - 1] );
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                writer.WriteLine( "   " + s + "> " + ShortenFileName( Path.GetFileName( files[State.cursorPosY - 1] ), 26 ) + "\\" );
            }
            else
            {
                writer.WriteLine( "   " + s + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[State.cursorPosY - 1] ), 26 )
                    + Path.GetExtension( files[State.cursorPosY - 1] ), Tools.DisplayFileSize( files[State.cursorPosY - 1] ) );
            }

            attr = File.GetAttributes( files[State.cursorPosY] );
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                writer.WriteLine( " -> > " + ShortenFileName( Path.GetFileName( files[State.cursorPosY] ), 26 ) + "\\" );
                State.selectedFile = Path.GetFullPath( files[State.cursorPosY] );
            }
            else
            {
                writer.WriteLine( " -> {0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[State.cursorPosY] ), 26 )
                    + Path.GetExtension( files[State.cursorPosY] ), Tools.DisplayFileSize( files[State.cursorPosY] ) );
                State.selectedFile = Path.GetFullPath( files[State.cursorPosY] );
            }
            ClearBlock( Console.WindowTop, Console.WindowTop + 5 );
            Console.SetCursorPosition( 0, State.cursorPosY + topPadding - 1 );
            writer.Flush();

            //Clear top of window and write current path
            ClearBlock( Console.WindowTop, Console.WindowTop + 5 );
            Console.SetCursorPosition( 0, Console.WindowTop );
            WriteCurrentPath();
            if (Console.BufferWidth > 59) { new ConsoleDisplayChild(); }
            //new FilePreview();
            if (Tools.selectionRegister.Contains( State.selectedFile )) RedrawSelectedFile();
        }

        public static void MoveUp()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition( 0, State.cursorPosY + topPadding );

            string s;
            if (Tools.selectionRegister.Contains( files[State.cursorPosY] )) { s = "C"; } else { s = " "; };

            FileAttributes attr = File.GetAttributes( files[State.cursorPosY] );
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                writer.WriteLine( " ->" + s + "> " + ShortenFileName( Path.GetFileName( files[State.cursorPosY] ), 26 ) + "\\" );
                State.selectedFile = Path.GetFullPath( files[State.cursorPosY] );
            }
            else
            {
                writer.WriteLine( " ->" + s + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[State.cursorPosY] ), 26 )
                    + Path.GetExtension( files[State.cursorPosY] ), Tools.DisplayFileSize( files[State.cursorPosY] ) );
                State.selectedFile = Path.GetFullPath( files[State.cursorPosY] );
            }

            if (Tools.selectionRegister.Contains( files[State.cursorPosY + 1] )) { s = "C"; } else { s = " "; };
            attr = File.GetAttributes( files[State.cursorPosY + 1] );
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                writer.WriteLine( "   " + s + "> " + ShortenFileName( Path.GetFileName( files[State.cursorPosY + 1] ), 26 ) + "\\" );
            }
            else
            {
                writer.WriteLine( "   " + s + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( files[State.cursorPosY + 1] ), 26 )
                    + Path.GetExtension( files[State.cursorPosY + 1] ), Tools.DisplayFileSize( files[State.cursorPosY + 1] ) );
            }
            writer.Flush();

            //Clear top of window and Write Current Directory
            Console.SetCursorPosition( 0, State.cursorPosY );
            ClearBlock( Console.WindowTop, Console.WindowTop + 5 );
            Console.SetCursorPosition( 0, Console.WindowTop );
            WriteCurrentPath();
            if (Console.BufferWidth > 59) { new ConsoleDisplayChild(); }
            //new FilePreview();
            if (Tools.selectionRegister.Contains( State.selectedFile )) RedrawSelectedFile();
        }

        private static void WriteCurrentPath()
        {
            string shortenedPath = ShortenPath( State.currentPath );

            if (Console.WindowTop != 0)
                writer.Write( "\n\n ^    " + shortenedPath + "\n      " );
            else
                writer.Write( "\n\n      " + shortenedPath + "\n      " );


            for (int i = 0; i < shortenedPath.Length; i++)
            {
                writer.Write( "_" );
            }
            writer.Write( "\n\n" );
            writer.Flush();
        }

        public static string ShortenFileName( string path, int maxLength )
        {
            if (path.Length > maxLength)
                return path.Substring( 0, maxLength ) + "~";
            else
                return path;
        }

        private static string ShortenPath( string path )
        {
            string drive = Path.GetPathRoot( path );
            if (path.Length >= 45)
            {
                path = path.Substring( path.Length - 45 );
                int index = path.IndexOf( Path.DirectorySeparatorChar );

                if (index < 0)
                    index = 0;

                path = path.Substring( index );
                path = drive + "..." + path;

                return path;
            }
            else
                return path;
        }
    }
}
