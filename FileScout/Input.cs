using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            State.findKeyMatches = new List<int>();
        }

        public void StartReading()
        {
            ConsoleKeyInfo consoleKeyInfo;

            do
            {
                //maybe this is a fix? It works so far leave it for a bit
                //string[] directoryArray = CombineArrays( ConsoleDisplay.currentPath );
                string[] directoryArray = ConsoleDisplay.files;

                if (!directoryArray.IsEmpty())
                    selectedFile = directoryArray[State.cursorPosY];

                //Read key to change cursor
                consoleKeyInfo = Console.ReadKey( true );
                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (State.cursorPosY > 0)
                        {
                            State.cursorPosY--;
                            ConsoleDisplay.MoveUp();
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (State.cursorPosY < ConsoleDisplay.files.Length - 1)
                        {
                            State.cursorPosY++;
                            ConsoleDisplay.MoveDown();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {
                            //If it it's a folder allow press right key
                            FileAttributes attr = File.GetAttributes( selectedFile );

                            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                State.currentPath = selectedFile;
                                History.SetPointer();
                                ConsoleDisplay.Display();
                            }
                            else
                            {
                                AttemptOpenFile();
                            }
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        {
                            History.AddEntry();

                            //Set cursor to Parent folder
                            if (Directory.GetParent( State.currentPath ) != null)
                            {
                                SetCursorToPreviousPosition();
                                State.currentPath = Directory.GetParent( State.currentPath ).ToString();
                            }
                            ConsoleDisplay.Display();
                        }
                        break;
                }
                switch (consoleKeyInfo.KeyChar)
                {
                    case 'k':
                        if (State.cursorPosY > 0)
                        {
                            State.cursorPosY--;
                            ConsoleDisplay.MoveUp();
                        }
                        break;
                    case 'j':
                        if (State.cursorPosY < ConsoleDisplay.files.Length - 1)
                        {
                            State.cursorPosY++;
                            ConsoleDisplay.MoveDown();
                        }
                        break;
                    case 'l':
                        {
                            //If it it's a folder allow press right key
                            FileAttributes attr = File.GetAttributes( selectedFile );

                            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                State.currentPath = selectedFile;
                                History.SetPointer();
                                ConsoleDisplay.Display();
                            }
                            else
                            {
                                AttemptOpenFile();
                            }
                        }
                        break;
                    case 'h':
                        {
                            History.AddEntry();
                            //Set cursor to Parent folder
                            if (Directory.GetParent( State.currentPath ) != null)
                            {
                                SetCursorToPreviousPosition();
                                State.currentPath = Directory.GetParent( State.currentPath ).ToString();
                            }
                            ConsoleDisplay.Display();
                        }
                        break;
                    case '\t':
                        {
                            Console.Clear();
                            Process process = new Process();
                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            process.StartInfo = startInfo;
                            startInfo.FileName = "cmd";
                            startInfo.UseShellExecute = true;
                            startInfo.WorkingDirectory = State.currentPath;
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
                            AttemptOpenFile();
                        }
                        break;
                    case 'r':
                        {
                            new InputBox().RenameFile( selectedFile );
                        }
                        break;
                    case 's':
                        {
                            new SelectionRegisterScreen();
                        }
                        break;
                    case 'n':
                        {
                            new InputBox().NewFile();
                        }
                        break;
                    case 'N':
                        {
                            new InputBox().NewFolder();
                        }
                        break;
                    case 'd':
                        {
                            if (selectedFile != State.currentPath)
                                new InputBox().DeleteFileWithPrompt( selectedFile );
                        }
                        break;
                    case ' ':
                        {
                            //Tools.SelectMultiple();
                            //or
                            //ConsoleDisplay.SelectMultpiple():
                        }
                        break;
                    case 'q':
                        {
                            Console.Clear();
                            Environment.Exit( 0 );
                        }
                        break;
                    case 'b':
                        {

                            if (ConsoleDisplay.files.Length != 0)
                            {
                                State.cursorPosY = ConsoleDisplay.files.Length - 1;
                                ConsoleDisplay.Display();
                            }
                        }
                        break;
                    case 't':
                        {
                            State.cursorPosY = 0;
                            ConsoleDisplay.Display();
                        }
                        break;
                    case 'i':
                        {
                            new FileInfoScreen( selectedFile );
                        }
                        break;
                    case 'c':
                        {
                            Tools.CopySelection( selectedFile );
                        }
                        break;
                    case 'p':
                        {
                            Tools.PasteSelection();
                        }
                        break;
                    case 'z':
                        {
                            Tools.DebugWindow();
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
            parentDirectory = Directory.GetParent( State.currentPath ).ToString();
            currentFileList = CombineArrays( parentDirectory );
            for (int i = 0; i < currentFileList.Length; i++)
            {
                if (string.Compare( State.currentPath.ToLower(), currentFileList[i].ToLower() ) == 0)
                {
                    State.cursorPosY = i;
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

        private void AttemptOpenFile()
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
                Tools.DisplayError( e );
            }
        }
    }
}
