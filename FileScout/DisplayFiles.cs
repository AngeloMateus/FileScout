using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            files = Directory.GetFileSystemEntries( currentPath );
            Console.CursorVisible = false;

            Array.Sort( files );

            Console.Clear();
            //Write to Console

            Console.WriteLine( currentPath );

            for (int i = 0; i < files.Length; i++)
            {
                if (Cursor.cursorPosY == i)
                {
                    FileAttributes attr = File.GetAttributes( files[i] );
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine( Path.GetFileName( files[i] ) + "\\" );
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine( Path.GetFileName( files[i] ) );
                        Console.ResetColor();
                    }
                }
                else if(Cursor.cursorPosY != i)
                {
                    FileAttributes attr = File.GetAttributes( files[i] );
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        Console.WriteLine( Path.GetFileName(files[i]) + "\\" );
                    }
                    else
                    {
                        Console.WriteLine( Path.GetFileName( files[i] ) );
                    }
                }
            }
            Console.SetCursorPosition(0,0);
        }
    }
}

