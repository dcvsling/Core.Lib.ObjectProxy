using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Core.Lib.ObjectProxy.Abstractions
{
    public interface IBuilderContainer
    {
        IList<FieldBuilder> DefindedFields { get; }
        IList<PropertyBuilder> DefindedProperties { get; }
        IList<MethodBuilder> DefindedMethods { get; }
        IList<ConstructorBuilder> DefindedConstructors { get; }
    }
}