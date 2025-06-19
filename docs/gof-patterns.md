# Паттерны проектирования (GoF - Gang of Four)

Паттерны проектирования — это переиспользуемые решения часто встречающихся проблем в рамках заданного контекста. Это не конкретные классы или библиотеки, а скорее формализованные лучшие практики.

## 1. Порождающие паттерны (Creational Patterns)
Отвечают за процесс создания объектов, позволяя сделать систему независимой от того, как именно создаются, компонуются и представляются её объекты.

- **Singleton (Одиночка)**
  - **Назначение**: Гарантирует, что у класса есть только один экземпляр, и предоставляет к нему глобальную точку доступа.
  - **Пример**:
    ```csharp
    public sealed class Logger
    {
        private static readonly Lazy<Logger> lazyInstance = 
            new Lazy<Logger>(() => new Logger());

        public static Logger Instance => lazyInstance.Value;

        private Logger() { /* ... инициализация ... */ }

        public void Log(string message) { /* ... */ }
    }
    // Использование: Logger.Instance.Log("message");
    ```
- **Factory Method (Фабричный метод)**
  - **Назначение**: Определяет интерфейс для создания объекта, но оставляет подклассам решение о том, какой именно класс создавать.
  - **Пример**:
    ```csharp
    // Абстрактный создатель
    public abstract class Document
    {
        public abstract void CreatePages();
    }
    
    // Конкретные создатели
    public class Report : Document { /* ... */ }
    public class Resume : Document { /* ... */ }

    // Фабрика
    public abstract class DocumentFactory
    {
        // Фабричный метод
        public abstract Document CreateDocument();
    }
    
    public class ReportFactory : DocumentFactory { /* ... */ }
    ```
- **Abstract Factory (Абстрактная фабрика)**
  - **Назначение**: Предоставляет интерфейс для создания семейств взаимосвязанных объектов, не специфицируя их конкретных классов.
  - **Пример**:
    ```csharp
    // Интерфейсы для семейства продуктов
    public interface IButton { void Render(); }
    public interface ICheckbox { void Render(); }

    // Конкретные реализации для Windows
    public class WinButton : IButton { /* ... */ }
    public class WinCheckbox : ICheckbox { /* ... */ }

    // Абстрактная фабрика
    public interface IUIFactory
    {
        IButton CreateButton();
        ICheckbox CreateCheckbox();
    }

    // Конкретная фабрика
    public class WindowsFactory : IUIFactory { /* ... */ }
    ```
- **Builder (Строитель)**
  - **Назначение**: Отделяет конструирование сложного объекта от его представления, так что в результате одного и того же процесса конструирования могут получаться разные представления.
  - **Пример**:
    ```csharp
    // Сложный объект
    public class Pizza
    {
        public string Dough { get; set; }
        public string Sauce { get; set; }
        public string Topping { get; set; }
    }

    // Абстрактный строитель
    public abstract class PizzaBuilder
    {
        protected Pizza pizza;
        public Pizza GetPizza() => pizza;
        public abstract void BuildDough();
        public abstract void BuildSauce();
        public abstract void BuildTopping();
    }
    // Использование: директор (director) управляет строителем для создания пиццы по шагам.
    ```
- **Prototype (Прототип)**
  - **Назначение**: Задаёт виды создаваемых объектов с помощью экземпляра-прототипа и создаёт новые объекты путём копирования этого прототипа.
  - **Пример**:
    ```csharp
    public abstract class Shape
    {
        public abstract Shape Clone();
    }
    
    public class Rectangle : Shape
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public override Shape Clone()
        {
            // MemberwiseClone создает поверхностную копию
            return (Shape)this.MemberwiseClone();
        }
    }
    ```

## 2. Структурные паттерны (Structural Patterns)
Определяют, как классы и объекты могут быть скомбинированы для образования более крупных структур.

- **Adapter (Адаптер)**
  - **Назначение**: Преобразует интерфейс одного класса в интерфейс другого, который ожидают клиенты. Позволяет классам работать вместе, когда это было бы невозможно из-за несовместимых интерфейсов.
  - **Пример**:
    ```csharp
    // Существующий класс с несовместимым интерфейсом
    public class OldLogger { public void LogMessage(string msg) { /* ... */ } }

    // Целевой интерфейс, который ожидает система
    public interface ILogger { void Log(string message); }

    // Адаптер
    public class LoggerAdapter : ILogger
    {
        private readonly OldLogger _oldLogger = new OldLogger();
        public void Log(string message) => _oldLogger.LogMessage(message);
    }
    ```
- **Bridge (Мост)**
  - **Назначение**: Отделяет абстракцию от её реализации так, чтобы то и другое можно было изменять независимо.
  - **Пример**: У вас есть фигуры (Абстракция) и способы их отрисовки (Реализация). Мост позволяет комбинировать любую фигуру с любым способом отрисовки.
    ```csharp
    // Абстракция
    public abstract class Shape
    {
        protected IRenderer renderer; // Ссылка на реализацию
        public Shape(IRenderer renderer) { this.renderer = renderer; }
        public abstract void Draw();
    }
    // Реализация
    public interface IRenderer { void RenderCircle(float radius); }
    
    public class VectorRenderer : IRenderer { /* ... */ }
    public class RasterRenderer : IRenderer { /* ... */ }
    ```
