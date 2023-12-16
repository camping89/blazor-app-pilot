using BlazorApp.Application;
using BlazorApp.Application.Services;

namespace BlazorApp.Data;

public class GenerateService
{
    private static TestData _testData;
    public TestData GenerateTestData()
    {
        if (_testData is null)
        {
            var testData =  new TestDataGenerator().Generate();
            _testData = testData;
        }
        return _testData;
    }
}