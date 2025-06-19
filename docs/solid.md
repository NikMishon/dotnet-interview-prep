# Принципы SOLID

SOLID — это акроним для пяти принципов объектно-ориентированного дизайна, которые были введены Робертом Мартином. Следование этим принципам помогает создавать более понятные, гибкие и поддерживаемые системы.

## 1. S – Single Responsibility Principle (Принцип единственной ответственности)
- **Суть**: У класса должна быть только одна причина для изменения.
- **Объяснение**: Класс должен быть сфокусирован на одной задаче или концепции. Если класс делает слишком много (например, и работает с данными, и логирует, и отправляет email), то изменение в одной из этих областей потребует изменения, тестирования и перевыпуска всего класса, что повышает риски.
- **Пример**:

  **Плохо:**
  ```csharp
  public class Report
  {
      public string GetReportData() { /* ... получение данных ... */ return "data"; }
      public void SaveToFile(string data) { /* ... сохранение в файл ... */ }
      public void PrintReport(string data) { /* ... печать отчета ... */ }
  }
  // Этот класс отвечает и за данные, и за сохранение, и за печать. 3 причины для изменения.
  ```

  **Хорошо:**
  ```csharp
  // Класс отвечает только за данные отчета
  public class ReportGenerator
  {
      public string GetReportData() { /* ... получение данных ... */ return "data"; }
  }

  // Класс отвечает только за сохранение
  public class ReportSaver
  {
      public void SaveToFile(string reportData) { /* ... сохранение в файл ... */ }
  }
  ```

## 2. O – Open/Closed Principle (Принцип открытости/закрытости)
- **Суть**: Программные сущности (классы, модули, функции) должны быть открыты для расширения, но закрыты для изменения.
- **Объяснение**: Вы должны иметь возможность добавлять новую функциональность, не изменяя существующий исходный код. Это достигается за счет использования абстракций (интерфейсов, абстрактных классов).
- **Пример**:

  **Плохо:**
  ```csharp
  public class Calculator
  {
      public double CalculateArea(object shape)
      {
          if (shape is Rectangle r) return r.Width * r.Height;
          if (shape is Circle c) return Math.PI * c.Radius * c.Radius;
          // Чтобы добавить квадрат, придется ИЗМЕНИТЬ этот метод.
          return 0;
      }
  }
  ```

  **Хорошо:**
  ```csharp
  public abstract class Shape
  {
      public abstract double CalculateArea();
  }

  public class Rectangle : Shape { /* ... */ }
  public class Circle : Shape { /* ... */ }

  // Чтобы добавить новую фигуру (e.g. Triangle), мы просто создадим новый класс,
  // не трогая существующий код. Система ОТКРЫТА для расширения.
  public class Triangle : Shape { /* ... */ } 
  ```

## 3. L – Liskov Substitution Principle (Принцип подстановки Барбары Лисков)
- **Суть**: Объекты в программе должны быть заменяемы на экземпляры их подтипов без изменения правильности выполнения программы.
- **Объяснение**: Если у вас есть код, который работает с базовым классом, он должен так же корректно работать с любым из его дочерних классов. Потомок не должен "ломать" ожидаемое поведение родителя.
- **Пример**: Классический пример с квадратом и прямоугольником.

  **Плохо:**
  ```csharp
  public class Rectangle
  {
      public virtual int Width { get; set; }
      public virtual int Height { get; set; }
  }
  
  public class Square : Rectangle
  {
      // Нарушение: при изменении ширины меняется и высота, 
      // что не соответствует поведению базового класса Rectangle.
      public override int Width { set { base.Width = base.Height = value; } }
      public override int Height { set { base.Width = base.Height = value; } }
  }
  
  void TestMethod(Rectangle r) 
  {
      r.Width = 5;
      r.Height = 10;
      // Если r - это Square, то Assert сломается, т.к. Width будет 10.
      Assert.AreEqual(5, r.Width); 
  }
  ```

  **Хорошо:**
  ```csharp
  // Не создавать такую иерархию. Квадрат — это не частный случай прямоугольника 
  // с точки зрения поведения. Лучше использовать отдельные классы или интерфейс.
  public interface IShape
  {
      double GetArea();
  }
  public class Rectangle : IShape { /* ... */ }
  public class Square : IShape { /* ... */ }
  ```

## 4. I – Interface Segregation Principle (Принцип разделения интерфейса)
- **Суть**: "Толстые" интерфейсы нужно разделять на более мелкие и специфичные, чтобы клиенты не зависели от методов, которые они не используют.
- **Объяснение**: Не заставляйте класс реализовывать методы, которые ему не нужны. Это приводит к созданию "пустых" реализаций или выбрасыванию `NotSupportedException`, что является плохим дизайном.
- **Пример**:

  **Плохо:**
  ```csharp
  public interface IWorker
  {
      void Work();
      void Eat();
  }

  public class Robot : IWorker
  {
      public void Work() { /* ... работает ... */ }
      // Робот не ест. Приходится реализовывать пустой метод.
      public void Eat() { } 
  }
  ```

  **Хорошо:**
  ```csharp
  public interface IWorkable { void Work(); }
  public interface IFeedable { void Eat(); }

  // Человек реализует оба интерфейса
  public class Human : IWorkable, IFeedable { /* ... */ }

  // Робот реализует только то, что ему нужно
  public class Robot : IWorkable { /* ... */ }
  ```

## 5. D – Dependency Inversion Principle (Принцип инверсии зависимостей)
- **Суть**: Модули верхних уровней не должны зависеть от модулей нижних уровней. Оба должны зависеть от абстракций. Абстракции не должны зависеть от деталей. Детали должны зависеть от абстракций.
- **Объяснение**: Вместо того чтобы высокоуровневая логика (например, бизнес-логика сервиса) напрямую зависела от низкоуровневой (например, конкретного класса для работы с SQL Server), она должна зависеть от интерфейса (например, `IRepository`). Это позволяет легко подменять реализации (например, заменить SQL Server на PostgreSQL или на заглушку для тестов), не изменяя высокоуровневую логику.
- **Пример**:

  **Плохо:**
  ```csharp
  // Высокоуровневый класс напрямую зависит от низкоуровневого
  public class ReportGenerator
  {
      private readonly SqlServerRepository _db = new SqlServerRepository();
      
      public void Generate()
      {
          var data = _db.GetData();
          // ...
      }
  }
  ```

  **Хорошо:**
  ```csharp
  public interface IReportRepository
  {
      string GetData();
  }
  
  public class SqlServerRepository : IReportRepository { /* ... */ }
  public class MongoDbRepository : IReportRepository { /* ... */ }

  // Высокоуровневый класс зависит от абстракции
  public class ReportGenerator
  {
      private readonly IReportRepository _repo;

      // Зависимость внедряется извне (Dependency Injection)
      public ReportGenerator(IReportRepository repo)
      {
          _repo = repo;
      }
      
      public void Generate()
      {
          var data = _repo.GetData();
          // ...
      }
  }
  ``` 