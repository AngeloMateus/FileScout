using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileScout
{
    class Program
    {
        static void Main( string[] args )
        {
            new WatchFileSystem();
            DisplayFiles.Display();

            ReadInput readInput = new ReadInput();

            Thread readInputThread = new Thread( new ThreadStart( readInput.StartReading ) );
            readInputThread.Start();
        }
    }
}
