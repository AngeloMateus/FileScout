using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileScout
{
    class ChildProcess
    {
        public ChildProcess( string command )
        {
            Console.Clear();
            try
            {
                Process process = new Process();
                State.isWatching = false;
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    WorkingDirectory = @State.currentPath,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
                    FileName = "cmd.exe",
                    RedirectStandardInput = false,
                    UseShellExecute = false,
                    Arguments = "/C " + command,
                };
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                Tools.DisplayError( e );
            }
            Console.Write( "\n\n(Program " );
            Console.BackgroundColor = Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(command);
            Console.ResetColor();
            Console.Write( " has exited)" );
            Console.ReadKey( true );
            State.isWatching = true;
            Console.Clear();
            ConsoleDisplay.Display();
        }
    }
}
