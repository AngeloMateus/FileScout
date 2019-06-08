using System;
using System.IO;

namespace FileScout
{
    class ConsoleDisplayChild
    {
        string[] childFiles;

        public ConsoleDisplayChild()
        {
            int originalTop = Console.WindowTop;
            for (int i = 0; i < Console.WindowHeight - 5; i++)
            {
                Console.SetCursorPosition( 60, Console.WindowTop + 5 + i );
                Console.Write( new string( ' ', Console.WindowWidth - 64 ) );

            }

            try
            {
                FileAttributes attr = File.GetAttributes( ConsoleDisplay.selectedFile );

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    childFiles = ConsoleDisplay.CombineArrays( ConsoleDisplay.selectedFile );
                    for (int i = 0; i < childFiles.Length; i++)
                    {
                        Console.SetCursorPosition( 60, Console.WindowTop + 5 + i );
                        Console.Write( "|     " );
                        if (i + 7 >= Console.WindowHeight)
                        {
                            Console.SetCursorPosition( 66, Console.WindowTop + 5 + i );

                            FileAttributes attrChild = File.GetAttributes( childFiles[i] );
                            if ((attrChild & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                Console.Write( ConsoleDisplay.ShortenFileName( "> " + Path.GetFileName( childFiles[i] ), 26 ) + Path.DirectorySeparatorChar );
                            }
                            else
                            {
                                Console.Write( ConsoleDisplay.ShortenFileName( Path.GetFileName( childFiles[i] ), 26 ) );

                            }
                            Console.SetCursorPosition( 66, Console.WindowTop + 6 + i );
                            Console.Write( "..." );
                            break;
                        }
                        else
                        {
                            Console.SetCursorPosition( 66, Console.WindowTop + 5 + i );

                            FileAttributes attrChild = File.GetAttributes( childFiles[i] );
                            if ((attrChild & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                Console.Write( ConsoleDisplay.ShortenFileName( "> " + Path.GetFileName( childFiles[i] ), 26 ) + Path.DirectorySeparatorChar );
                            }
                            else
                            {
                                Console.Write( ConsoleDisplay.ShortenFileName( Path.GetFileName( childFiles[i] ), 26 ) );
                            }
                        }
                    }
                    if (childFiles.Length == 0)
                    {
                        Console.SetCursorPosition( 66, Console.WindowTop + 5 );
                        Console.Write( "(empty)" );
                    }
                }


            }
            catch (UnauthorizedAccessException)
            {
                Console.SetCursorPosition( 66, Console.WindowTop + 5 );
                Console.Write( "(Unauthorized access)" );
            }
            catch (FileNotFoundException e)
            {
                ConsoleDisplay.ClearLine( Console.WindowTop );
                Console.SetCursorPosition( 0, Console.WindowTop );
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write( "(!) " + e.Message );
                Console.ResetColor();
                Console.ReadKey( true );
                ConsoleDisplay.ClearLine( Console.WindowTop );
                ConsoleDisplay.ClearLine( Console.WindowTop + 1 );
            }
            Console.SetWindowPosition( 0, originalTop );

        }
    }
}
