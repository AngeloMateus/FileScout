using System.Collections.Generic;

namespace FileScout
{
    static class State
    {
        public static int cursorPosY;
        public static string currentPath;

        public static bool isWatching = true;

        public static char currentFindKey;
        public static int currentFindKeyPosition;
        public static List<int> findKeyMatches;
    }
}