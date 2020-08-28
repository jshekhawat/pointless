using System;
using System.Text;

namespace lang {

    public class Printer : Expr.IVisitor<String>
    {

        public String Print(Expr expr) {
            return expr.Accept(this);            
        }

        public String MakeTree(String name, Expr[] exprs) {
            
            StringBuilder sb = new StringBuilder();
            sb.Append("(").Append(name);

            foreach(var e in exprs) {
                sb.Append(" ");
                sb.Append(e.Accept(this));                
            }
            sb.Append(")");
            
            return sb.ToString();
        }

        public string VisitBinaryExpr(Expr.Binary expr)
        {
            return MakeTree(expr.Operator.Lexeme, new[] {expr.Left, expr.Right});
        }

        public string VisitGroupingExpr(Expr.Grouping expr)
        {
            return MakeTree("Grouping Expression", new[] {expr.Expression});
        }

        public string VisitLiteralExpr(Expr.Literal expr)
        {
            if(expr.TokenValue == null) return "Nil";
            return expr.TokenValue.ToString();
        }

        public string VisitUnaryExpr(Expr.Unary expr)
        {
            return MakeTree(expr.Operator.Lexeme, new[] { expr.Right });
        }
    }

}