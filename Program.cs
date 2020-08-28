using System;

namespace lang
{
    class Program
    {
        static void Main(string[] args)
        {
            //Usage: pointless -f [script]

            
            string version = "0.0.0.1";
            Console.WriteLine("Pointless v_{0}", version);            
            if(args.Length < 1) {
                Pointless.runREPL();
            } else if (args.Length == 2 && args[0] == "-g") {
                Console.WriteLine("Generating Base Classes at Location: " + args[1]);
                Pointless.GenerateClasses(args[1]);
            }  else {
                Console.WriteLine("Usage: pointless -f [script]");
            }

        }
    }
}
