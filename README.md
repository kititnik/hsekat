# ВышКат

Консольное приложение учёта парка кикшеринга. C# / .NET 10, DI через `Microsoft.Extensions.DependencyInjection`, тесты на xUnit + NSubstitute.

## Запуск
```
dotnet run --project src/HseKat.ConsoleApp
```
### Тесты
```
dotnet test
```

## Структура

```
src/
    HseKat.Domain       сущности и интерфейсы
    HseKat.Services     техосмотр, приёмка, отчёты. Зависит от Domain
    HseKat.Repository   хранилище в памяти. Зависит от Domain
    HseKat.ConsoleApp   меню. Зависит от всего
tests/
    HseKat.Tests        тесты. Зависит от всего
```

## Модель

```
InventoryItem (abstract) : IInventoryItem
    Vehicle (abstract)
        Bicycle
        ElectricScooter : IEnergyConsumer
        ElectricBicycle : IEnergyConsumer
    Thing (abstract)
        Helmet
        DockStation : IEnergyConsumer
        ChargingCabinet : IEnergyConsumer
```

- `IInventoryItem` - всё, что стоит на балансе.
- `IEnergyConsumer` - всё что потребляет энергию.
- `Vehicle` - техническое состояние и простота для новичка.
- `Thing` - вещи, которые ставятся на баланс без техосмотра. Своих членов нет, нужен для сигнатуры `RegisterItem`.

## Ключевые решения

- Техосмотр. `FleetService.AdmitVehicle` отдаёт транспорт в `IServiceCenter`, и в репозиторий он попадает только при `Accepted`. `BasicServiceCenter` принимает только `TechnicalState.Good`. Вещи регистрируются через `RegisterItem`, который принимает только `Thing`, иначе можно было бы обойти техосмотр.
- Инвентарные номера выдаёт репозиторий (`IInventoryRepository.NextInventoryNumber`). Уникальность обеспечивает тот, кто хранит данные. `FleetService.NextInventoryNumber` - место для будущих бизнес-правил нумерации (формат, префиксы). Номер выдаётся до техосмотра, поэтому у отклонённого транспорта он сгорает и в нумерации возможны пропуски.
- Правило для новичков вынесено в `IBeginnerPolicy`. По умолчанию это `MinEaseBeginnerPolicy(6)`.

## SOLID

| Принцип | Где |
|---|---|
| S | `FleetService` добавляет в репозиторий, `FleetReportService` отдаёты отчёты из него, техосмотр отдельно в `IServiceCenter`. |
| O | Новый транспорт - новый наследник `Vehicle`, новое правило для новичков - новая `IBeginnerPolicy`. |
| L | Любой `Vehicle` одинаково проходит техосмотр и отчёты. |
| I | Энергия вынесена в `IEnergyConsumer`, шлем и велосипед его не реализуют. |
| D | Сервисы зависят от интерфейсов, реализации собираются в `Program.cs`. |

## Обязательные вопросы

### Какой принцип применён наиболее явно?
DIP: `FleetService` зависит от `IServiceCenter`, поэтому техосмотр подменяется в тестах без правки сервиса.

### Какой принцип сознательно ограничен?
- OCP в `ConsoleMenu` - новый тип нужно дописывать в меню в нескольких местах. Однако городить фабрики ради трёх типов я посчитал оверинжинирингом.
- DRY для `DailyEnergyKwh`, который повторяется в четырёх классах, потому что потребители в разных ветках иерархии. Лишний уровень наследования выглядит оверинжинирингом.

### Что будет при расширении?
- Новый транспорт - добавляется новый класс-наследник Vehicle и пункт в `ConsoleMenu`.
- Сотрудник - не реализует `IInventoryItem`, поэтому не попадает в репозиторий и отчёты. Ничего не ломается.
- Склад запчастей - отдельный сервис со своим репозиторием, так как внутри склада множество запчастей, которые учитываются по количеству. Существующие сервисы при этом тоже не меняются.

### Какие тесты фиксируют техосмотр и правило для новичков?
- Техосмотр - `AdmitVehicle_Rejected_ShouldNotAddToRepository` и `AdmitVehicle_Accepted_ShouldAddToRepositoryOnce`. IServiceCenter подменяется через NSubstitute и передаётся в конструктор FleetService.
- Новички - `GetBeginnerVehicles_EaseBoundary_ShouldReturnExpectedResult`. Новый IBeginnerPolicy и регистрация в Program.cs

## Тесты

32 теста, покрытие 94% по Domain, Services и Repository.

```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator \
  -reports:"tests/HseKat.Tests/TestResults/*/coverage.cobertura.xml" \
  -targetdir:"coverage-report" \
  -reporttypes:Html
```

ИИ не использовался
