using System;
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

            if (!historyDictionary.ContainsKey( ConsoleDisplay.currentPath ))
            {
                historyDictionary.Add( ConsoleDisplay.currentPath, Cursor.cursorPosY );
            }
            else
            {
                historyDictionary.Remove(ConsoleDisplay.currentPath);
                historyDictionary.Add( ConsoleDisplay.currentPath, Cursor.cursorPosY );
            }
        }

        public static void SetPointer()
        {
            historyDictionary.TryGetValue(ConsoleDisplay.currentPath, out Cursor.cursorPosY);
        }

    }
}
