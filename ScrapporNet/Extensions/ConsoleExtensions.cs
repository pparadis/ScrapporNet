using System;

namespace ScrapporNet.Extensions
{
    public static class Console
    {
        public static void Pause()
        {
            System.Console.WriteLine("Appuyez sur une touche pour continuer.");
            System.Console.ReadLine();
        }

    }
}
