using Xunit;
using System.Threading.Tasks;
using System.Net.Http;

namespace Internal.Services.Movements.IntegrationTests
{
    public class IntegrationTest : IClassFixture<TestStartup<Program>>
    { 

        private readonly TestStartup<Program> _factory;
        private readonly Utilities.MoqHelper _moq;
        private readonly HttpClient _client;

        public IntegrationTest(TestStartup<Program> factory)
        {
            _factory = factory;
            _moq = new Utilities.MoqHelper(factory.MovementMock);
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Test1()
        {

        }
    }
}