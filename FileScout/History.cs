﻿using System;
using System.Collections.Generic;

namespace FileScout
{
    static class History
    {
        private static SortedDictionary<string, int> historyDictionary;

        static History()
        {
            historyDictionary = new SortedDictionary<string, int>();
        }

        public static void PrintHistory()
        {
            foreach (KeyValuePair<string, int> kvp in historyDictionary)
            {
                Console.WriteLine( "   " +  kvp.Key.Substring( Math.Max( 0, kvp.Key.Length - 10 ) ) + " "+kvp.Value );
            }
        }

        public static void AddEntry()
        {

            if (!historyDictionary.ContainsKey( State.currentPath ))
            {
                historyDictionary.Add( State.currentPath, State.cursorPosY );
            }
            else
            {
                historyDictionary.Remove( State.currentPath);
                historyDictionary.Add( State.currentPath, State.cursorPosY );
            }
        }

        public static void SetPointer()
        {
            historyDictionary.TryGetValue( State.currentPath, out State.cursorPosY);
        }

    }
}
