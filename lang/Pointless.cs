using System;
using System.IO;
using tools;
using System.Linq;


namespace lang {

    public class Pointless {

        public static void runREPL() {

            String[] emojis = new String[] {"👀", "😒", "🤯", "💩", "🤯", "😭", "💀", "🤦‍♂️", "🤷‍♂️", "😩"}; 
            var rand = new Random();

            while(true) {                
                Console.Write("{0} > ", emojis[rand.Next(0, emojis.Length)]);
                var val = Console.ReadLine();

                if (val != "exit") {                    
                    if(!String.IsNullOrEmpty(val)) {
                        run(val);        
                    }
                } else {
                    break;
                }
            }
        }

        private  static void run(string val) {
            Lexer lexer = new Lexer(val);
            var tokens = lexer.Tokenise();
            Parser parser = new Parser(tokens);
            Printer printer = new Printer();            
            Console.WriteLine(printer.Print(parser.Parse()));
        }

        public void runFile() {
                
        }

        public static void GenerateClasses(String outputDir) {
   
            ASTGenerator.Generate(outputDir);
        }

    }
}
