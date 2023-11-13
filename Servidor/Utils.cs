using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Servidor
{
    internal class Utils
    {
        static string separator = "---------------------------------------------------------------------------------------";
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
            //string[] log = new[] { "Files: " };

            foreach (string f in files)
            {
                Console.WriteLine(f); // Mostramos los archivos en la consola
                //log.Append(f);
            }
            printSeparator();
            //Logger.writeLog(log);
        }

        public static void printClassError(string className, Exception error)
        {
            string classError = className;
            string errorMessage = "Error: " + error.Message.ToString();
            Console.WriteLine("");
            printSeparator();
            Console.WriteLine(classError);
            Console.WriteLine(errorMessage);
            printSeparator();
            Console.WriteLine("");
            string[] log = new[] {separator, classError, errorMessage, separator};
            Logger.writeLog(log);
            Console.ReadKey();
        }
    }

}
