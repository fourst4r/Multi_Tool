using System;
using UserInterface.DataStructures.Constants;
using UserInterface.Handlers.FileHandlers;
using UserInterface.Menu;

namespace UserInterface
{
    // dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "PR2 Multi Tool";

            new MainMenu().Start();
        }

    }
}
