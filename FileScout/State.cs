﻿using System;
using System.Collections.Generic;

namespace FileScout
{
    static class State
    {
        public static int cursorPosY;
        public static string currentPath;

        public static char currentFindKey;

        public static string selectedFile;

        public static bool isWatching = true;

        public static int currentFindKeyPosition;
        public static List<int> findKeyMatches;

        public static int activeScreen;

        public static int windowWidth;
        public static int windowHeight;

        public static int currentFileChunk;
        public enum screens { FILESYSTEM, ABOUT, INFO };
    }
}