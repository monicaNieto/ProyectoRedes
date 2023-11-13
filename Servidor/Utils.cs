using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
    internal class Utils
    {
        public static void printSeparator()
        {
            Console.WriteLine("----------------------------------------------------------------------");
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

        public static void printClassError(string className)
        {
            printSeparator();
            Console.WriteLine("Class error: {0}", className);
            printSeparator();
        }
    }

}
