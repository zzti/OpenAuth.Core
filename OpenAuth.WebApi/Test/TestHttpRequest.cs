﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Autofac;
using Infrastructure;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenAuth.App;
using OpenAuth.App.SSO;

namespace OpenAuth.WebApi.Test
{
    /// <summary>
    /// 模拟HTTP请求测试
    /// </summary>
    public class TestHttpRequest
    {
        private HttpClient _client;

        void ConfigureTestServices(IServiceCollection services)
        {
            services.AddSingleton("");
        }

        void ConfigureTestContainer(ContainerBuilder builder)
        {
            AutofacExt.InitAutofac(builder);
        }

        [SetUp]
        public void Init()
        {
            var factory = new AutofacWebApplicationFactory<Startup>();

            _client = factory
                .WithWebHostBuilder(builder => {
                    builder.ConfigureTestServices(ConfigureTestServices);
                    builder.ConfigureTestContainer<ContainerBuilder>(ConfigureTestContainer);
                })
                .CreateClient();
        }
        
        [Test]
        public void TestLogin()
        {
            var loginreq = new PassportLoginRequest
            {
                Account = "System",
                Password = "123456",
                AppKey = "openauth"
            };
            // Act
            var request = new StringContent(JsonHelper.Instance.Serialize(loginreq));
            request.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = _client.PostAsync("/api/Check/Login", request);
            
            Console.WriteLine($"返回结果:{response.Result.Content}");
        }
    }
}