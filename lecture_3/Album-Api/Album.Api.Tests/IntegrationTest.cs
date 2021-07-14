using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Album.Api.Tests
{
    public class IntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        public HttpClient Client { get; }

        public IntegrationTest(WebApplicationFactory<Startup> fixture)
        {
            Client = fixture.CreateClient();
        }


        [Theory]
        [InlineData("api/Hello")]
        public async Task TestEndPoints(string endpoint)
        {
            var response = await Client.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
        }
    }
}