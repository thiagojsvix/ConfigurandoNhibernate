using System;

namespace Dominio
{
    public static class ConsoleWrite
    {
        public static void Color(string value, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(value);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
        }
    }
}
