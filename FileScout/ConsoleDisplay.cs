using System;
using System.IO;
using System.Threading;
using System.Linq;

namespace FileScout
{

    class ConsoleDisplay
    {
        public static string[] files;
        public static string[] currentChunkFiles;
        private static string[][] fileChunks;
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
                //Sort and combine Directories and Files
                files = CombineArrays( State.currentPath );
                //Create file chunk sizes to fit in window
                fileChunks = ChunkerizeFiles( files );

                Console.SetCursorPosition( 0, Console.WindowTop + 5 );

                if (Tools.IsEmpty( files ))
                {
                    writer.WriteLine( "    (empty)" );
                }
                else
                {
                    //Output all files and folders
                    currentChunkFiles = fileChunks[State.currentFileChunk];

                    for (int j = 0; j < currentChunkFiles.Length; j++)
                    {
                        string copiedIndicator;
                        if (Tools.selectionRegister.Contains( currentChunkFiles[j] )) { copiedIndicator = "*"; } else { copiedIndicator = " "; };
                        if (State.cursorPosY == j)
                        {
                            FileAttributes attr = File.GetAttributes( currentChunkFiles[j] );
                            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                writer.WriteLine( " ->" + copiedIndicator + "> " + ShortenFileName( Path.GetFileName( currentChunkFiles[j] ), 26 ) + "\\" );
                                State.selectedFile = Path.GetFullPath( currentChunkFiles[j] );
                            }
                            else
                            {
                                writer.WriteLine( " ->" + copiedIndicator + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( currentChunkFiles[j] ), 26 )
                                    + Path.GetExtension( currentChunkFiles[j] ), Tools.DisplayFileSize( currentChunkFiles[j] ) );
                                State.selectedFile = Path.GetFullPath( currentChunkFiles[j] );
                            }
                        }
                        else if (State.cursorPosY != j)
                        {
                            FileAttributes attr = File.GetAttributes( currentChunkFiles[j] );
                            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                writer.WriteLine( "   " + copiedIndicator + "> " + ShortenFileName( Path.GetFileName( currentChunkFiles[j] ), 26 ) + "\\" );
                            }
                            else
                            {
                                writer.WriteLine( "   " + copiedIndicator + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( currentChunkFiles[j] ), 26 )
                                    + Path.GetExtension( currentChunkFiles[j] ), Tools.DisplayFileSize( currentChunkFiles[j] ) );
                            }
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
            catch (IndexOutOfRangeException e)
            {
                Tools.DisplayError(e);
            }

            writer.Flush();
            Console.SetCursorPosition( 0, State.cursorPosY );

            //Clear top of window and Write Current Directory
            ClearBlock( Console.WindowTop, Console.WindowTop + 5 );
            Console.SetCursorPosition( 0, Console.WindowTop );
            WriteCurrentPath();

            if (Console.BufferWidth > 94) { new ConsoleDisplayChild(); }
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

