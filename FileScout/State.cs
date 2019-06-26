using System.Collections.Generic;

namespace FileScout
{
    static class State
    {
        public static int cursorPosY;
        public static string currentPath;

        public static char currentFindKey;

        public static bool isWatching = true;

        public static int currentFindKeyPosition;
        public static List<int> findKeyMatches;

        public static int activeScreen;

        public enum screens {FILESYSTEM, ABOUT, INFO};
    }
}