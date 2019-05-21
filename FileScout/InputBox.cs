using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace FileScout
{
    class InputBox
    {

        public void CommadMode()
        {
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.SetCursorPosition( 0, Console.WindowTop );

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write( ":>" );
            Console.ResetColor();

            var command = Console.ReadLine();

            //Add all commands to an array if command isnt found display "Command Not Found"
            string[] commands = new string[] { "quit", "q", "cmd", "command prompt", "powershell", "pwr", "explore", "x", "v", "version" };

            bool commandExists = true;

            for (int i = 0; i < commands.Length; i++)
            {
                if (command.ToLower().Equals( commands[i].ToLower() ))
                {
                    commandExists = true;
                    break;
                }
                else
                {
                    commandExists = false;
                }
            }

            if (!commandExists && command != String.Empty)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition( 0, Console.WindowTop );
                Console.WriteLine( "Command not found." );
                Console.ResetColor();
                Thread.Sleep( 260 );
            }

            switch (command.ToLower())
            {
                case "":
                    break;
                case "quit":
                case "q":
                    {
                        Console.Clear();
                        Environment.Exit( 0 );
                    }
                    break;
                case "cmd":
                case "command prompt":
                    {
                        Console.Clear();
                        Process process = new Process();
                        process.StartInfo.FileName = "cmd";
                        process.StartInfo.UseShellExecute = true;
                        process.StartInfo.WorkingDirectory = ConsoleDisplay.currentPath;
                        process.Start();
                        ConsoleDisplay.Display();
                    }
                    break;
                case "powershell":
                case "pwr":
                    {
                        Console.Clear();
                        Process process = new Process();
                        process.StartInfo.FileName = "powershell";
                        process.StartInfo.UseShellExecute = true;
                        process.StartInfo.WorkingDirectory = ConsoleDisplay.currentPath;
                        process.Start();
                        ConsoleDisplay.Display();
                    }
                    break;
                case "explore":
                case "x":
                    {
                        Console.Clear();
                        Process.Start( @ConsoleDisplay.currentPath );
                        ConsoleDisplay.Display();
                    }
                    break;
                case "v":
                case "version":
                    {
                        Console.Clear();
                        new About();
                        ConsoleDisplay.Display();
                    }
                    break;
                    
            }
            ConsoleDisplay.ClearLine( Console.WindowTop );
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
                if (Path.GetFileName(ConsoleDisplay.files[i]).ToLower().Contains( pattern.ToLower() ))
                {
                    Cursor.cursorPosY = i;
                    break;
                }
            }


            Console.Clear();
            ConsoleDisplay.Display();
        }

        public void JumpToLetter()
        {
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.SetCursorPosition( 0, Console.WindowTop );

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write( ":." );
            Console.ResetColor();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            string key = keyInfo.KeyChar.ToString();

            for (int i = 0; i < ConsoleDisplay.files.Length; i++)
            {
                if (Path.GetFileName( ConsoleDisplay.files[i]).StartsWith( key, StringComparison.InvariantCultureIgnoreCase))
                {
                    Cursor.cursorPosY = i;
                    break;
                }
            }
            Console.Clear();
            ConsoleDisplay.Display();
        }

        public void RenameFile(string file)
        {
            string oldFile = file;
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.SetCursorPosition( 0, Console.WindowTop );
            Console.Write("RENAME to: ");
            string line = Console.ReadLine();
            try
            {
                //Check if the file is a directory or not for File.Move() or Directory.Move()
                FileAttributes attr = File.GetAttributes( file );
                if ((attr & FileAttributes.Directory) != FileAttributes.Directory && line != String.Empty)
                {
                    File.Move( file, ConsoleDisplay.currentPath + Path.DirectorySeparatorChar + line );
                }
                else if ((attr & FileAttributes.Directory) == FileAttributes.Directory && line != String.Empty)
                {
                    Directory.Move( file, ConsoleDisplay.currentPath + Path.DirectorySeparatorChar + line );
                }
                else
                {
                    ConsoleDisplay.Display();
                }
            }
            catch (Exception e)
            {
                ConsoleDisplay.ClearLine( Console.WindowTop );
                Console.SetCursorPosition( 0, Console.WindowTop );
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("(!) " + e.Message );
                Console.ResetColor();
                Console.ReadKey(true);
                ConsoleDisplay.ClearLine( Console.WindowTop );
            }
            
            while (!oldFile.Equals( file ) )
            {
                Thread.Sleep( 50 );
            }
        }
    }
}
