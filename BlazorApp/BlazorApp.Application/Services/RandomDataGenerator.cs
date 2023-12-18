using System.Diagnostics;
using BlazorApp.Share.Entities;
using BlazorApp.Share.Enums;
using Bogus;

namespace BlazorApp.Application.Services;

public class TestData
{
    public List<Client>    Clients    { get; set; } = new();
    public List<Employee>  Employees  { get; set; } = new();
    public List<Shift>     Shifts     { get; set; } = new();
    public List<Deviation> Deviations { get; set; } = new();
}

public class RandomDataGenerator
{
    private static int _index;

    public TestData Generate()
    {
        var clients    = GetClient(20);
        var employees  = GetEmployee(5);
        var shifts     = new List<Shift>();
        var deviations = new List<Deviation>();
        _index = clients.Select(client => client.Id).Union(employees.Select(employee => employee.Id)).Max();

        return new TestData
        {
            Clients    = clients,
            Employees  = employees,
            Shifts     = shifts,
            Deviations = deviations
        };
    }

    public static int GetId()
    {
        var faker = new Faker();
        return faker.Random.Number(_index, int.MaxValue);
    }

    public static List<Employee> GetEmployee(int amount)
    {
        var faker = new Faker<Employee>().StrictMode(true)
                                         .RuleFor(_ => _.Id,            f => f.IndexGlobal)
                                         .RuleFor(_ => _.Name,          f => f.Person.FullName)
                                         .RuleFor(_ => _.THoursWeekly,  f => f.Random.Number(10, 60))
                                         .RuleFor(_ => _.THoursDaily,   f => f.Random.Number(2,  6))
                                         .RuleFor(_ => _.DaysAvailable, f => f.Random.Number(3,  6))
                                         .RuleFor(_ => _.TranzitMin,    f => f.Random.Number(10, 15))
                                         .RuleFor(_ => _.VacationDays,  f => f.Random.Number(1,  5))
                                         .RuleFor(_ => _.Shifts,        new List<Shift>())
                                         .RuleFor(_ => _.Deviations,    new List<Deviation>())
                                         .RuleFor(_ => _.CreatedAt,     DateTime.Now)
                                         .RuleFor(_ => _.ModifiedAt,    DateTime.Now)
                                         .FinishWith((f, e) => { Debug.WriteLine("Generated Employee {0}|{1}", e.Id, e.Name); });

        return faker.Generate(amount);
    }

    public static List<Client> GetClient(int amount)
    {
        var faker = new Faker<Client>().StrictMode(true)
                                       .RuleFor(_ => _.Id,         f => f.IndexGlobal)
                                       .RuleFor(_ => _.Name,       f => f.Person.FullName)
                                       .RuleFor(_ => _.NDayparts,  f => f.Random.Number(1,      4))
                                       .RuleFor(_ => _.NMin,       f => f.Random.Number(60 * 4, 60 * 6))
                                       .RuleFor(_ => _.Shifts,     new List<Shift>())
                                       .RuleFor(_ => _.CreatedAt,  DateTime.Now)
                                       .RuleFor(_ => _.ModifiedAt, DateTime.Now)
                                       .FinishWith((f, e) => { Debug.WriteLine("Generated Client {0}|{1}", e.Id, e.Name); });

        return faker.Generate(amount);
    }

    public static Shift GetShift(int employeeId, int clientId)
    {
        var faker = new Faker<Shift>().StrictMode(true)
                                      .RuleFor(_ => _.Id,               f => f.IndexGlobal)
                                      .RuleFor(_ => _.EmployeeId,       f => employeeId)
                                      .RuleFor(_ => _.ClientId,         f => clientId)
                                      .RuleFor(_ => _.Title,            f => f.Lorem.Slug(5))
                                      .RuleFor(_ => _.Date,             f => f.Date.SoonDateOnly(14))
                                      .RuleFor(_ => _.StartTime,        f => new TimeOnly(f.Random.Number(0, 23), f.Random.Number(0, 59)))
                                      .RuleFor(_ => _.EndTime,          (f, s) => s.StartTime.Add(TimeSpan.FromHours(6)))
                                      .RuleFor(_ => _.Status,           f => f.PickRandom(ShiftStatus.Planned, ShiftStatus.Approved, ShiftStatus.Completed))
                                      .RuleFor(_ => _.CreatedAt,        (f, s) => f.Date.Recent(10, s.Date.ToDateTime(TimeOnly.MinValue)))
                                      .RuleFor(_ => _.ModifiedAt,       (f, s) => f.Date.Recent(10, s.CreatedAt))
                                      .RuleFor(_ => _.Deviations,       new List<Deviation>())
                                      .RuleFor(_ => _.Client,           new Client())
                                      .RuleFor(shift => shift.Employee, new Employee())
                                      .FinishWith((f, e) => { Debug.WriteLine("Generated Shift {0}|{1}", e.Id, e.Title); });

        return faker.Generate(1).First();
    }

    public static Deviation GetDeviation(Shift shift)
    {
        var faker = new Faker<Deviation>().StrictMode(true)
                                          .RuleFor(_ => _.Id,            f => f.IndexGlobal)
                                          .RuleFor(_ => _.ShiftId,       f => shift.Id)
                                          .RuleFor(_ => _.EmployeeId,    f => shift.EmployeeId)
                                          .RuleFor(_ => _.DeviationType, f => f.PickRandom(DeviationType.Lateness, DeviationType.EarlyLeave))
                                          .RuleFor(_ => _.StartTime,
                                                   (f, d) =>
                                                   {
                                                       return d.DeviationType switch
                                                       {
                                                           DeviationType.Lateness   => shift.StartTime,
                                                           DeviationType.EarlyLeave => shift.EndTime.Add(TimeSpan.FromMinutes(-30)),
                                                           _                        => throw new ArgumentOutOfRangeException()
                                                       };
                                                   })
                                          .RuleFor(_ => _.EndTime,
                                                   (f, d) =>
                                                   {
                                                       return d.DeviationType switch
                                                       {
                                                           DeviationType.Lateness   => shift.StartTime.Add(TimeSpan.FromMinutes(30)),
                                                           DeviationType.EarlyLeave => shift.EndTime.Add(TimeSpan.FromMinutes(-1)),
                                                           _                        => throw new ArgumentOutOfRangeException()
                                                       };
                                                   })
                                          .RuleFor(_ => _.Reason,     f => f.Lorem.Sentence())
                                          .RuleFor(_ => _.Status,     f => f.PickRandom(DeviationStatus.Pending, DeviationStatus.Approved, DeviationStatus.Rejected))
                                          .RuleFor(_ => _.CreatedAt,  (f, d) => DateTime.Now)
                                          .RuleFor(_ => _.ModifiedAt, (f, d) => DateTime.Now)
                                          .FinishWith((f,                 e) => { Debug.WriteLine("Generated Shift {0}|{1}", e.Id, e.Reason); });

        return faker.Generate(1).First();
    }
}