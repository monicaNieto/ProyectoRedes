using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
    internal class Logger
    {
        public static void writeLog(string[] inputData)
        {
            string fileName = "log_server.txt";

            // Datos que se escribirán en el archivo.
            string[] lines = inputData;

            // Tamaño del búfer (en bytes)
            int bufferSize = 1024;

            try
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Append))
                using (BufferedStream bufferedStream = new BufferedStream(fileStream, bufferSize))
                using (StreamWriter writer = new StreamWriter(bufferedStream))
                {
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }

                    writer.Close();
                }

                //Console.WriteLine("Se guardó el archivo con exito!.");
            }
            catch (IOException e)
            {
                Console.WriteLine("Error al escribir en el archivo: " + e.Message);
            }
        }
    }
}
