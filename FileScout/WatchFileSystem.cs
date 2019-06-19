using System.IO;
using System.Security.Permissions;

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
                     | NotifyFilters.DirectoryName;

            watcher.Filter = "*";

            watcher.Changed += new FileSystemEventHandler( WatcherChanged );
            watcher.Created += new FileSystemEventHandler( WatcherChanged );
            watcher.Deleted += new FileSystemEventHandler( WatcherChanged );
            watcher.Renamed += new RenamedEventHandler( WatcherChanged );

            watcher.EnableRaisingEvents = true;

        }

        public static void RefreshWatcherPath()
        {
            watcher.Path = State.currentPath;
        }

        private void WatcherChanged( object sender, FileSystemEventArgs e )
        {
            if (State.isWatching)
                ConsoleDisplay.Display();
        }

    }
}
