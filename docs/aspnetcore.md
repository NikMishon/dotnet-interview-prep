# ASP.NET Core: Ключевые аспекты

ASP.NET Core — это кросс-платформенный фреймворк с открытым исходным кодом для создания современных веб-приложений и сервисов.

## 1. Middleware (Конвейер обработки запросов)
- **Что это**: Middleware — это компоненты, которые выстраиваются в цепочку (конвейер) для обработки HTTP-запросов и ответов. Каждый компонент решает, передать ли запрос следующему компоненту в конвейере или завершить обработку.
- **Порядок важен**: Порядок регистрации Middleware в `Program.cs` определяет порядок их выполнения.
- **Примеры**:
  - `UseRouting()`: Определяет, какой endpoint будет обрабатывать запрос.
  - `UseAuthentication()`: Определяет, кем является пользователь.
  - `UseAuthorization()`: Определяет, что разрешено делать пользователю.
  - `UseStaticFiles()`: Обслуживает статические файлы (JS, CSS, изображения).
  - `MapControllers()` / `MapRazorPages()`: Передает запрос в соответствующий контроллер или страницу.
- **Пример конвейера в `Program.cs`**:
  ```csharp
  var app = builder.Build();

  if (app.Environment.IsDevelopment())
  {
      app.UseDeveloperExceptionPage();
  }
  
  app.UseStaticFiles();
  app.UseRouting();
  app.UseAuthentication();
  app.UseAuthorization();

  app.MapControllers();
  
  app.Run();
  ```

## 2. Dependency Injection (DI)
- **Суть**: ASP.NET Core построен на основе встроенного DI-контейнера. Это позволяет создавать слабосвязанные, тестируемые и поддерживаемые приложения.
- **Регистрация сервисов**: Сервисы (классы, которые выполняют какую-то работу) регистрируются в контейнере в `Program.cs`.
  ```csharp
  builder.Services.AddScoped<IUserService, UserService>();
  builder.Services.AddTransient<IEmailSender, EmailSender>();
  ```
- **Времена жизни (Lifetimes)**:
  - `Singleton`: Один экземпляр на всё приложение.
  - `Scoped`: Один экземпляр на один HTTP-запрос.
  - `Transient`: Новый экземпляр каждый раз, когда запрашивается.

## 3. Конфигурация
- **Система конфигурации**: Гибкая система, которая позволяет читать настройки из различных источников.
- **Источники (Providers)**: `appsettings.json`, переменные окружения, Azure Key Vault, секреты пользователя (`user-secrets`), аргументы командной строки.
- **Приоритеты**: Источники, добавленные позже, "перетирают" значения из источников, добавленных ранее. Переменные окружения имеют более высокий приоритет, чем `appsettings.json`.
- **Паттерн Options**: Способ строготипизированной работы с конфигурацией. Создается класс, который отражает структуру секции в `appsettings.json`, и этот класс регистрируется в DI-контейнере.
  ```json
  // appsettings.json
  "SmtpSettings": {
    "Host": "smtp.example.com",
    "Port": 587
  }
  ```
  ```csharp
  // Класс настроек
  public class SmtpSettings { public string Host { get; set; } public int Port { get; set; } }
  
  // Регистрация в Program.cs
  builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
  
  // Использование в классе
  public class EmailService
  {
      private readonly SmtpSettings _settings;
      public EmailService(IOptions<SmtpSettings> options)
      {
          _settings = options.Value;
      }
  }
  ```

## 4. Роутинг (Routing)
- **Что это**: Механизм, который сопоставляет входящие URL-адреса с обработчиками (endpoints).
- **Типы роутинга**:
  - **Convention-based Routing (Традиционный)**: Определяет общие шаблоны URL. Используется в основном в MVC-приложениях с Views.
    ```csharp
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    ```
  - **Attribute-based Routing (На основе атрибутов)**: Маршруты задаются непосредственно над контроллерами и методами с помощью атрибутов (`[Route(...)]`, `[HttpGet(...)]`). Это предпочтительный способ для Web API.
    ```csharp
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id) { /* ... */ }
    }
    ```

## 5. Модели хостинга
- **Kestrel**: Встроенный, кросс-платформенный веб-сервер. Он быстрый и эффективный, но не имеет всех возможностей "взрослых" серверов.
- **Reverse Proxy**: В production-окружении Kestrel обычно используется за **обратным прокси-сервером** (IIS в Windows, Nginx или Apache в Linux).
- **Зачем нужен Reverse Proxy**:
  - **Безопасность**: Скрывает Kestrel от прямого доступа из интернета.
  - **Балансировка нагрузки**: Распределяет запросы между несколькими экземплярами приложения.
  - **SSL-терминация**: Управляет HTTPS-соединениями.
  - **Кэширование**: Кэширует ответы.