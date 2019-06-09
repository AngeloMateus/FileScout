using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileScout
{
    class SelectionRegisterScreen
    {
        public SelectionRegisterScreen()
        {
            Console.Clear();
            Console.WriteLine("\n Current Selection:\n");
            foreach (String item in Tools.selectionRegister)
            {
                Console.WriteLine( " " + item);
            }

            Console.ReadKey(true);
            ConsoleDisplay.Display();
        }
        
    }
}
