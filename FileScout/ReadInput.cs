using System;
using System.IO;
using System.Threading;

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
                    string[] directoryArray = CombineArrays(DisplayFiles.currentPath);



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
                                    GetPreviousCursorPosition();

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
                GetPreviousCursorPosition();
                DisplayFiles.currentPath = Directory.GetParent( DisplayFiles.currentPath ).ToString();
                DisplayFiles.Display();
            }

        }

        private void GetPreviousCursorPosition()
        {
            parentDirectory = Directory.GetParent( DisplayFiles.currentPath ).ToString();
            currentFileList = CombineArrays( parentDirectory ) ;
            for (int i = 0; i < currentFileList.Length; i++)
            {
                if (string.Compare( DisplayFiles.currentPath, currentFileList[i] ) == 0)
                {
                    Cursor.cursorPosY = i;
                }
            }
        }

        public static string[] CombineArrays(string path)
        {
            string[] directories = Directory.GetDirectories( path );
            string[] files = Directory.GetFiles( path );
            string[] combinedDirectory = new string[directories.Length + files.Length];
            Array.Copy( directories, combinedDirectory, directories.Length );
            Array.Copy( files, 0, combinedDirectory, directories.Length, files.Length );
            return combinedDirectory;
        }

    }
}
