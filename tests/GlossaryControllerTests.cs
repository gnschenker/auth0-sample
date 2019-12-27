using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using api;
using Xunit;

namespace tests
{
    public class GlossaryControllerTests : ControllerTestsBase
    {
        public GlossaryControllerTests(WebApiTesterFactory factory): base(factory)
        {}

        [Fact]
        public async Task should_return_list_of_glossary_items_without_need_for_token()
        {
            var response = await client.GetAsync("glossary");

            var data = await JsonSerializer.DeserializeAsync<IEnumerable<GlossaryItem>>(response.Content.ReadAsStreamAsync().Result);
            Assert.True(data.Count() > 0);
        }

        [Fact]
        public async Task should_return_glossary_item_without_need_for_token()
        {
            var response = await client.GetAsync("glossary/accesstoken");

            var data = await JsonSerializer.DeserializeAsync<GlossaryItem>(response.Content.ReadAsStreamAsync().Result);
            Assert.NotNull(data);
        }
        
        [Fact]
        public void should_throw_unauthorized_if_token_is_not_provided_upon_delete()
        {
            Assert.ThrowsAsync<HttpRequestException>(() => client.DeleteAsync("glossary"));
        }

        [Fact]
        public void should_throw_unauthorized_if_token_is_not_provided_upon_create()
        {
            Assert.ThrowsAsync<HttpRequestException>(() => client.PostAsync("glossary", null));
        }

        [Fact]
        public void should_throw_unauthorized_if_token_is_not_provided_upon_modify()
        {
            Assert.ThrowsAsync<HttpRequestException>(() => client.PutAsync("glossary", null));
        }

        [Fact]
        public async Task should_delete_glossary_item()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await client.DeleteAsync("glossary/openid");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task should_return_not_found_if_glossary_item_to_delete_does_not_exist()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync("glossary/jwt");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
            response = await client.DeleteAsync("glossary/jwt");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
