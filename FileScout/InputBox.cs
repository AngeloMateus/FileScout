using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace FileScout
{
    class InputBox
    {
        public void CommadMode()
        {
            Console.CursorVisible = true;
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.SetCursorPosition( 0, Console.WindowTop );

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write( ":>" );
            Console.ResetColor();

            string command = Console.ReadLine();

            //Add all commands to an array if command isnt found display "Command Not Found"
            string[] commands = new string[] { "q", "c", "p", "x", "v", "preview" };

            switch (command.ToLower())
            {
                case "":
                    break;
                case "q":
                    {
                        Console.Clear();
                        Environment.Exit( 0 );
                    }
                    break;
                case "c":
                    {
                        Console.Clear();
                        Process process = new Process();
                        process.StartInfo.FileName = "cmd";
                        process.StartInfo.UseShellExecute = true;
                        process.StartInfo.WorkingDirectory = State.currentPath;
                        process.Start();
                        ConsoleDisplay.Display();
                    }
                    break;
                case "p":
                    {
                        Console.Clear();
                        Process process = new Process();
                        process.StartInfo.FileName = "powershell";
                        process.StartInfo.UseShellExecute = true;
                        process.StartInfo.WorkingDirectory = State.currentPath;
                        process.Start();
                        ConsoleDisplay.Display();
                    }
                    break;
                case "x":
                    {
                        Console.Clear();
                        Process.Start( @State.currentPath );
                        ConsoleDisplay.Display();
                    }
                    break;
                case "v":
                    {
                        Console.Clear();
                        new About();
                        ConsoleDisplay.Display();
                    }
                    break;
                case "m":
                    {
                        new FilePreview();
                    }
                    break;
                default:
                    {
                        try
                        {
                            Console.SetBufferSize( Console.WindowWidth, 999);
                            new ChildProcess( command );
                        }
                        catch (Exception e)
                        {
                            Tools.DisplayError( e );
                        }
                    }
                    break;
            }
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.CursorVisible = false;
        }

        public void SearchMode()
        {
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.SetCursorPosition( 0, Console.WindowTop );

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write( ":/" );
            Console.ResetColor();

            string pattern = Console.ReadLine();


            for (int i = 0; i < ConsoleDisplay.files.Length; i++)
            {
                if (Path.GetFileName( ConsoleDisplay.files[i] ).ToLower().Contains( pattern.ToLower() ))
                {
                    State.cursorPosY = i;
                    break;
                }
                else if (i == ConsoleDisplay.files.Length - 1)
                    Tools.DisplayError( new Exception( "Search returned no results" ) );
            }


            Console.Clear();
            ConsoleDisplay.Display();
        }

        public void JumpToLetter()
        {
            try
            {
                ConsoleDisplay.ClearLine( Console.WindowTop );
                Console.SetCursorPosition( 0, Console.WindowTop );

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write( ":." );
                Console.ResetColor();

                ConsoleKeyInfo keyInfo = Console.ReadKey( true );
                char key = keyInfo.KeyChar;

                if (key != State.currentFindKey)
                {
                    State.findKeyMatches.Clear();
                }

                bool hasKey;
                //iterate through directory and find files that start with key
                for (int i = 0; i < ConsoleDisplay.files.Length; i++)
                {
                    hasKey = Path.GetFileName( ConsoleDisplay.files[i] ).StartsWith( key.ToString(), StringComparison.InvariantCultureIgnoreCase );

                    if (hasKey)
                    {
                        State.findKeyMatches.Add( i );
                    }
                }

                State.cursorPosY = State.findKeyMatches[State.currentFindKeyPosition];
                if (State.currentFindKeyPosition + 1 < State.findKeyMatches.Count)
                {
                    State.currentFindKeyPosition++;
                }
                else
                {
                    State.currentFindKeyPosition = 0;
                }
                Console.Clear();
                ConsoleDisplay.Display();
            }
            catch (Exception e)
            {
                Tools.DisplayError( new Exception( "Search returned no results" ) );
            }
        }

        //renames 'file' in the argument list to 'line'
        public void RenameFile()
        {
            string file = State.selectedFile;
            string oldFile = file;
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.SetCursorPosition( 0, Console.WindowTop );
            Console.Write( "RENAME to: " );
            string line = Console.ReadLine();
            try
            {
                //Check if the file is a directory or not for File.Move() or Directory.Move()
                FileAttributes attr = File.GetAttributes( file );
                if ((attr & FileAttributes.Directory) != FileAttributes.Directory && line != String.Empty)
                {
                    File.Move( file, State.currentPath + Path.DirectorySeparatorChar + line );
                }
                else if ((attr & FileAttributes.Directory) == FileAttributes.Directory && line != String.Empty)
                {
                    Directory.Move( file, State.currentPath + Path.DirectorySeparatorChar + line );
                }
                else
                {
                    ConsoleDisplay.Display();
                }
            }
            catch (Exception e)
            {
                Tools.DisplayError( e );
            }

            while (!oldFile.Equals( file ))
            {
                Thread.Sleep( 50 );
            }
        }
        public void NewFolder()
        {
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.SetCursorPosition( 0, Console.WindowTop );
            Console.Write( "NEW folder: " );
            string file = Console.ReadLine();

            try
            {
                if (file != String.Empty && !Directory.Exists( State.currentPath + Path.DirectorySeparatorChar + file ))
                {
                    Directory.CreateDirectory( State.currentPath + Path.DirectorySeparatorChar + file );
                }
                else if (Directory.Exists( State.currentPath + Path.DirectorySeparatorChar + file ))
                {
                    ConsoleDisplay.ClearLine( Console.WindowTop );
                    Console.SetCursorPosition( 0, Console.WindowTop );
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write( "(!) Directory " + file + " already exists!" );
                    Console.ResetColor();
                    Console.ReadKey( true );
                    ConsoleDisplay.ClearLine( Console.WindowTop );
                    ConsoleDisplay.ClearLine( Console.WindowTop + 1 );
                }
                else
                {
                    ConsoleDisplay.Display();
                }
            }
            catch (Exception e)
            {
                Tools.DisplayError( e );
            }
        }

        public void NewFile()
        {
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.SetCursorPosition( 0, Console.WindowTop );
            Console.Write( "NEW file: " );
            string file = Console.ReadLine();
            file = State.currentPath + Path.DirectorySeparatorChar + file;

            try
            {
                if (file != String.Empty && !File.Exists( file ))
                {
                    FileStream fs = File.Create( file );
                    fs.Close();
                }
                else if (File.Exists( State.currentPath + Path.DirectorySeparatorChar + file ))
                {
                    ConsoleDisplay.ClearLine( Console.WindowTop );
                    Console.SetCursorPosition( 0, Console.WindowTop );
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write( "(!) File " + file + " already exists!" );
                    Console.ResetColor();
                    Console.ReadKey( true );
                    ConsoleDisplay.ClearLine( Console.WindowTop );
                    ConsoleDisplay.ClearLine( Console.WindowTop + 1 );
                }
                else
                {
                    ConsoleDisplay.Display();
                }
            }
            catch (Exception e)
            {
                Tools.DisplayError( e );
            }
        }

        public void DeleteFile( String file )
        {
            try
            {
                FileAttributes attr = File.GetAttributes( file );

                if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    File.Delete( file );
                    Console.WriteLine( "Deleted: " + file );
                }
                else if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    DeleteFolder( file );
                }

                else
                {
                    ConsoleDisplay.Display();
                }
            }
            catch (Exception e)
            {
                Tools.DisplayError( e );
            }
        }

        public void DeleteFileWithPrompt( String file )
        {
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.SetCursorPosition( 0, Console.WindowTop );
            Console.Write( "DELETE " );
            Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write( Path.GetFileName( file ) );
            Console.ResetColor();
            Console.Write( " (y?)" );
            ConsoleKeyInfo cki = Console.ReadKey( true );

            try
            {
                FileAttributes attr = File.GetAttributes( file );

                if (cki.KeyChar == 'y')
                {
                    if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                    {
                        State.selectedFile = null;
                        File.Delete( file );
                        Console.WriteLine( "Deleted: " + file );

                        if (State.cursorPosY > 0)
                            State.cursorPosY = State.cursorPosY - 1;
                    }
                    else if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        DeleteFolderWithPrompt( file );
                    }
                }
                else
                {
                    ConsoleDisplay.Display();
                }
            }
            catch (Exception e)
            {
                Tools.DisplayError( e );
            }
        }

        private void DeleteFolder( String folder )
        {
            IEnumerable<String> children = Directory.EnumerateFileSystemEntries( folder );
            if (Tools.IsEmpty( children ))
            {
                Directory.Delete( folder );
                if (State.cursorPosY > 0)
                    State.cursorPosY = State.cursorPosY - 1;
                if (State.cursorPosY == 0)
                    State.selectedFile = State.currentPath;
            }
            else
            {
                foreach (String child in children)
                {
                    DeleteFile( child );
                }
                Directory.Delete( folder );
                State.cursorPosY = 0;
            }
        }

        private void DeleteFolderWithPrompt( String folder )
        {
            IEnumerable<String> children = Directory.EnumerateFileSystemEntries( folder );
            if (Tools.IsEmpty( children ))
            {
                Directory.Delete( folder );
                if (State.cursorPosY > 0)
                    State.cursorPosY = State.cursorPosY - 1;
                if (State.cursorPosY == 0)
                    State.selectedFile = State.currentPath;
            }
            else
            {
                ConsoleDisplay.ClearLine( Console.WindowTop );
                Console.SetCursorPosition( 0, Console.WindowTop );
                Console.Write( "Folder " );
                Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write( Path.GetFileName( folder ) );
                Console.ResetColor();
                Console.Write( " is not empty! Delete folder and its contents? (y?) " );
                ConsoleKeyInfo cki = Console.ReadKey( true );

                if (cki.KeyChar == 'y')
                {
                    Console.Clear();
                    foreach (String child in children)
                    {
                        DeleteFile( child );
                    }
                    Directory.Delete( folder );
                    Console.WriteLine( " Deleted: " + folder );
                    State.cursorPosY = 0;
                }
                else
                {
                    ConsoleDisplay.Display();
                }
            }
        }
    }
}
