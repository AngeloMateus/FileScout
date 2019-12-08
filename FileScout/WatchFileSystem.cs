using System.IO;
using System.Security.Permissions;
using System.Threading;
using System;

namespace FileScout
{
    class WatchFileSystem
    {
        private static FileSystemWatcher watcher;
        //Check for changes in the directory and redraw if changes were found
        [PermissionSet( SecurityAction.Demand, Name = "FullTrust" )]
        public void CheckFiles()
        {
            watcher = new FileSystemWatcher();

            watcher.Path = State.currentPath;
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.Size;

            watcher.Filter = "*";

            watcher.Changed += new FileSystemEventHandler( WatcherChanged );
            watcher.Renamed += new RenamedEventHandler( WatcherChanged );
            watcher.Created += new FileSystemEventHandler( WatcherCreatedFile );
            watcher.Deleted += new FileSystemEventHandler( WatcherDeletedFile );

            watcher.EnableRaisingEvents = true;

        }

        public static void RefreshWatcherPath()
        {
            watcher.Path = State.currentPath;
        }

        private void WatcherChanged( object sender, FileSystemEventArgs e )
        {
            if (State.isWatching)
            {
                ConsoleDisplay.Display();
            }

        }
        private void WatcherCreatedFile( object sender, FileSystemEventArgs e )
        {
            if (State.isWatching)
            {
                ConsoleDisplay.files = ConsoleDisplay.CombineArrays( State.currentPath );
                State.cursorPosY = Array.IndexOf( ConsoleDisplay.files, e.FullPath );
                State.selectedFile = e.FullPath;
                ConsoleDisplay.Display();
            }
        }
        private void WatcherDeletedFile( object sender, FileSystemEventArgs e )
        {
            if (State.isWatching)
            {
                State.selectedFile = null;
                ConsoleDisplay.Display();
            }
        }

    }
}
