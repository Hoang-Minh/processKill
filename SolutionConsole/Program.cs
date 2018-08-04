using System;
using Solution;
using System.Diagnostics;

namespace SolutionConsole
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World! = " + (args != null ? args[0] : "NO args"));
            Solution.ProcessUtils.Main(args);
        }
    }
}
