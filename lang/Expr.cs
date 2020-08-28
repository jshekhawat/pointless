using System;

namespace lang {

  public abstract class Expr {

  public abstract T Accept<T>(IVisitor<T> visitor);

  public interface IVisitor<T> {
      T VisitBinaryExpr(Binary expr);
      T VisitUnaryExpr(Unary expr);
      T VisitGroupingExpr(Grouping expr);
      T VisitLiteralExpr(Literal expr);
  }


      public class Binary : Expr { 

          public Expr Left {
              get;
              set;
          }

          public Token Operator {
              get;
              set;
          }

          public Expr Right {
              get;
              set;
          }

          public override T Accept<T>(IVisitor<T> visitor) {
              return visitor.VisitBinaryExpr(this);
          }
      }

      public class Unary : Expr { 

          public Token Operator {
              get;
              set;
          }

          public Expr Right {
              get;
              set;
          }

          public override T Accept<T>(IVisitor<T> visitor) {
              return visitor.VisitUnaryExpr(this);
          }
      }

      public class Grouping : Expr { 

          public Expr Expression {
              get;
              set;
          }

          public override T Accept<T>(IVisitor<T> visitor) {
              return visitor.VisitGroupingExpr(this);
          }
      }

      public class Literal : Expr { 

          public Object TokenValue {
              get;
              set;
          }

          public override T Accept<T>(IVisitor<T> visitor) {
              return visitor.VisitLiteralExpr(this);
          }
      }

  }
}
