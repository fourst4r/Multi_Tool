using System;
using System.Diagnostics;

namespace UserInterface
{

    internal class IOHandler
    {

        internal static readonly ConsoleColor ErrorColor      = ConsoleColor.DarkRed;
        internal static readonly ConsoleColor WarningColor    = ConsoleColor.DarkYellow;
        internal static readonly ConsoleColor UserInputColor  = ConsoleColor.DarkCyan;
        internal static readonly ConsoleColor NoteColor       = ConsoleColor.Cyan;
        internal static readonly ConsoleColor DefaultColor    = ConsoleColor.Cyan;
        private  static readonly ConsoleColor BackgroundColor = ConsoleColor.Black;

        private static ConsoleColor _previousColor;


        static IOHandler()
        {
            SetDefaultSettings();
        }


        private static void SetDefaultSettings()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = DefaultColor;

            Console.Clear();

            _previousColor = DefaultColor;
        }


        protected static void WriteLine(string s = "") => Write(s + Environment.NewLine);

        protected static void WriteLine(string s, ConsoleColor c) =>  Write(s + Environment.NewLine, c);

        protected static void Write(string s, ConsoleColor c)
        {
            _previousColor = Console.ForegroundColor;

            if (c != BackgroundColor)
                Console.ForegroundColor = c;

            Write(s);

            if(c != UserInputColor)
                Console.ForegroundColor = _previousColor;
        }

        protected static void Write(string s)
        {
            Console.Write(s);
            Console.Out.Flush();
        }

        protected static string ReadInput(bool trim = true)
        {
            string input = Console.ReadLine();
            WriteLine();
            Console.ForegroundColor = _previousColor;

            if (input == null)
                return string.Empty;

            return trim ?  input.Trim() : input;
        }

        protected static void ShowExceptionToUser(Exception ex)
        {
            WriteLine();
            WriteLine(Environment.NewLine + "\tType: " + ex.GetType().Name, ErrorColor);
            WriteLine(Environment.NewLine + "\tError: " + ex.Message, ErrorColor);

            if (Debugger.IsAttached)
            {
                var trace = new StackTrace(ex).ToString().Replace("   ", String.Empty).Replace("\r\n", Environment.NewLine + "\t", StringComparison.InvariantCultureIgnoreCase);
                WriteLine(Environment.NewLine + "\tStacktrace:" + Environment.NewLine + "\t" + trace, ErrorColor);
            }
        }

    }
}
