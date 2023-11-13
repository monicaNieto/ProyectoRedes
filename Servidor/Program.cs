﻿using Servidor;
using System;

class Program
{
    private static readonly string ServerDirectory = "ServerFiles";

    static void Main()
    {
        try
        {
            Server server = new Server();
            Console.ReadKey();
        }
        catch (Exception e)
        {
            Utils.printClassError("Main", e);
        }

    }
}
