using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileScout
{
    class WindowProperties
    {
        private static bool cancellationSignal;

        public void Stop()
        {
            cancellationSignal = true;
        }
        public void Start()
        {
            while (!cancellationSignal)
            {
                if(State.windowHeight != Console.WindowHeight || State.windowWidth != Console.WindowWidth)
                {
                    ConsoleDisplay.Display();
                }
                State.windowWidth = Console.WindowWidth;
                State.windowHeight = Console.WindowHeight;

                Thread.Sleep(50);
            }
            cancellationSignal = false;
        }
    }
}
