using System;

namespace FileScout
{
    class SelectionRegisterScreen
    {
        public SelectionRegisterScreen()
        {
            Console.Clear();
            Console.WriteLine("\n  Current Selection:\n");
            Console.WriteLine("  (c)lear list\n");
            foreach (String item in Tools.selectionRegister)
            {
                Console.WriteLine(" " + item);
            }

            ConsoleKeyInfo cki = Console.ReadKey(true);
            if (cki.Key == ConsoleKey.C)
            {
                Tools.selectionRegister.Clear();
                Console.Clear();
                Console.WriteLine("\n  Current Selection:\n");
                Console.WriteLine("  (c)lear list\n");
                Console.ReadKey(true);
            }
            ConsoleDisplay.Display();
        }

    }
}
