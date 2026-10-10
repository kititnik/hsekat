using HseKat.ConsoleApp;
using HseKat.Domain.Abstractions;
using HseKat.Repository;
using HseKat.Services.Fleet;
using HseKat.Services.Inspection;
using HseKat.Services.Reports;
using Microsoft.Extensions.DependencyInjection;

using var provider = new ServiceCollection()
    .AddSingleton<IInventoryRepository, InMemoryInventoryRepository>()
    .AddSingleton<IServiceCenter, BasicServiceCenter>()
    .AddSingleton<IBeginnerPolicy>(new MinEaseBeginnerPolicy(6))
    .AddSingleton<IFleetService, FleetService>()
    .AddSingleton<IFleetReportService, FleetReportService>()
    .AddSingleton<ConsoleInput>()
    .AddSingleton<ConsoleMenu>()
    .BuildServiceProvider();

provider.GetRequiredService<ConsoleMenu>().Run();
