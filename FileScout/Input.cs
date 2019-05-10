using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace FileScout
{
    class Input
    {
        string selectedFile;
        string[] currentFileList;
        string parentDirectory;

        public Input()
        {
            Console.CursorVisible = false;
        }

        public void StartReading()
        {
            ConsoleKeyInfo consoleKeyInfo;

            do
            {
                string[] directoryArray = CombineArrays( ConsoleDisplay.currentPath );

                if (directoryArray.Length != 0)
                    selectedFile = directoryArray[Cursor.cursorPosY];


                //Read key to change cursor
                consoleKeyInfo = Console.ReadKey( true );
                switch (consoleKeyInfo.KeyChar)
                {
                    case 'k':
                        if (Cursor.cursorPosY > 0)
                        {
                            Cursor.cursorPosY--;
                            ConsoleDisplay.MoveUp();
                        }
                        break;
                    case 'j':
                        if (Cursor.cursorPosY < ConsoleDisplay.files.Length - 1)
                        {
                            Cursor.cursorPosY++;
                            ConsoleDisplay.MoveDown();
                        }
                        break;
                    case 'l':
                        {
                            //If it it's a folder allow press right key
                            FileAttributes attr = File.GetAttributes( selectedFile );

                            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                ConsoleDisplay.currentPath = selectedFile;
                                History.SetPointer();
                                ConsoleDisplay.Display();
                            }
                        }
                        break;
                    case 'h':
                        {
                            History.AddEntry();

                            //Set cursor to Parent folder
                            if (Directory.GetParent( ConsoleDisplay.currentPath ) != null)
                            {
                                SetCursorToPreviousPosition();
                                ConsoleDisplay.currentPath = Directory.GetParent( ConsoleDisplay.currentPath ).ToString();
                            }
                            ConsoleDisplay.Display();
                        }
                        break;
                    case 't':
                        {
                            Console.Clear();
                            Process process = new Process();
                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            process.StartInfo = startInfo;
                            startInfo.FileName = "cmd";
                            startInfo.UseShellExecute = true;
                            startInfo.WorkingDirectory = ConsoleDisplay.currentPath;
                            process.Start();
                            process.WaitForExit();
                            ConsoleDisplay.Display();
                        }
                        break;
                    case ':':
                        {
                            new InputBox().CommadMode();
                        }
                        break;
                    case '/':
                        {
                            new InputBox().SearchMode();
                        }
                        break;
                    case '.':
                        {
                            new InputBox().JumpToLetter();
                        }
                        break;
                    case '\r':
                        {
                            try
                            {
                                ProcessStartInfo startInfo = new ProcessStartInfo();
                                startInfo.FileName = @selectedFile;
                                startInfo.RedirectStandardOutput = false;
                                Process newProcess = Process.Start( startInfo );
                            }
                            catch (Exception e)
                            {
                                ConsoleDisplay.ClearLine( Console.WindowTop );
                                Console.SetCursorPosition( 0, Console.WindowTop );
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write( "(!) " + e.Message );
                                Console.ResetColor();
                                Console.ReadKey( true );
                                ConsoleDisplay.Display(); ;
                            }
                        }
                        break;
                    case 'r':
                        {
                            new InputBox().RenameFile( selectedFile );
                            ConsoleDisplay.Display();
                        }
                        break;
                }
            }

            while (consoleKeyInfo.Key != ConsoleKey.Escape);
            Console.Clear();
        }

        //Select Parent folder and set cursor to this position
        private void SetCursorToPreviousPosition()
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

            Array.Sort( files, new AlphaNumComparer() );
            Array.Sort( directories, new AlphaNumComparer() );

            string[] combinedDirectory = new string[directories.Length + files.Length];
            Array.Copy( directories, combinedDirectory, directories.Length );
            Array.Copy( files, 0, combinedDirectory, directories.Length, files.Length );
            return combinedDirectory;
        }
    }
}
