using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileScout
{
    class ReadInput
    {
        string selectedFile;
        string[] currentFileList;
        string parentDirectory;

        public ReadInput()
        {
            Console.CursorVisible = false;
        }

        public void StartReading()
        {
            try
            {
                ConsoleKeyInfo consoleKeyInfo;

                do
                {

                    string[] directoryArray = Directory.GetFileSystemEntries( DisplayFiles.currentPath );
                    Array.Sort( directoryArray );

                    if (directoryArray.Length != 0)
                        selectedFile = directoryArray[Cursor.cursorPosY];

                    //Read key to change cursor
                    consoleKeyInfo = Console.ReadKey( true );
                    switch (consoleKeyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (Cursor.cursorPosY > 0)
                            {
                                Cursor.cursorPosY--;
                                DisplayFiles.Display();
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (Cursor.cursorPosY < DisplayFiles.files.Length - 1)
                            {
                                Cursor.cursorPosY++;
                                DisplayFiles.Display();
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            {
                                FileAttributes attr = File.GetAttributes( selectedFile );

                                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                                {
                                    DisplayFiles.currentPath = selectedFile;
                                    Cursor.cursorPosY = 0;
                                }

                                DisplayFiles.Display();
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            {
                                if (Directory.GetParent( DisplayFiles.currentPath ) != null)
                                {
                                    parentDirectory = Directory.GetParent( DisplayFiles.currentPath ).ToString();
                                    currentFileList = Directory.GetFileSystemEntries( parentDirectory );
                                    for (int i = 0; i < currentFileList.Length; i++)
                                    {
                                        if (string.Compare( DisplayFiles.currentPath, currentFileList[i] ) == 0)
                                        {
                                            Cursor.cursorPosY = i;
                                        }
                                    }


                                    if (Directory.GetParent( DisplayFiles.currentPath ) != null)
                                        DisplayFiles.currentPath = Directory.GetParent( DisplayFiles.currentPath ).ToString();
                                }
                                DisplayFiles.Display();
                            }
                            break;
                    }
                    Thread.Sleep( 8 );

                }

                while (consoleKeyInfo.Key != ConsoleKey.Q && consoleKeyInfo.Key != ConsoleKey.Escape);
                Console.WriteLine( "Exiting..." );
                Console.Clear();
            }
            catch (UnauthorizedAccessException)
            {
                Console.Clear();
                Console.WriteLine("Access Denied");
                Thread.Sleep(300);
                parentDirectory = Directory.GetParent( DisplayFiles.currentPath ).ToString();
                currentFileList = Directory.GetFileSystemEntries( parentDirectory );
                for (int i = 0; i < currentFileList.Length; i++)
                {
                    if (string.Compare( DisplayFiles.currentPath, currentFileList[i] ) == 0)
                    {
                        Cursor.cursorPosY = i;
                    }
                }
                DisplayFiles.currentPath = Directory.GetParent( DisplayFiles.currentPath ).ToString();
                DisplayFiles.Display();
            }

        }
    }
}
