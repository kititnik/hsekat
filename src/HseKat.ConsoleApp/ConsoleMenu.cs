using HseKat.Domain.Abstractions;
using HseKat.Domain.Things;
using HseKat.Domain.Vehicles;
using HseKat.Services.Fleet;
using HseKat.Services.Inspection;
using HseKat.Services.Reports;

namespace HseKat.ConsoleApp;

public sealed class ConsoleMenu
{
    private readonly IFleetService _fleetService;
    private readonly IFleetReportService _reportService;
    private readonly ConsoleInput _input;
    private int _lastInventoryNumber;

    public ConsoleMenu(IFleetService fleetService, IFleetReportService reportService, ConsoleInput input)
    {
        ArgumentNullException.ThrowIfNull(fleetService);
        ArgumentNullException.ThrowIfNull(reportService);
        ArgumentNullException.ThrowIfNull(input);

        _fleetService = fleetService;
        _reportService = reportService;
        _input = input;
    }

    public void Run()
    {
        try
        {
            while (true)
            {
                PrintMenu();
                var choice = _input.ReadInt("Выберите пункт", 0, 5);
                if (choice == 0)
                    return;

                Console.WriteLine();
                Execute(choice);
                Console.WriteLine();
            }
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine();
            Console.WriteLine("Ввод завершён.");
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("=== ВышКат: учёт парка ===");
        Console.WriteLine("1. Принять транспорт");
        Console.WriteLine("2. Добавить вещь");
        Console.WriteLine("3. Энергия и количество по типам");
        Console.WriteLine("4. Транспорт для новичков");
        Console.WriteLine("5. Инвентарный список");
        Console.WriteLine("0. Выход");
    }

    private void Execute(int choice)
    {
        try
        {
            switch (choice)
            {
                case 1:
                    AdmitVehicle();
                    break;
                case 2:
                    RegisterItem();
                    break;
                case 3:
                    PrintEnergyAndCounts();
                    break;
                case 4:
                    PrintBeginnerVehicles();
                    break;
                case 5:
                    PrintInventoryList();
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private void AdmitVehicle()
    {
        var type = _input.ReadInt("Тип (1 - самокат, 2 - велосипед, 3 - электровелосипед)", 1, 3);
        var name = _input.ReadText("Название");
        var state = _input.ReadInt("Состояние (1 - Good, 2 - NeedsRepair, 3 - Broken)", 1, 3) switch
        {
            1 => TechnicalState.Good,
            2 => TechnicalState.NeedsRepair,
            _ => TechnicalState.Broken
        };
        var ease = _input.ReadInt("Простота для новичка", Vehicle.MinBeginnerEase, Vehicle.MaxBeginnerEase);

        Vehicle vehicle = type switch
        {
            1 => new ElectricScooter(NextInventoryNumber(), name, state, ease, ReadDailyEnergy()),
            2 => new Bicycle(NextInventoryNumber(), name, state, ease),
            _ => new ElectricBicycle(NextInventoryNumber(), name, state, ease, ReadDailyEnergy())
        };

        var result = _fleetService.AdmitVehicle(vehicle);
        var prefix = $"#{vehicle.InventoryNumber} {Describe(vehicle)}";
        if (result == InspectionResult.Accepted)
            Console.WriteLine($"{prefix} принят в парк.");
        else
            Console.WriteLine($"{prefix} отклонён техосмотром (состояние: {vehicle.State}).");
    }

    private void RegisterItem()
    {
        var type = _input.ReadInt("Тип (1 - шлем, 2 - док-станция, 3 - зарядный шкаф)", 1, 3);
        var name = _input.ReadText("Название");

        IInventoryItem item = type switch
        {
            1 => new Helmet(NextInventoryNumber(), name),
            2 => new DockStation(NextInventoryNumber(), name, ReadDailyEnergy()),
            _ => new ChargingCabinet(NextInventoryNumber(), name, ReadDailyEnergy())
        };

        _fleetService.RegisterItem(item);
        Console.WriteLine($"#{item.InventoryNumber} {Describe(item)} поставлен на баланс.");
    }

    private void PrintEnergyAndCounts()
    {
        var counts = _reportService.GetCountsByType();
        if (counts.Count == 0)
        {
            Console.WriteLine("Парк пуст.");
            return;
        }

        Console.WriteLine($"Суммарное потребление: {_reportService.GetTotalDailyEnergyKwh()} кВт·ч/сутки");
        Console.WriteLine("Количество по типам:");
        foreach (var (type, count) in counts.OrderBy(pair => pair.Key))
            Console.WriteLine($"{type}: {count}");
    }

    private void PrintBeginnerVehicles()
    {
        var vehicles = _reportService.GetBeginnerVehicles();
        if (vehicles.Count == 0)
        {
            Console.WriteLine("Подходящего транспорта нет.");
            return;
        }

        Console.WriteLine("Транспорт для новичков:");
        foreach (var vehicle in vehicles)
            Console.WriteLine($"#{vehicle.InventoryNumber} {Describe(vehicle)} (простота {vehicle.BeginnerEase})");
    }

    private void PrintInventoryList()
    {
        var items = _reportService.GetInventoryList();
        if (items.Count == 0)
        {
            Console.WriteLine("Парк пуст.");
            return;
        }

        Console.WriteLine("Инвентарный список:");
        foreach (var item in items)
            Console.WriteLine($"#{item.InventoryNumber} {Describe(item)}");
    }

    private double ReadDailyEnergy()
    {
        return _input.ReadDouble("Расход энергии, кВт·ч/сутки", 0);
    }

    private int NextInventoryNumber()
    {
        return ++_lastInventoryNumber;
    }

    private static string Describe(IInventoryItem item)
    {
        return $"{item.GetType().Name} {item.Name}";
    }
}
