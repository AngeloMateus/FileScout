using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                Console.Clear();
                files = Directory.GetFileSystemEntries( currentPath );
                Console.CursorVisible = false;

                Array.Sort( files );

                //Write to Console
                writer.Write( currentPath + "\n\n" );

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
    }
}

