using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using Xunit;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Lib.Shared.Tests
{

    public class OpenGenericTests
    {
        private IServiceProvider Service =>
            new ServiceCollection()
                .AddSingleton<IConfiguration>(
                    _ => new ConfigurationBuilder()
                        .AddInMemoryCollection(new Dictionary<string, string>()
                        {
                            ["serviceproxy:NamespaceFormat"] = "{0}.Proxi",
                            ["serviceproxy:ModuleNameFormat"] = "{0}",
                            ["serviceproxy:TypeNameFormat"] = "{0}Proxy",
                            ["serviceproxy:InputFieldName"] = "_target"
                        }).Build())
                .AddSingleton<TextWriter, StringWriter>()
                .AddSingleton(typeof(IWork<>), typeof(Work<>))
                .AddSingleton(typeof(IWork<>), typeof(Work<>))
                .AddSingleton(typeof(IWork<>), typeof(Work<>))
                .AddSingleton(typeof(IDecorateWork<>),typeof(DecorateWork<>))
                .AddSingleton(typeof(IDecorateWork<>), typeof(DecorateWork<>))
                .AddSingleton(typeof(IDecorateWork<>), typeof(DecorateWork<>))
                .AddServiceProxy()
                    .AddService(typeof(IWork<>))
                    .AddService(typeof(IWork<>))
                    .AddService(typeof(IWork<>))
                    .AddService(typeof(IDecorateWork<>))
                    .AddService(typeof(IDecorateWork<>))
                    .AddService(typeof(IDecorateWork<>))
                .Services
                .BuildServiceProvider();

        [Fact]
        public void test_open_generic_type()
        {
            var expect = "Ok";
            var services = Service;
            var writer = services.GetService<TextWriter>();
            var work = services.GetServices<IWork<string>>();

            work.Each(x => x.DoWork("Ok"));

            Assert.Equal(string.Join(string.Empty,Enumerable.Repeat(expect,3)), writer.ToString());
        }

        [Fact]
        public void test_open_generic_type_decorate()
        {
            var expect = "Ok";
            var services = Service;
            var writer = services.GetService<TextWriter>();
            var work = services.GetServices<IDecorateWork<string>>();

            work.Each(x => x.DoWork("Ok"));

            Assert.Equal(string.Join(string.Empty, Enumerable.Repeat(expect, 6)), writer.ToString());
        }
    }

    public interface IWork<T>
    {
        void DoWork(T value);
    }

    public interface IDecorateWork<T> : IWork<T>
    {
    }

    public class Work : IWork<string>
    {
        private readonly TextWriter _writer;

        public Work(TextWriter writer)
        {
            _writer = writer;
        }
        public void DoWork(string value)
        {
            _writer.Write(value);
        }
    }

    public class Work<T> : IWork<T>
    {
        private readonly TextWriter _writer;

        public Work(TextWriter writer)
        {
            _writer = writer;
        }
        public void DoWork(T value)
        {
            _writer.Write(value);
        }
    }

    public class DecorateWork<T> : IDecorateWork<T>
    {
        private readonly IWork<T> _work;

        public DecorateWork(IWork<T> work)
        {
            _work = work;
        }
        public void DoWork(T value)
        {
            _work.DoWork(value);
            _work.DoWork(value);
        }
    }
}
