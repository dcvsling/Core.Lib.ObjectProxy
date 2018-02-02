using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.Options;
namespace Core.Lib.ObjectProxy
{

    public class ServiceProxyOptions
    {
        public string NamespaceFormat { get; set; }
        public string ModuleNameFormat { get; set; }
        public string TypeNameFormat { get; set; }
        public string InputFieldName { get; set; }
    }
}
