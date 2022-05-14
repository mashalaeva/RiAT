using System;
using System.Collections.Generic;
using Services;

namespace RiAT
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleDialogue consoleDialogue = new ConsoleDialogue();
            consoleDialogue.StartDialog();
        }
    }
}