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
            State.activeScreen = (int)State.screens.ABOUT;
            //build | year dayOfYear | timeStamp
            string intro = "FileScout version " + FileVersionInfo.GetVersionInfo( Assembly.GetExecutingAssembly().Location ).ProductVersion;
            List<string> doList = new List<string>();

            doList.Add( "On rename/newfile/newfolder change cursor position to the file to alter and rename from there" );
            doList.Add( "Add TAB auto-complete for : commands" );
            doList.Add( "Search child directories WITH REGEX!" );
            doList.Add( "Optimize scrolling through long directories (maybe by not iterating through array with each setCursorPosition)" );
            doList.Add( "File/Dir Selection via space" );
            doList.Add( "Single file Cut/Copy" );
            doList.Add( "Multiple Selection Delete/Cut/Copy" );
            doList.Add( "Option to Sort files by size" );
            doList.Add( "Create :help window to remind myself of the commands" );
            doList.Add( "Folder Marks: ' to view all marks in `Marks` screen; m to mark a location with prompt to name location; ' + d to delete mark" );
            doList.Add( "Color coding for Folder Marks" );
            doList.Add( "Make folder deletion create a new top level thread" );
            doList.Add( "\n" );
            doList.Add( "TO FIX:\n" );
            doList.Add( "Fix flickering path when scrolling long directories" );
            doList.Add( "Fix treversing directories - UnauthorizedAccessException - when pressing i" );
            doList.Add( "Fix `i` File Size not displaying correct Size on large files" );
            doList.Add( "Creating new file or folder needs to set cursor to position of new file or folder" );

            Console.WriteLine( new string( '*', Console.WindowWidth ) );
            Console.SetCursorPosition( (Console.WindowWidth - intro.Length) / 2, Console.WindowHeight / 2 - 6 );
            Console.WriteLine( intro + "\n" );
            Console.CursorLeft = (Console.WindowWidth - 7) / 2;
            Console.WriteLine( "Author:\n" );
            Console.CursorLeft = (Console.WindowWidth - 13) / 2;
            Console.WriteLine( "Angelo Mateus\n" );
            Console.CursorLeft = (Console.WindowWidth - 31) / 2;
            Console.WriteLine( "https://github.com/angelomateus" + "\n\n\n" );
            Console.SetCursorPosition( 0, Console.WindowHeight - 1 );
            Console.WriteLine( new string( '*', Console.WindowWidth ) );
            Console.CursorLeft = (Console.WindowWidth - 5) / 2;
            Console.WriteLine( "To do:\n" );

            foreach (string li in doList)
            {
                try
                {
                    Console.CursorLeft = (Console.WindowWidth - li.Length) / 2;
                    Console.WriteLine( li );
                }
                catch (Exception e)
                {
                    Console.WriteLine( li );
                }
            }

            Console.CursorTop = 0;
            Console.ReadKey();
        }
    }
}
