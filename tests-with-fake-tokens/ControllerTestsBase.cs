using System;
using System.Dynamic;
using System.Net.Http;
using Xunit;

namespace tests_with_fake_tokens
{
    public class ControllerTestsBase : IClassFixture<WebApiTesterFactory>
    {
        protected readonly WebApiTesterFactory factory;
        protected HttpClient client;
        protected dynamic token;

        public ControllerTestsBase(WebApiTesterFactory factory)
        {
            this.factory = factory;
            client = factory.CreateClient();

            token = new ExpandoObject();
            token.sub = Guid.NewGuid();
            token.role = new[] { "sub_role", "admin" };
        }
    }
}