- **Composite (Компоновщик)**
  - **Назначение**: Объединяет объекты в древовидные структуры для представления иерархий "часть-целое". Позволяет клиентам единообразно обращаться как к отдельным объектам, так и к группам объектов.
  - **Пример**: Графические примитивы. И `Линия`, и `Круг`, и `ГруппаОбъектов` могут быть нарисованы.
    ```csharp
    public interface IGraphic
    {
        void Draw();
    }
    public class Line : IGraphic { /* ... */ }
    public class CompositeGraphic : IGraphic
    {
        private List<IGraphic> _children = new List<IGraphic>();
        public void Draw() => _children.ForEach(c => c.Draw());
    }
    ```
- **Decorator (Декоратор)**
  - **Назначение**: Динамически добавляет объекту новые обязанности. Является гибкой альтернативой наследованию для расширения функциональности.
  - **Пример**: `Stream` в .NET.
    ```csharp
    // Компонент
    public interface IDataSource { void Write(string data); }
    public class FileDataSource : IDataSource { /* ... */ }
    
    // Базовый декоратор
    public abstract class DataSourceDecorator : IDataSource { /* ... */ }

    // Конкретный декоратор
    public class EncryptionDecorator : DataSourceDecorator
    {
        public override void Write(string data)
        {
            // Добавляем новую функциональность - шифрование
            var encrypted = Encrypt(data);
            base.Write(encrypted);
        }
    }
    ```
- **Facade (Фасад)**
  - **Назначение**: Предоставляет простой, унифицированный интерфейс к сложной подсистеме. Скрывает сложность, предоставляя удобный API.
  - **Пример**: Фасад для запуска видео.
    ```csharp
    public class VideoConverterFacade
    {
        public void ConvertVideo(string filename, string format)
        {
            // Внутри скрыта сложная логика работы с кодеками,
            // аудио микшером, файловой системой и т.д.
            var file = new VideoFile(filename);
            var sourceCodec = CodecFactory.Extract(file);
            // ...
        }
    }
    ```
- **Flyweight (Приспособленец)**
  - **Назначение**: Позволяет вместить в память большее количество объектов за счет разделения общего состояния между ними. Экономит память, вынося общее состояние во внешнее хранилище.
  - **Пример**: Отрисовка деревьев в лесу. У всех деревьев есть общие данные (меш, текстуры - внутреннее состояние) и уникальные (координаты, размер - внешнее состояние).
    ```csharp
    // Приспособленец
    public class TreeType { /* Меш, текстуры - общие для всех деревьев одного типа */ }

    // Фабрика приспособленцев
    public class TreeFactory { /* Кэширует и выдает TreeType */ }
    
    // Контекст
    public class Tree { private TreeType _type; /* Координаты, размер - уникальные */ }
    ```
- **Proxy (Заместитель)**
  - **Назначение**: Предоставляет суррогат или заполнитель для другого объекта для контроля доступа к нему.
  - **Пример**: Ленивая загрузка "тяжелого" объекта.
    ```csharp
    public interface IImage { void Display(); }
    public class RealImage : IImage { /* ... загрузка из файла ... */ }
    
    public class ImageProxy : IImage
    {
        private RealImage _realImage;
        private string _filename;
        
        public void Display()
        {
            if (_realImage == null)
            {
                _realImage = new RealImage(_filename); // Загрузка по требованию
            }
            _realImage.Display();
        }
    }
    ```

## 3. Поведенческие паттерны (Behavioral Patterns)
Определяют алгоритмы и способы коммуникации между объектами.

- **Chain of Responsibility (Цепочка обязанностей)**
  - **Назначение**: Позволяет избежать привязки отправителя запроса к его получателю, давая шанс обработать запрос нескольким объектам. Связывает объекты-получатели в цепочку и передает запрос вдоль этой цепочки, пока он не будет обработан.
  - **Пример**: Цепочка обработчиков middleware в ASP.NET Core.
    ```csharp
    public abstract class Handler { /* ... */ public abstract void HandleRequest(object request); }
    public class ConcreteHandler1 : Handler { /* ... */ }
    public class ConcreteHandler2 : Handler { /* ... */ }
    // Каждый обработчик решает, может ли он обработать запрос, или его нужно передать дальше по цепочке.
    ```
- **Command (Команда)**
  - **Назначение**: Инкапсулирует запрос как объект, позволяя тем самым параметризовать клиентов с другими запросами, ставить запросы в очередь или протоколировать их, а также поддерживать отмену операций.
  - **Пример**: Операции в текстовом редакторе (Копировать, Вставить).
    ```csharp
    public interface ICommand { void Execute(); }
    public class CopyCommand : ICommand { /* ... */ }
    public class PasteCommand : ICommand { /* ... */ }
    // Кнопка в UI хранит объект ICommand и просто вызывает Execute(), не зная, что конкретно делает команда.
    ```
