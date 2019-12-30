using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using api;
using Xunit;

namespace tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task should_read_glossary_items()
        {
            var factory = new WebApiTesterFactory();
            var client = factory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001/api/");

            var response = await client.GetAsync("glossary");

            var data = await JsonSerializer.DeserializeAsync<IEnumerable<GlossaryItem>>(
                response.Content.ReadAsStreamAsync().Result);
            Assert.True(data.Count() > 0);
        }
    }
}