            string copiedIndicator;
            if (Tools.selectionRegister.Contains( State.selectedFile )) { copiedIndicator = "*"; } else { copiedIndicator = " "; };

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                writer.WriteLine( " ->" + copiedIndicator + "> " + ShortenFileName( Path.GetFileName( State.selectedFile ), 26 ) + "\\" );
                State.selectedFile = Path.GetFullPath( State.selectedFile );
            }
            else
            {
                writer.WriteLine( " ->" + copiedIndicator + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( State.selectedFile ), 26 )
                    + Path.GetExtension( State.selectedFile ), Tools.DisplayFileSize( State.selectedFile ) );
                State.selectedFile = Path.GetFullPath( State.selectedFile );
            }
            writer.Flush();
        }

        public static void MoveDown()
        {
            Console.CursorVisible = false;
            if (State.cursorPosY == currentChunkFiles.Length - 1)
            {
                if(State.currentFileChunk + 1< fileChunks.Length)
                State.currentFileChunk += 1 ;
                State.cursorPosY = 0;
                Display();
            }
            else
            {
                if (State.cursorPosY < ConsoleDisplay.files.Length - 1)
                {
                    State.cursorPosY++;

                    string copiedIndicator;
                    if (Tools.selectionRegister.Contains( State.selectedFile )) { copiedIndicator = "*"; } else { copiedIndicator = " "; };

                    FileAttributes attr = File.GetAttributes( currentChunkFiles[State.cursorPosY - 1] );
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        writer.WriteLine( "   " + copiedIndicator + "> " + ShortenFileName( Path.GetFileName( currentChunkFiles[State.cursorPosY - 1] ), 26 ) + "\\" );
                    }
                    else
                    {
                        writer.WriteLine( "   " + copiedIndicator + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( currentChunkFiles[State.cursorPosY - 1] ), 26 )
                            + Path.GetExtension( currentChunkFiles[State.cursorPosY - 1] ), Tools.DisplayFileSize( currentChunkFiles[State.cursorPosY - 1] ) );
                    }

                    attr = File.GetAttributes( currentChunkFiles[State.cursorPosY] );
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        writer.WriteLine( " -> > " + ShortenFileName( Path.GetFileName( currentChunkFiles[State.cursorPosY] ), 26 ) + "\\" );
                        State.selectedFile = Path.GetFullPath( currentChunkFiles[State.cursorPosY] );
                    }
                    else
                    {
                        writer.WriteLine( " -> {0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( currentChunkFiles[State.cursorPosY] ), 26 )
                            + Path.GetExtension( currentChunkFiles[State.cursorPosY] ), Tools.DisplayFileSize( currentChunkFiles[State.cursorPosY] ) );
                        State.selectedFile = Path.GetFullPath( currentChunkFiles[State.cursorPosY] );
                    }
                    ClearBlock( Console.WindowTop, Console.WindowTop + 5 );
                    Console.SetCursorPosition( 0, State.cursorPosY + topPadding - 1 );
                    writer.Flush();

                    //Clear top of window and write current path
                    ClearBlock( Console.WindowTop, Console.WindowTop + 5 );
                    Console.SetCursorPosition( 0, Console.WindowTop );
                    WriteCurrentPath();
                    if (Console.BufferWidth > 94) { new ConsoleDisplayChild(); }
                    //new FilePreview();
                    if (Tools.selectionRegister.Contains( State.selectedFile )) RedrawSelectedFile();
                }
            }
        }

        public static void MoveUp()
        {
            Console.CursorVisible = false;
            if (State.cursorPosY == 0 && State.currentFileChunk > 0)
            {
                State.currentFileChunk -= 1;
                State.cursorPosY = currentChunkFiles.Length -1;
                Display();
            }
            else
            {
                if (State.cursorPosY > 0)
                {
                    State.cursorPosY--;
                    Console.SetCursorPosition( 0, State.cursorPosY + topPadding );

                    string copiedIndicator;
                    if (Tools.selectionRegister.Contains( currentChunkFiles[State.cursorPosY] )) { copiedIndicator = "*"; } else { copiedIndicator = " "; };

                    FileAttributes attr = File.GetAttributes( currentChunkFiles[State.cursorPosY] );
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        writer.WriteLine( " ->" + copiedIndicator + "> " + ShortenFileName( Path.GetFileName( currentChunkFiles[State.cursorPosY] ), 26 ) + "\\" );
                        State.selectedFile = Path.GetFullPath( currentChunkFiles[State.cursorPosY] );
                    }
                    else
                    {
                        writer.WriteLine( " ->" + copiedIndicator + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( currentChunkFiles[State.cursorPosY] ), 26 )
                            + Path.GetExtension( currentChunkFiles[State.cursorPosY] ), Tools.DisplayFileSize( currentChunkFiles[State.cursorPosY] ) );
                        State.selectedFile = Path.GetFullPath( currentChunkFiles[State.cursorPosY] );
                    }

                    if (Tools.selectionRegister.Contains( currentChunkFiles[State.cursorPosY + 1] )) { copiedIndicator = "*"; } else { copiedIndicator = " "; };
                    attr = File.GetAttributes( currentChunkFiles[State.cursorPosY + 1] );
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        writer.WriteLine( "   " + copiedIndicator + "> " + ShortenFileName( Path.GetFileName( currentChunkFiles[State.cursorPosY + 1] ), 26 ) + "\\" );
                    }
                    else
                    {
                        writer.WriteLine( "   " + copiedIndicator + "{0,-40}{1,16}", ShortenFileName( Path.GetFileNameWithoutExtension( currentChunkFiles[State.cursorPosY + 1] ), 26 )
                            + Path.GetExtension( currentChunkFiles[State.cursorPosY + 1] ), Tools.DisplayFileSize( currentChunkFiles[State.cursorPosY + 1] ) );
                    }
                    writer.Flush();

                    //Clear top of window and Write Current Directory
                    Console.SetCursorPosition( 0, State.cursorPosY );
                    ClearBlock( Console.WindowTop, Console.WindowTop + 5 );
                    Console.SetCursorPosition( 0, Console.WindowTop );
                    WriteCurrentPath();
                    if (Console.BufferWidth > 94) { new ConsoleDisplayChild(); }
                    //new FilePreview();
                    if (Tools.selectionRegister.Contains( State.selectedFile )) RedrawSelectedFile();
                }
            }
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

        private static string[][] ChunkerizeFiles( string[] files )
        {
            //Divides files array into numbered chunks [chunk][file]
            string[][] chunks = files
            .Select( ( s, i ) => new { Value = s, Index = i } )
            .GroupBy( x => x.Index / (Console.WindowHeight - topPadding - 1) )
            .Select( grp => grp.Select( x => x.Value ).ToArray() )
            .ToArray();

            return chunks;
        }
    }
}
