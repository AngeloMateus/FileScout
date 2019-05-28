using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileScout
{
    public static class Tools
    {
        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            return items != null && !items.Any();
        }

        public static string CalculateFileSize( string file )
        {
            string[] sizeSuffixes = { " b", " KB", " MB", " GB", " TB", " PB", " EB" };
            double fileSize = (double)new FileInfo( file ).Length;

            //byte=0, Kb=1, Mb=2 ...
            int magnitude = (int)Math.Log( fileSize, 1000 );

            decimal adjustedSize = (decimal)fileSize / (1L << (magnitude * 10));

            if (magnitude < 0)
            {
                magnitude = 0;
            }

            return string.Format( "{0,0}{1,-8}", Math.Round( adjustedSize, 0 ), sizeSuffixes[magnitude] );
        }

        public static string CalculateFolderSize( string file )
        {

            return "";
        }
    }
}
