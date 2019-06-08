using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileScout
{
    public static class Tools
    {
        public static bool IsEmpty<T>( this IEnumerable<T> items )
        {
            return items != null && !items.Any();
        }

        public static string DisplayFileSize( string file )
        {
            string[] sizeSuffixes = { " b", " KB", " MB", " GB", " TB", " PB", " EB" };
            long fileSize = (long)new FileInfo( file ).Length;

            //byte=0, Kb=1, Mb=2 ...
            int magnitude = (int)Math.Log( fileSize, 1024 );

            decimal adjustedSize = (decimal)fileSize / (1L << (magnitude * 10));

            if (magnitude < 0)
            {
                magnitude = 0;
            }

            return string.Format( "{0,0}{1,-8}", Math.Round( adjustedSize, 1 ), sizeSuffixes[magnitude] );
        }

        private static long CalculateFileSize( DirectoryInfo dir )
        {

            long sizeSum = dir.EnumerateFiles().Sum( file => file.Length );

            return sizeSum;
        }

        private static long CalculateFolderSize( DirectoryInfo dir )
        {
            long totalSize = 0;
            try
            {
                IEnumerable<DirectoryInfo> dirInfos = dir.EnumerateDirectories();


                if (IsEmpty( dirInfos ))
                {
                    totalSize += CalculateFileSize( dir );
                }
                else
                {
                    foreach (DirectoryInfo info in dirInfos)
                    {
                        totalSize += CalculateFolderSize( info );
                    }
                    totalSize += CalculateFileSize( dir );
                }
            }
            catch (UnauthorizedAccessException ua)
            {

            }

            return totalSize;
        }

        //Look at files in child Directory and add filesizes
        public static string DisplayFolderSize( string folder )
        {
            string[] sizeSuffixes = { " b", " KB", " MB", " GB", " TB", " PB", " EB" };
            DirectoryInfo dirInfo = new DirectoryInfo( folder );
            long totalSize = 0;

            totalSize = CalculateFolderSize( dirInfo );


            //byte=0, Kb=1, Mb=2 ...
            int magnitude = (int)Math.Log( totalSize, 1024 );

            decimal adjustedSize = (decimal)totalSize / (1L << (magnitude * 10));

            if (magnitude < 0)
            {
                magnitude = 0;
            }

            return string.Format( "{0,0}{1,-8}", Math.Round( adjustedSize, 1 ), sizeSuffixes[magnitude] );
        }
    }
}
