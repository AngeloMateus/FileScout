using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

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



            watcher.Path = DisplayFiles.currentPath;
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
            DisplayFiles.Display();
        }

    }
}
