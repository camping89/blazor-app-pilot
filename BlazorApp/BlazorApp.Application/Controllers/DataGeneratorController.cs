using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataGeneratorController : ControllerBase
    {
        [HttpGet("test-data")]
        public TestData GetRandomEmployee()
        {
            return new TestDataGenerator().Generate();
        }
    }
}