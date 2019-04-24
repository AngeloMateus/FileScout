using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileScout
{
    class About
    {
        public About()
        {
            string intro = "FileScout version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            List<string> todo = new List<string>();

            todo.Add( "Format file sizes with Kb Mb Gb Tb" );
            todo.Add( "Format Current path to display a maximum length" );
            todo.Add( "Always display current path on top of window" );
            todo.Add( "Open new cmd without starting new window" );
            todo.Add( "Search path or files by regex" );
            todo.Add( "Search within directory by keypress" );
            todo.Add( "Hide/Show hidden files" );

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
                Console.WriteLine( li  );
            }

            Console.CursorTop = 0;
            Console.ReadKey();
        }
    }
}
