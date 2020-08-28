using System;

namespace lang {

  public abstract class Stmt {

  public abstract T Accept<T>(IVisitor<T> visitor);

  public interface IVisitor<T> {
  }


  }
}
