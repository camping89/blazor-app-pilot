using System.Diagnostics;
using BlazorApp.Share.Enums;
using BlazorApp.Share.Models;
using Bogus;

namespace BlazorApp.Application;

public class TestData
{
    public List<Client> Clients { get; set; } = new();
    public List<Employee> Employees { get; set; } = new();
    public List<Shift> Shifts { get; set; } = new();
    public List<Deviation> Deviations { get; set; } = new();
}

public class TestDataGenerator
{
    public TestData Generate()
    {
        var clients = GetClient(20);
        var employees = GetEmployee(5);
        var shifts = new List<Shift>();
        var deviations = new List<Deviation>();
        
        Randomizer.Seed = new Random(new Random().Next(1000, 10000));

        foreach (var employee in employees)
        {
            var clientIds = clients.Select(_ => _.Id).ToList();
            var size = Randomizer.Seed.Next(2, 5);
            for (var i = 0; i < size; i++) // 2-5 clients per employee
            {
                var clientId = clientIds[Randomizer.Seed.Next(0, clientIds.Count)];
                clientIds.Remove(clientId);

                var shiftCount = Randomizer.Seed.Next(10, 21);
                for (var j = 0; j < shiftCount; j++)
                {
                    var shift = GetShift(employee.Id, clientId);
                    shifts.Add(shift);
                }

                foreach (var shift in shifts)
                {
                    var deviation = GetDeviation(shift);
                    deviations.Add(deviation);
                }
            }
        }
        
        return new TestData
        {
            Clients = clients,
            Employees = employees,
            Shifts = shifts,
            Deviations = deviations
        };
    }

    public static List<Employee> GetEmployee(int amount)
    {
        var faker = new Faker<Employee>().StrictMode(true)
            .RuleFor(_ => _.Id, f => f.IndexGlobal)
            .RuleFor(_ => _.Name, f => f.Person.FullName)
            .RuleFor(_ => _.THoursWeekly, f => f.Random.Number(10, 60))
            .RuleFor(_ => _.THoursDaily, f => f.Random.Number(2, 6))
            .RuleFor(_ => _.DaysAvailable, f => f.Random.Number(3, 6))
            .RuleFor(_ => _.TranzitMin, f => f.Random.Number(10, 15))
            .RuleFor(_ => _.VacationDays, f => f.Random.Number(1, 5))
            .RuleFor(_ => _.Shifts, new List<Shift>())
            .RuleFor(_ => _.Deviations, new List<Deviation>())
            .FinishWith((f, e) => { Debug.WriteLine("Generated Employee {0}|{1}", e.Id, e.Name); });

        return faker.Generate(amount);
    }

    public static List<Client> GetClient(int amount)
    {
        var faker = new Faker<Client>().StrictMode(true)
            .RuleFor(_ => _.Id, f => f.IndexGlobal)
            .RuleFor(_ => _.Name, f => f.Person.FullName)
            .RuleFor(_ => _.NDayparts, f => f.Random.Number(1, 4))
            .RuleFor(_ => _.NMin, f => f.Random.Number(60 * 4, 60 * 6))
            .RuleFor(_=>_.Shifts, new List<Shift>())
            .FinishWith((f, e) => { Debug.WriteLine("Generated Client {0}|{1}", e.Id, e.Name); });

        return faker.Generate(amount);
    }

    public static Shift GetShift(int employeeId, int clientId)
    {
        var faker = new Faker<Shift>().StrictMode(true)
            .RuleFor(_ => _.Id, f => f.IndexGlobal)
            .RuleFor(_ => _.EmployeeId, f => employeeId)
            .RuleFor(_ => _.ClientId, f => clientId)
            .RuleFor(_ => _.Title, f => f.Lorem.Sentence())
            .RuleFor(_ => _.Date, f => f.Date.SoonDateOnly(30))
            .RuleFor(_ => _.StartTime, f => new TimeOnly(f.Random.Number(0, 23), f.Random.Number(0, 59)))
            .RuleFor(_ => _.EndTime, (f, s) => s.StartTime.Add(TimeSpan.FromHours(f.Random.Number(1, 6))))
            .RuleFor(_ => _.Status, f => f.PickRandom<Status>())
            .RuleFor(_ => _.CreatedAt, (f, s) => f.Date.Recent(10, s.Date.ToDateTime(TimeOnly.MinValue)))
            .RuleFor(_ => _.ModifiedAt, (f, s) => f.Date.Recent(10, s.CreatedAt))
            .RuleFor(_ => _.Deviations, new List<Deviation>())
            .RuleFor(_ => _.Client, new Client())
            .RuleFor(shift => shift.Employee, new Employee())
            .FinishWith((f, e) => { Debug.WriteLine("Generated Shift {0}|{1}", e.Id, e.Title); });

        return faker.Generate(1).First();
    }

    public static Deviation GetDeviation(Shift shift)
    {
        var faker = new Faker<Deviation>().StrictMode(true)
            .RuleFor(_ => _.Id, f => f.IndexGlobal)
            .RuleFor(_ => _.ShiftId, f => shift.Id)
            .RuleFor(_ => _.EmployeeId, f => shift.EmployeeId)
            .RuleFor(_ => _.StartTime,
                f => shift.StartTime)
            .RuleFor(_ => _.EndTime, (f, d) => d.StartTime.Add(TimeSpan.FromMinutes(f.Random.Number(1, 15))))
            .RuleFor(_ => _.DeviationType, f => f.PickRandom<DeviationType>())
            .RuleFor(_ => _.Reason, f => f.Lorem.Sentence())
            .RuleFor(_ => _.Status, f => f.PickRandom<Status>())
            .RuleFor(_ => _.CreatedAt, (f, d) => DateTime.Now)
            .RuleFor(_ => _.ModifiedAt, (f, d) =>  DateTime.Now)
            .FinishWith((f, e) => { Debug.WriteLine("Generated Shift {0}|{1}", e.Id, e.Reason); });

        return faker.Generate(1).First();
    }
}