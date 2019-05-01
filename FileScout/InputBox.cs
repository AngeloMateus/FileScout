using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
            string[] commands = new string[] { "quit", "q", "cmd", "command prompt", "powershell", "pwr", "explorer", "x", "v", "version" };

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
                case "explorer":
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
                if (pattern.Length > 0 && ConsoleDisplay.files[i].ToLower().Contains( pattern.ToLower() ))
                {
                    Cursor.cursorPosY = i;
                    break;
                }
            }


            Console.Clear();
            ConsoleDisplay.Display();
        }
    }
}
