using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileScout
{
    class About
    {
        public About()
        {
            string intro = "FileScout version " + FileVersionInfo.GetVersionInfo( Assembly.GetExecutingAssembly().Location ).ProductVersion;
            List<string> todo = new List<string>();

            todo.Add( "Format Current path to display a maximum length" );
            todo.Add( "Make TAB an assignable button" );
            todo.Add( "Fix flickering path when scrolling long directories" );
            todo.Add( "Open new cmd without starting new window" );
            todo.Add( "Search path or files by regex" );
            todo.Add( "Search within directory by keypress" );
            todo.Add( "Hide/Show hidden files" );
            todo.Add( "Rename/Delete/Cut/Copy" );
            todo.Add( "Sort files by size" );

            Console.SetCursorPosition( (Console.WindowWidth - intro.Length) / 2, 4 );
            Console.WriteLine( intro + "\n" );
            Console.CursorLeft = (Console.WindowWidth - 7) / 2;
            Console.WriteLine( "Author:\n" );
            Console.CursorLeft = (Console.WindowWidth - 13) / 2;
            Console.WriteLine( "Angelo Mateus\n" );
            Console.CursorLeft = (Console.WindowWidth - 31) / 2;
            Console.WriteLine( "https://github.com/angelomateus" + "\n\n\n" );
            Console.CursorLeft = (Console.WindowWidth - 5) / 2;
            Console.WriteLine( "To do:\n" );

            foreach (string li in todo)
            {
                Console.CursorLeft = (Console.WindowWidth - li.Length) / 2;
                Console.WriteLine( li );
            }

            Console.CursorTop = 0;
            Console.ReadKey();
        }
    }
}