- **Interpreter (Интерпретатор)**
  - **Назначение**: Для заданного языка определяет представление его грамматики, а также интерпретатор этой грамматики.
  - **Пример**: Редко используется. Применяется для парсинга SQL или других языков.
- **Iterator (Итератор)**
  - **Назначение**: Предоставляет способ последовательного доступа ко всем элементам составного объекта, не раскрывая его внутреннего представления.
  - **Пример**: `foreach` в C# работает с любым объектом, реализующим `IEnumerable`.
    ```csharp
    // IEnumerable - это стандартный интерфейс итератора в .NET
    foreach (var item in myList) { /* ... */ }
    ```
- **Mediator (Посредник)**
  - **Назначение**: Определяет объект, инкапсулирующий способ взаимодействия множества объектов. Обеспечивает слабую связность, избавляя объекты от необходимости явно ссылаться друг на друга.
  - **Пример**: Диспетчер в аэропорту. Самолеты не общаются друг с другом напрямую, а только с диспетчером.
    ```csharp
    public interface IChatMediator { void SendMessage(string msg, User user); }
    public class User { /* ... */ }
    // Все пользователи общаются через центральный объект-медиатор.
    ```
- **Memento (Снимок)**
  - **Назначение**: Не нарушая инкапсуляции, фиксирует и выносит за пределы объекта его внутреннее состояние так, чтобы позднее можно было восстановить в нем объект.
  - **Пример**: Реализация Undo/Redo (Отменить/Повторить) в редакторе.
    ```csharp
    // Originator
    public class TextEditor { public Memento CreateMemento() { /* ... */ } public void Restore(Memento m) { /* ... */ } }
    // Memento
    public class Memento { public string State { get; } }
    // Caretaker
    public class History { /* ... хранит список Memento ... */ }
    ```
- **Observer (Наблюдатель)**
  - **Назначение**: Определяет зависимость "один ко многим" между объектами так, что при изменении состояния одного объекта все зависящие от него оповещаются и автоматически обновляются.
  - **Пример**: События (`event`) в C#.
    ```csharp
    // Subject
    public class StockTicker
    {
        public event EventHandler<StockPrice> StockPriceChanged;
    }
    // Observer'ы подписываются на событие.
    ```
- **State (Состояние)**
  - **Назначение**: Позволяет объекту изменять свое поведение при изменении его внутреннего состояния. Внешне это выглядит так, как будто изменился класс объекта.
  - **Пример**: Документ в системе документооборота (Черновик -> На согласовании -> Опубликован).
    ```csharp
    public abstract class DocumentState { public abstract void Publish(); }
    public class DraftState : DocumentState { /* ... */ }
    public class PublishedState : DocumentState { /* ... */ }
    public class Document { private DocumentState _state; }
    // Поведение метода document.Publish() зависит от текущего состояния.
    ```
- **Strategy (Стратегия)**
  - **Назначение**: Определяет семейство алгоритмов, инкапсулирует каждый из них и делает их взаимозаменяемыми. Позволяет изменять алгоритмы независимо от клиентов, которые их используют.
  - **Пример**: Различные способы сжатия файлов.
    ```csharp
    public interface ICompressionStrategy { void Compress(string file); }
    public class ZipCompression : ICompressionStrategy { /* ... */ }
    public class RarCompression : ICompressionStrategy { /* ... */ }
    // Контекст (например, архиватор) использует один из этих алгоритмов через общий интерфейс.
    ```
- **Template Method (Шаблонный метод)**
  - **Назначение**: Определяет "скелет" алгоритма в операции, перекладывая определение некоторых шагов на подклассы. Позволяет подклассам переопределять некоторые шаги алгоритма, не меняя его структуру в целом.
  - **Пример**: Сборка приложения на CI. Общий алгоритм (скачать, собрать, протестировать, запаковать) одинаков, но конкретные шаги могут отличаться.
    ```csharp
    public abstract class Builder
    {
        public void Build() // Шаблонный метод
        {
            DownloadSource();
            Compile();
            Test();
            Package();
        }
        protected abstract void Compile();
        protected abstract void Package();
        // ... другие методы ...
    }
    ```
- **Visitor (Посетитель)**
  - **Назначение**: Представляет операцию, которую нужно выполнить над элементами структуры объектов. Позволяет определить новую операцию, не изменяя классы этих элементов.
  - **Пример**: Экспорт фигур (`Circle`, `Square`) в разные форматы (`XML`, `JSON`).
    ```csharp
    public interface IShape { void Accept(IVisitor visitor); }
    public interface IVisitor { void VisitCircle(Circle c); void VisitSquare(Square s); }
    public class XmlExportVisitor : IVisitor { /* ... реализует экспорт в XML ... */ }
    // Чтобы добавить экспорт в JSON, создается новый Visitor, не трогая классы фигур.
    ``` 