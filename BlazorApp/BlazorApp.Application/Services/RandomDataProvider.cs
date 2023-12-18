using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Share.Entities;
using BlazorApp.Share.Enums;

namespace BlazorApp.Application.Services;

public class RandomDataProvider : IRandomDataProvider
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IShiftRepository _shiftRepository;
    private readonly IDeviationRepository _deviationRepository;

    public RandomDataProvider(IEmployeeRepository employeeRepository, IClientRepository clientRepository, IShiftRepository shiftRepository, IDeviationRepository deviationRepository)
    {
        _employeeRepository = employeeRepository;
        _clientRepository = clientRepository;
        _shiftRepository = shiftRepository;
        _deviationRepository = deviationRepository;
    }

    public async Task Generate()
    {
        var testDataGenerator = new RandomDataGenerator();
        var testData = testDataGenerator.Generate();

        foreach (var employee in testData.Employees)
        {
            await _employeeRepository.Add(employee);
        }

        foreach (var client in testData.Clients)
        {
            await _clientRepository.Add(client);
        }

        var clientsGroup = testData.Clients.Partition(4).ToList();
        for (int i = 0; i < testData.Employees.Count; i++)
        {
            var clients = clientsGroup[i];
            var employee = testData.Employees[i];
            var shifts = new List<Shift>();
            foreach (var client in clients)
            {
                var shift = RandomDataGenerator.GetShift(employee.Id, client.Id);
                var deviation = RandomDataGenerator.GetDeviation(shift);
                await _deviationRepository.Add(deviation);
                shift.Deviations = new List<Deviation> { deviation };
                // shift.Client = client;
                // shift.Employee = employee;
                shifts.Add(shift);
                await _shiftRepository.Add(shift);
                client.Shifts = new List<Shift> { shift };
                await _clientRepository.Update(client.Id.ToString(), client);
            }

            employee.Shifts = shifts;

            await _employeeRepository.Update(employee.Id.ToString(), employee);

        }
    }
}