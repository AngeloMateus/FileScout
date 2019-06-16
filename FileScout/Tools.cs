using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileScout
{
    public static class Tools
    {
        public static List<string> selectionRegister = new List<string>();

        public static bool IsEmpty<T>( this IEnumerable<T> items )
        {
            return items != null && !items.Any();
        }

        //Returns a string with the size of a file
        public static string DisplayFileSize( string file )
        {
            string[] sizeSuffixes = { " b", " KB", " MB", " GB", " TB", " PB", " EB" };
            long fileSize = (long)new FileInfo( file ).Length;

            //byte=0, Kb=1, Mb=2 ...
            int magnitude = (int)Math.Log( fileSize, 1024 );

            decimal adjustedSize = (decimal)fileSize / (1L << (magnitude * 10));

            if (magnitude < 0)
            {
                magnitude = 0;
            }

            return string.Format( "{0,0}{1,-8}", Math.Round( adjustedSize, 1 ), sizeSuffixes[magnitude] );
        }

        //Sums the size of all files in a folder
        private static long CalculateFileSize( DirectoryInfo dir )
        {

            long sizeSum = dir.EnumerateFiles().Sum( file => file.Length );

            return sizeSum;
        }

        //Check if there are folders in a folder and recursively calculate foldersize
        //using CalculateFileSize() above
        private static long CalculateFolderSize( DirectoryInfo dir )
        {
            long totalSize = 0;
            try
            {
                IEnumerable<DirectoryInfo> dirInfos = dir.EnumerateDirectories();


                if (dirInfos.IsEmpty())
                {
                    totalSize += CalculateFileSize( dir );
                }
                else
                {
                    foreach (DirectoryInfo info in dirInfos)
                    {
                        totalSize += CalculateFolderSize( info );
                    }
                    totalSize += CalculateFileSize( dir );
                }
            }
            catch (UnauthorizedAccessException ua)
            {

            }

            return totalSize;
        }

        //Returns a string with the size of all files in a folder
        public static string DisplayFolderSize( string folder )
        {
            string[] sizeSuffixes = { " b", " KB", " MB", " GB", " TB", " PB", " EB" };
            DirectoryInfo dirInfo = new DirectoryInfo( folder );
            long totalSize = 0;
            totalSize = CalculateFolderSize( dirInfo );


            //byte=0, Kb=1, Mb=2 ...
            int magnitude = (int)Math.Log( totalSize, 1024 );

            decimal adjustedSize = (decimal)totalSize / (1L << (magnitude * 10));

            if (magnitude < 0)
            {
                magnitude = 0;
            }

            return string.Format( "{0,0}{1,-8}", Math.Round( adjustedSize, 1 ), sizeSuffixes[magnitude] );
        }

        public static void CopySelection( string item )
        {
            //move down if possible
            if (State.cursorPosY < ConsoleDisplay.files.Length - 1)
            {
                State.cursorPosY++;
                ConsoleDisplay.MoveDown();
            }
            if (!selectionRegister.Contains(item)) {
                selectionRegister.Add( item );
                Console.SetCursorPosition( 0, 0 );
                Console.Write( "Added " );
                Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write( Path.GetFileName( item ) );
                Console.ResetColor();
                Console.Write( " to Selection." );
            }
        }

        public static void CopyDirectory( string sourceDirectory, string destDirectory )
        {
            try
            {
                DirectoryInfo destDirectoryInfo = Directory.CreateDirectory( destDirectory );
                DirectoryInfo sourceDirInfo = new DirectoryInfo( sourceDirectory );
                IEnumerable<FileInfo> files = sourceDirInfo.EnumerateFiles();

                if (!files.IsEmpty())
                {
                    foreach (FileInfo file in files)
                    {
                        File.Copy( file.FullName, destDirectoryInfo.FullName + Path.DirectorySeparatorChar + file.Name );
                    }
                }

                IEnumerable<DirectoryInfo> subDirectories = sourceDirInfo.EnumerateDirectories();
                if (!subDirectories.IsEmpty())
                {
                    foreach (DirectoryInfo item in subDirectories)
                    {
                        string name = item.Name;
                        string dest = Path.Combine(destDirectory, name);
                        CopyDirectory( item.FullName, dest);
                    }
                }
            }
            catch (Exception e)
            {
                DisplayError( e );
            }
        }

        public static void PasteSelection()
        {
            try
            {
                if (!selectionRegister.IsEmpty())
                {
                    foreach (string item in selectionRegister)
                    {
                        FileAttributes attr = File.GetAttributes( item );
                        string itemName = Path.GetFileName( item );
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            CopyDirectory( item, State.currentPath + Path.DirectorySeparatorChar + Path.GetFileName( item ) );
                        }
                        else
                        {
                            File.Copy( item, State.currentPath + Path.DirectorySeparatorChar + itemName );
                        }
                    }
                }
                else {
                    DisplayError(new Exception("Nothing to paste"));
                }
                selectionRegister.Clear();
            }
            catch (Exception e)
            {
                DisplayError( e );
            }
        }

        //To be used on a catch block
        public static void DisplayError( Exception e )
        {
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.SetCursorPosition( 0, Console.WindowTop );
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write( "(!) " + e.Message);
            Console.ResetColor();
            Console.ReadKey( true );
            ConsoleDisplay.ClearLine( Console.WindowTop );
            ConsoleDisplay.ClearLine( Console.WindowTop + 1 );
        }
    }
}
