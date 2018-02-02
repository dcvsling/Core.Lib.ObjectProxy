using Core.Lib.ObjectProxy.Abstractions;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Core.Lib.ObjectProxy.Internal
{

    public class ProxyBuilderContainer : IBuilderContainer
    {
        public IList<FieldBuilder> DefindedFields { get; } = new List<FieldBuilder>();
        public IList<PropertyBuilder> DefindedProperties { get; } = new List<PropertyBuilder>();
        public IList<MethodBuilder> DefindedMethods { get; } = new List<MethodBuilder>();
        public IList<ConstructorBuilder> DefindedConstructors { get; } = new List<ConstructorBuilder>();
    }
}