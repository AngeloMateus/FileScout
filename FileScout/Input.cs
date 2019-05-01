﻿using System;
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
                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.K:
                        if (Cursor.cursorPosY > 0)
                        {
                            Cursor.cursorPosY--;
                            ConsoleDisplay.MoveUp();
                        }
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.J:
                        if (Cursor.cursorPosY < ConsoleDisplay.files.Length - 1)
                        {
                            Cursor.cursorPosY++;
                            ConsoleDisplay.MoveDown();
                        }
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.L:
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
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.H:
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
                    case ConsoleKey.Tab:
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
                    case ConsoleKey.Enter:
                        {
                            new InputBox().CommadMode();
                        }
                        break;
                    case ConsoleKey.F:
                        {
                            new InputBox().SearchMode();
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


            string[] combinedDirectory = new string[directories.Length + files.Length];
            Array.Copy( directories, combinedDirectory, directories.Length );
            Array.Copy( files, 0, combinedDirectory, directories.Length, files.Length );
            return combinedDirectory;
        }
    }
}
