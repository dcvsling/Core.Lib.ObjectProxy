using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using Xunit;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Lib.Shared.Tests
{
    public class CommonTests
    {
        
        [Theory]
        [MemberData(nameof(GetTestScenarios))]
        public void Test_Property(IServiceProvider p,int times)
        {
            var expect = "test";
            var proxys = p.GetServices<ITest>();
            var writer = p.GetService<TextWriter>();
            proxys.Each(x => x.Property = expect);

            Assert.Collection(
                proxys,
                Enumerable.Repeat<Action<ITest>>(
                        t => Assert.Equal(expect, t.Property), times)
                    .ToArray());
        }

        [Theory]
        [MemberData(nameof(GetTestScenarios))]
        public void Test_NoArg_Action(IServiceProvider p, int times)
        {
            var expect = "Ok";
            var writer = p.GetService<TextWriter>();
            var proxys = p.GetServices<ITest>();

            proxys.Each(x => x.Action());

            Assert.Equal(string.Join(string.Empty, Enumerable.Repeat(expect, times)), writer.ToString());
        }

        [Theory]
        [MemberData(nameof(GetTestScenarios))]
        public void Test_Arg_Action(IServiceProvider p, int times)
        {
            var expect = "Ok";
            var writer = p.GetService<TextWriter>();
            var proxys = p.GetServices<ITest>();

            proxys.Each(x => x.Action(expect));

            Assert.Equal(string.Join(string.Empty, Enumerable.Repeat(expect, times)), writer.ToString());
        }

        [Theory]
        [MemberData(nameof(GetTestScenarios))]
        public void Test_Func(IServiceProvider p, int times)
        {
            var expect = "test";
            var proxys = p.GetServices<ITest>();
            Assert.Collection(
                proxys,
                Enumerable.Repeat<Action<ITest>>(
                        t => Assert.Equal(expect, t.Func(expect)), times)
                    .ToArray());
        }
        private static IServiceProvider CreateServiceProvider(Action<IServiceCollection> config)
        {
            var services = new ServiceCollection()
                  .AddSingleton<IConfiguration>(
                      _ => new ConfigurationBuilder()
                          .AddInMemoryCollection(new Dictionary<string, string>()
                          {
                              ["serviceproxy:NamespaceFormat"] = "{0}.Proxi",
                              ["serviceproxy:ModuleNameFormat"] = "{0}",
                              ["serviceproxy:TypeNameFormat"] = "{0}Proxy",
                              ["serviceproxy:InputFieldName"] = "_target"
                          }).Build())
                  .AddSingleton<TextWriter, StringWriter>();
            config(services);
            return services.BuildServiceProvider();
        }

        public static IEnumerable<object[]> GetTestScenarios()
        {
            yield return new object[] { OneToOne , 1 };
            yield return new object[] { OneToMany , 1 };
            yield return new object[] { ManyToOne , 3 };
            yield return new object[] { ManyToMany , 3 };
        }

        private static IServiceProvider OneToOne
            => CreateServiceProvider(services
                => services.AddSingleton<ITest, Test>()
                    .AddServiceProxy()
                        .AddService<ITest>());


        private static IServiceProvider ManyToOne
            => CreateServiceProvider(services
                => services.AddSingleton<ITest, Test>()
                    .AddSingleton<ITest, Test>()
                    .AddSingleton<ITest, Test>()
                    .AddServiceProxy()
                        .AddService<ITest>());
        private static IServiceProvider OneToMany
            => CreateServiceProvider(services
                => services.AddSingleton<ITest, Test>()
                    .AddServiceProxy()
                        .AddService<ITest>()
                        .AddService<ITest>()
                        .AddService<ITest>());

        private static IServiceProvider ManyToMany
            => CreateServiceProvider(services
                => services.AddSingleton<ITest, Test>()
                    .AddSingleton<ITest, Test>()
                    .AddSingleton<ITest, Test>()
                    .AddServiceProxy()
                        .AddService<ITest>()
                        .AddService<ITest>()
                        .AddService<ITest>());
    }
}
