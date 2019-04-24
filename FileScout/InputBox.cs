using System;
using System.Diagnostics;
using System.Threading;

namespace FileScout
{
    class InputBox
    {

        public InputBox()
        {
            ConsoleDisplay.ClearLine( Console.WindowTop );
            Console.SetCursorPosition( 0, Console.WindowTop );

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write( ":>" );
            Console.ResetColor();

            var command = Console.ReadLine();

            //Add all commands to an array if command isnt found display "Command Not Found"
            string[] commands = new string[] { "exit", "quit", "q", "cmd", "prompt", "command prompt", "powershell", "pwr", "explore", "explorer", "start", "about", "v", "version"};

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
                case "exit":
                case "quit":
                case "q":
                    {
                        Console.Clear();
                        Environment.Exit( 0 );
                    }
                    break;
                case "cmd":
                case "prompt":
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
                case "explorer":
                case "start":
                    {
                        Console.Clear();
                        Process.Start( @ConsoleDisplay.currentPath );
                        ConsoleDisplay.Display();
                    }
                    break;
                case "about":
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
    }
}
