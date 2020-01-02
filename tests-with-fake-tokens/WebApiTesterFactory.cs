using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WebMotions.Fake.Authentication.JwtBearer;
using api;

namespace tests_with_fake_tokens
{
    public class WebApiTesterFactory : WebApplicationFactory<Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(".");
            builder
                .UseTestServer()
                .ConfigureTestServices(collection =>
                        {
                            collection.AddAuthentication(options =>
                            {
                                options.DefaultAuthenticateScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                                options.DefaultChallengeScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                            }).AddFakeJwtBearer();
                });
            base.ConfigureWebHost(builder);
        }
    }
}