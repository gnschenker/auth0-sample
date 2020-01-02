using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using api;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace tests_with_fake_tokens
{
    public class GlossaryControllerTests : ControllerTestsBase
    {
        public GlossaryControllerTests(WebApiTesterFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task should_return_list_of_glossary_items_without_need_for_token()
        {
            var response = await client.GetAsync("/api/glossary");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IEnumerable<GlossaryItem>>(json);
            Assert.True(data.Count() > 0);
            Assert.Equal("AccessToken", data.First().Term);
        }

        [Fact]
        public async Task should_return_glossary_item_without_need_for_token()
        {
            var response = await client.GetAsync("/api/glossary/accesstoken");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<GlossaryItem>(json);
            Assert.NotNull(data);
            Assert.Equal("AccessToken", data.Term);
        }

        [Fact]
        public async Task should_throw_unauthorized_if_token_is_not_provided_upon_create()
        {
            var response = await client.PostAsync("/api/glossary", null);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task should_throw_unauthorized_if_token_is_not_provided_upon_update()
        {
            var response = await client.PutAsync("/api/glossary", null);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task should_throw_unauthorized_if_token_is_not_provided_upon_delete()
        {
            var response = await client.DeleteAsync("/api/glossary/jwt");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task should_create_glossary_item()
        {
            client.SetFakeBearerToken((object)token);
            var item = new GlossaryItem
            { 
                Term = "Apple", 
                Definition = "A very popular fruit"
            };
            string json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/glossary", content);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task should_return_conflict_if_create_glossary_item_with_duplicate_term()
        {
            client.SetFakeBearerToken((object)token);
            var item = new GlossaryItem
            { 
                Term = "Prune", 
                Definition = "A blue and juicy fruit"
            };
            string json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/glossary", content);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var response2 = await client.PostAsync("/api/glossary", content);
            
            response2.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task should_update_existing_glossary_item()
        {
            client.SetFakeBearerToken((object)token);
            var item = new GlossaryItem
            { 
                Term = "Pear", 
                Definition = "Another very popular fruit"
            };
            string json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/glossary", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            item.Definition += " (changed)";
            json = JsonConvert.SerializeObject(item);
            content = new StringContent(json, Encoding.UTF8, "application/json");

            var response2 = await client.PutAsync("/api/glossary", content);
            
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        }

        [Fact]
        public async Task should_respond_with_bad_request_if_updating_non_existing_glossary_item()
        {
            client.SetFakeBearerToken((object)token);
            var item = new GlossaryItem
            { 
                Term = "Apricot", 
                Definition = "Yet another very popular fruit (changed)"
            };
            string json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("/api/glossary", content);
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task should_delete_glossary_item()
        {
            client.SetFakeBearerToken((object)token);

            var response = await client.DeleteAsync("/api/glossary/openid");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task should_return_not_found_if_glossary_item_to_delete_does_not_exist()
        {
            client.SetFakeBearerToken((object)token);
            var response = await client.DeleteAsync("/api/glossary/jwt");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
            response = await client.DeleteAsync("/api/glossary/jwt");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}