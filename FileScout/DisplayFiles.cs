using System;
using System.IO;
using System.Threading;

namespace FileScout
{
    class DisplayFiles
    {
        public static string currentPath = Directory.GetCurrentDirectory();
        public static string[] files;

        //Display Current Dir files and folders and selected file
        public static void Display()
        {
            StreamWriter writer = new StreamWriter( Console.OpenStandardOutput() );
            try
            {
                files = CombineArrays();
                Console.Clear();

                Console.CursorVisible = false;


                //Write to Console
                writer.Write("   "+ currentPath +"\n   ");
                for (int i = 0; i < currentPath.Length; i++)
                {
                    writer.Write("_");
                }
                writer.Write("\n\n");

                for (int i = 0; i < files.Length; i++)
                {
                    if (Cursor.cursorPosY == i)
                    {
                        FileAttributes attr = File.GetAttributes( files[i] );
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            writer.WriteLine( "-> " + Path.GetFileName( files[i] ) + "\\" );
                        }
                        else
                        {
                            writer.WriteLine( "-> " + Path.GetFileName( files[i] ) );
                        }
                    }
                    else if (Cursor.cursorPosY != i)
                    {
                        FileAttributes attr = File.GetAttributes( files[i] );
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            writer.WriteLine( "   "+Path.GetFileName( files[i] ) + "\\" );

                        }
                        else
                        {
                            writer.WriteLine( "   "+Path.GetFileName( files[i] ) );

                        }
                    }
                }
                writer.Flush();

                Console.SetCursorPosition( 0, Cursor.cursorPosY );
            }
            catch(UnauthorizedAccessException)
            {
                Console.Clear();
                Console.WriteLine( "Access Denied" );
                Thread.Sleep( 300 );
                string parentDirectory = Directory.GetParent( currentPath ).ToString();
                string[] currentFileList = Directory.GetFileSystemEntries( parentDirectory );
                for (int i = 0; i < currentFileList.Length; i++)
                {
                    if (string.Compare( currentPath, currentFileList[i] ) == 0)
                    {
                        Cursor.cursorPosY = i;
                    }
                }
                currentPath = Directory.GetParent( currentPath ).ToString();
                Display();
            }
        }
        public static string[] CombineArrays()
        {
            string[] directories = Directory.GetDirectories( currentPath );
            string[] files = Directory.GetFiles( currentPath );
            string[] combinedDirectory = new string[directories.Length + files.Length];
            Array.Copy( directories, combinedDirectory, directories.Length );
            Array.Copy(files, 0,combinedDirectory,directories.Length,files.Length);
            return combinedDirectory;
        }
    }
}

