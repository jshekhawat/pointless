using System;
using System.IO;
using System.Collections.Generic;


namespace tools {

    public class ASTGenerator {

        
        public static void Generate(String outputDir) {

            

            String outPutdir = outputDir;


            generateClasses("Expr", new List<string>() {
                "Binary: Expr Left, Token Operator, Expr Right",
                "Unary: Token Operator, Expr Right",
                "Grouping: Expr Expression",
                "Literal: Object TokenValue"
                
            }, outPutdir);

            generateClasses("Stmt", new List<string>() {

            }, outPutdir);
        }

        private static void generateClasses(String baseClass, List<String> subClasses, String outPutdir) {
            String filePath = Path.Combine(outPutdir, (baseClass + ".cs"));
            
            if(File.Exists(filePath)) File.Delete(filePath);                                    

            StreamWriter sw = new StreamWriter(Path.Combine(outPutdir, (baseClass + ".cs")));
            //StreamWriter sw = new StreamWriter(Console.OpenStandardOutput());
            sw.WriteLine("using System;");
         
            sw.WriteLine();
            
            sw.WriteLine("namespace lang {");
            sw.WriteLine();
            sw.WriteLine("  public abstract class " + baseClass + " {");
            sw.WriteLine();
            sw.WriteLine("  public abstract T Accept<T>(IVisitor<T> visitor);");
            sw.WriteLine();

            sw.WriteLine("  public interface IVisitor<T> {");
            foreach(var c in subClasses) {
                var sc = c.Split(':')[0];
                sw.WriteLine("      T Visit" + sc + baseClass + "(" + sc + " expr);");
            }
            sw.WriteLine("  }");
            sw.WriteLine();




            foreach(var c in subClasses) {
                var args = c.Split(':');
                var className = args[0].Trim();
                var fields = args[1].Split(',');
                sw.WriteLine();               
                sw.WriteLine("      public class " + className + " : " + baseClass + " { ");
                sw.WriteLine();
                
                foreach(var f in fields) {                                    
                    var types = f.Trim().Split(' ');                    
                    sw.WriteLine("          public " + types[0].Trim() + " " + types[1].Trim() + " {");
                    sw.WriteLine("              get;");
                    sw.WriteLine("              set;");
                    sw.WriteLine("          }");
                    sw.WriteLine();
                }

                sw.WriteLine("          public override T Accept<T>(IVisitor<T> visitor) {");
                sw.WriteLine("              return visitor.Visit" + className +  baseClass + "(this);");
                sw.WriteLine("          }");




                sw.WriteLine("      }");


            }


            sw.WriteLine();
            sw.WriteLine("  }");
            sw.WriteLine("}");

            sw.Close();
        }
    }


}