using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Share.Entities;
using BlazorApp.Share.Enums;
using BlazorApp.Share.Extensions;

namespace BlazorApp.Application.Services;

public class RandomDataProvider : IRandomDataProvider
{
    private readonly IEmployeeRepository  _employeeRepository;
    private readonly IClientRepository    _clientRepository;
    private readonly IShiftRepository     _shiftRepository;
    private readonly IDeviationRepository _deviationRepository;

    public RandomDataProvider
    (IEmployeeRepository  employeeRepository,
     IClientRepository    clientRepository,
     IShiftRepository     shiftRepository,
     IDeviationRepository deviationRepository)
    {
        _employeeRepository  = employeeRepository;
        _clientRepository    = clientRepository;
        _shiftRepository     = shiftRepository;
        _deviationRepository = deviationRepository;
    }

    public async Task Generate()
    {
        Random random            = new Random();
        var    testDataGenerator = new RandomDataGenerator();
        var    testData          = testDataGenerator.Generate();

        foreach (var employee in testData.Employees)
        {
            await _employeeRepository.Add(employee);
        }

        foreach (var client in testData.Clients)
        {
            await _clientRepository.Add(client);
        }

        for (int i = 0; i < testData.Employees.Count; i++)
        {
            var employee = testData.Employees[i];
            var client   = testData.Clients[i];

            for (int j = 0; j < 6; j++)
            {
                var dateTime = DateTime.Now.AddDays(j);
                await CreateShift(employee, client, random, dateTime);
                
                dateTime = DateTime.Now.AddDays(-j);
                await CreateShift(employee, client, random, dateTime);
            }
        }
    }

    private async Task CreateShift(Employee employee, Client client, Random random, DateTime dateTime)
    {
        var morningShift = RandomDataGenerator.GetShift(employee.Id, client.Id);
        morningShift.Date      = dateTime.ToDateOnly();
        morningShift.StartTime = new TimeOnly(8,  0);
        morningShift.EndTime   = new TimeOnly(12, 0);
        morningShift.Status    = ShiftStatus.Planned;

        bool fiftyPercentChance = random.NextDouble() < 0.5;
        if (fiftyPercentChance)
        {
            var deviation = RandomDataGenerator.GetDeviation(morningShift);
            morningShift.Deviations.Add(deviation);
            await _deviationRepository.Add(deviation);
        }
        await _shiftRepository.Add(morningShift);

        // afternoon shift
        var afternoonShift = RandomDataGenerator.GetShift(employee.Id, client.Id);
        afternoonShift.Date      = dateTime.ToDateOnly();
        afternoonShift.StartTime = new TimeOnly(13, 0);
        afternoonShift.EndTime   = new TimeOnly(17, 0);
        afternoonShift.Status    = ShiftStatus.Planned;

        fiftyPercentChance = random.NextDouble() < 0.3;
        if (fiftyPercentChance)
        {
            var deviation = RandomDataGenerator.GetDeviation(afternoonShift);
            afternoonShift.Deviations.Add(deviation);
            await _deviationRepository.Add(deviation);
        }
        await _shiftRepository.Add(afternoonShift);
    }
}