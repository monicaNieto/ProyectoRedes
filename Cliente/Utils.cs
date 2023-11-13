using System;
using System.IO;

namespace Servidor
{
    internal class Utils
    {
        public static void printSeparator()
        {
            Console.WriteLine("---------------------------------------------------------------------------------------");
        }

        public static void printFileAndList(string fileName, string directory)
        {
            printSeparator();
            Console.WriteLine($"Archivo recibido y guardado: {fileName}");
            printSeparator();

            string[] files = Directory.GetFiles(@directory);

            foreach (string f in files)
            {
                Console.WriteLine(f); // Mostramos los archivos en la consola
            }
            printSeparator();
        }

        public static void printClassError(string className, Exception error)
        {
            Console.WriteLine("");
            printSeparator();
            Console.WriteLine("Class error: {0}", className);
            Console.WriteLine("");
            Console.WriteLine("Error: {0}", error.Message);
            Console.WriteLine("");
            Console.ReadKey();
        }

        public static void printMessage(string message)
        {
            printSeparator();
            Console.WriteLine(message);
            printSeparator();
        }
    }

}
