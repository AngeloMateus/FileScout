using System.IO;
using System.Security.Permissions;

namespace FileScout
{
    class WatchFileSystem
    {
        public WatchFileSystem()
        {
            CheckFiles();
        }
        //Check for changes in the directory and redraw if changes were found
        [PermissionSet( SecurityAction.Demand, Name = "FullTrust" )]
        private static void CheckFiles()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Path = ConsoleDisplay.currentPath;
            watcher.NotifyFilter = NotifyFilters.LastAccess
                     | NotifyFilters.LastWrite
                     | NotifyFilters.FileName
                     | NotifyFilters.DirectoryName;

            watcher.Filter = "*";

            watcher.Changed += new FileSystemEventHandler(WatcherChanged);
            watcher.Created += new FileSystemEventHandler( WatcherChanged );
            watcher.Deleted += new FileSystemEventHandler( WatcherChanged );
            watcher.Renamed += new RenamedEventHandler( WatcherChanged );

            watcher.EnableRaisingEvents = true;
        }

        private static void WatcherChanged( object sender, FileSystemEventArgs e )
        {
            ConsoleDisplay.Display();
        }

    }
}
