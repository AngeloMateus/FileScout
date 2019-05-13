using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace FileScout
{
    class About
    {
        public About()
        {
            string intro = "FileScout version " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            List<string> doList = new List<string>();

            doList.Add("Add TAB auto-complete for : commands");
            doList.Add("Make TAB an assignable button");
            doList.Add("Fix flickering path when scrolling long directories");
            doList.Add("Open new cmd without starting new window");
            doList.Add("Search path or files by regex");
            doList.Add("Search within directory by keypress");
            doList.Add("Hide/Show hidden files");
            doList.Add("File/Dir Selection via space");
            doList.Add("Rename/Delete/Cut/Copy");
            doList.Add("Sort files by size");
            doList.Add("Startup Arguments to start FileScout in a specific directory");
            doList.Add("Return the console window to previous position if directory has been visited");
            doList.Add("Create :help window to remind myslef of the commands");

            Console.WriteLine(new string('*', Console.WindowWidth));
            Console.SetCursorPosition((Console.WindowWidth - intro.Length) / 2, 4);
            Console.WriteLine(intro + "\n");
            Console.CursorLeft = (Console.WindowWidth - 7) / 2;
            Console.WriteLine("Author:\n");
            Console.CursorLeft = (Console.WindowWidth - 13) / 2;
            Console.WriteLine("Angelo Mateus\n");
            Console.CursorLeft = (Console.WindowWidth - 31) / 2;
            Console.WriteLine("https://github.com/angelomateus" + "\n\n\n");
            Console.WriteLine(new string('*', Console.WindowWidth));
            Console.CursorLeft = (Console.WindowWidth - 5) / 2;
            Console.WriteLine("To do:\n");

            foreach (string li in doList)
            {
                Console.CursorLeft = (Console.WindowWidth - li.Length) / 2;
                Console.WriteLine(li);
            }

            Console.CursorTop = 0;
            Console.ReadKey();
        }
    }
}
