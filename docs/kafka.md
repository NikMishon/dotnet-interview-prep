# Apache Kafka

Apache Kafka — это распределённая платформа для потоковой обработки событий, которая используется для публикации и подписки, хранения и обработки потоков событий в режиме реального времени. Kafka разработана для обеспечения высокой пропускной способности, низкой задержки, масштабируемости и отказоустойчивости.

## Основные концепции

- **Событие (Event)**: Запись о том, что "что-то произошло". В Kafka событие (или сообщение) обычно состоит из ключа, значения, временной метки и опциональных метаданных.
- **Продюсер (Producer)**: Клиентское приложение, которое публикует (пишет) события в Kafka.
- **Консумер (Consumer)**: Клиентское приложение, которое подписывается на (читает и обрабатывает) события из Kafka.
- **Топик (Topic)**: Категория или имя фида, в который публикуются события. В Kafka топики разделены на партиции.
- **Партиция (Partition)**: Топики разбиваются на несколько партиций. Каждая партиция — это упорядоченный, неизменяемый лог событий. Партиционирование позволяет распределять нагрузку и обеспечивает параллелизм.
- **Офсет (Offset)**: Уникальный последовательный идентификатор для каждого события в партиции.
- **Брокер (Broker)**: Сервер Kafka, который хранит данные. Кластер Kafka состоит из одного или нескольких брокеров.
- **Группа консумеров (Consumer Group)**: Одна или несколько консумеров, которые совместно читают данные из топика. Каждая партиция читается только одним консумером из группы.

## Архитектура

Kafka использует модель "публикация-подписка" (pub/sub). Продюсеры отправляют события в топики, а консумеры подписываются на топики для чтения событий.

Ключевой элемент архитектуры Kafka — это **распределённый лог коммитов**. Данные в партициях хранятся на диске и реплицируются между брокерами для обеспечения отказоустойчивости. Консумеры отслеживают свою позицию в логе (офсет) и могут читать данные в своём собственном темпе. Это позволяет хранить данные в Kafka в течение длительного времени и перечитывать их при необходимости.

## Сравнение Kafka и RabbitMQ

| Характеристика      | Apache Kafka                                                                  | RabbitMQ                                                                                  |
| ------------------- | ----------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| **Архитектура**     | Распределённая платформа для потоковой обработки событий (распределённый лог). | Традиционный брокер сообщений с поддержкой сложных сценариев маршрутизации.               |
| **Модель доставки** | Pull-модель: консумеры сами запрашивают (вытягивают) данные из брокера.       | Push-модель: брокер проталкивает сообщения консумерам.                                    |
| **Хранение данных** | Данные хранятся на диске в течение настраиваемого периода (могут быть "вечными"). | Сообщения удаляются после подтверждения обработки консумером.                             |
| **Пропускная способность** | Очень высокая, спроектирована для больших объёмов данных (миллионы сообщений/сек). | Высокая, но обычно ниже, чем у Kafka.                                                     |
| **Маршрутизация**   | Простая маршрутизация на основе топиков и партиций.                           | Гибкая и сложная маршрутизация с использованием exchanges, queues и bindings.              |
| **Порядок сообщений** | Гарантируется в пределах одной партиции.                                      | Гарантируется в пределах одной очереди (если нет приоритетов).                             |
| **Приоритеты**      | Не поддерживает приоритезацию сообщений.                                      | Поддерживает очереди с приоритетами.                                                      |
| **Протоколы**       | Использует собственный бинарный протокол поверх TCP.                          | Поддерживает AMQP, MQTT, STOMP и др.                                                      |
| **Основные юзкейсы** | - Потоковая обработка данных в реальном времени<br>- Агрегация логов<br>- Аналитика больших данных<br>- Event Sourcing | - Фоновые задачи (work queues)<br>- Комплексная маршрутизация сообщений<br>- Связь между микросервисами |

### Когда использовать Kafka?

- Когда нужна высокая пропускная способность для обработки потоков данных в реальном времени.
- Для систем, где данные нужно хранить и перечитывать (например, для аналитики или восстановления состояния).
- В сценариях сбора и агрегации логов и метрик.
- Для построения event-driven архитектур, основанных на потоках событий.

### Когда использовать RabbitMQ?

- Когда нужна сложная и гибкая маршрутизация сообщений.
- Для традиционных "задач в очереди" (task queues), где нужно распределить работу между несколькими воркерами.
- Когда требуется поддержка различных протоколов обмена сообщениями.
- Для систем, где важна низкая задержка и гарантированная доставка каждого отдельного сообщения с подтверждением. 