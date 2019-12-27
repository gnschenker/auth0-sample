using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace tests
{
    public class ControllerTestsBase : IClassFixture<WebApiTesterFactory>
    {
        protected readonly WebApiTesterFactory factory;
        protected readonly JsonElement result;
        protected readonly string token;
        protected readonly HttpClient client;

        public ControllerTestsBase(WebApiTesterFactory factory)
        {
            this.factory = factory;
            client = factory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001/api/");

            var config = factory.Services.GetService(typeof(IConfiguration)) as IConfiguration;
            Assert.NotNull(config);

            // NOTE: we are using client secrets to avoid having secret valuse in code!
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var url = config["auth0:url"];
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var json = new {
                client_id = config["auth0:client_id"],
                client_secret = config["auth0:client_secret"],
                audience = config["auth0:audience"],
                grant_type = "client_credentials"
            };
            var content = new StringContent(
                JsonSerializer.Serialize(json, typeof(object)),
                Encoding.UTF8,
                "application/json");
            request.Content = content;
            var response = httpClient.PostAsync(url, content).Result;

            result = JsonSerializer.Deserialize<JsonElement>(response.Content.ReadAsStringAsync().Result);
            token = result.GetProperty("access_token").GetString();
        }
    }
}
