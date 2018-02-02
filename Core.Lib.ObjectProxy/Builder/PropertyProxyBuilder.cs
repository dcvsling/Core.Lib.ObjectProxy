using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System;
using Microsoft.Extensions.Options;
using Core.Lib.Sharedd.Helper;
using Core.Lib.ObjectProxy.Abstractions;
using Core.Lib.ObjectProxy.Internal;

namespace Core.Lib.ObjectProxy.Builder
{
    internal class PropertyProxyBuilder : IPropertyProxyBuilder
    {
        private readonly ServiceProxyOptions _options;
        private readonly IILBuilder _il;
        private readonly IOptionsMonitorCache<IBuilderContainer> _cache;

        public PropertyProxyBuilder(IOptions<ServiceProxyOptions> options,IILBuilder il, IOptionsMonitorCache<IBuilderContainer> cache)
        {
            _options = options.Value;
            _il = il;
            _cache = cache;
        }

        public void Create(Type serviceType, TypeBuilder builder)
            => serviceType.GetProperties()
                .Aggregate(builder, (seed,next) => CreatePropertyProxy(serviceType,seed,next));

        private PropertyBuilder CreateProperty(TypeBuilder builder,PropertyInfo property)
            => builder.DefineProperty(
                property.Name, 
                property.Attributes,
                property.PropertyType,
                property.GetIndexParameters()
                    .Select(x => x.ParameterType).ToArray());

        private void CreatePropertyProxy(Type serviceType, TypeBuilder type, PropertyInfo @old)
        {
            var container = _cache.GetOrAdd(serviceType.FullName, () => new ProxyBuilderContainer());
            var @new = CreateProperty(type, @old);
            DefineAttribute(old, @new);

            DefineGetMethod(
                serviceType, 
                type, 
                @old.GetAccessors().First(x => x.Name.StartsWith("get")), 
                @new);

            @old.GetAccessors().FirstOrDefault(x => x.Name.StartsWith("set"))
                .Condition(x => x is MethodInfo)
                .IsFalse(_ => (Action)(() => { }))
                .Or(m => () => DefineSetMethod(serviceType, type, m, @new))
                .Invoke();

            container.DefindedProperties.Add(@new);
        }

        private void DefineAttribute(PropertyInfo old, PropertyBuilder @new)
            => old.CustomAttributes.Select(
                x => new CustomAttributeBuilder(
                    x.Constructor,
                    x.ConstructorArguments.Select(y => y.Value).ToArray(),
                    x.NamedArguments.Where(y => y.IsField).Select(y => (FieldInfo)y.MemberInfo).ToArray(),
                    x.NamedArguments.Where(y => y.IsField).Select(y => y.TypedValue.Value).ToArray()))
                .ToList()
                .ForEach(@new.SetCustomAttribute);
        
        private void DefineGetMethod(Type serviceType, TypeBuilder builder, MethodInfo old, PropertyBuilder @new)
        {
            var container = _cache.GetOrAdd(serviceType.FullName, () => new ProxyBuilderContainer());
            var paramsGet = old.GetParameters().Select(x => x.ParameterType).ToArray();
            var pGet = builder.DefineMethod(
                old.Name, 
                (MethodAttributes)(old.Attributes - MethodAttributes.Abstract), 
                old.ReturnType, 
                paramsGet);
            _il.DefineIL(
                old,
                pGet.GetILGenerator(),
                il =>
                {
                    il.Emit(OpCodes.Ldfld, container.DefindedFields.First());
                    il.EmitCall(OpCodes.Callvirt, old, paramsGet);
                });
            @new.SetGetMethod(pGet);
            container.DefindedMethods.Add(pGet);
        }

        private void DefineSetMethod(Type serviceType,TypeBuilder builder,MethodInfo old, PropertyBuilder @new)
        {
            var container = _cache.GetOrAdd(serviceType.FullName, () => new ProxyBuilderContainer());
            var @paramsSet = old.GetParameters().Select(x => x.ParameterType).ToArray();
            var pSet = builder.DefineMethod(
                old.Name,
                (MethodAttributes)(old.Attributes - MethodAttributes.Abstract), 
                old.ReturnType, 
                paramsSet);
            _il.DefineIL(old,
                pSet.GetILGenerator(),
                il =>
                {
                    il.Emit(OpCodes.Ldfld, container.DefindedFields.First());
                    il.Emit(OpCodes.Ldarg_1);
                    il.EmitCall(OpCodes.Callvirt, old, paramsSet);
                });
            @new.SetSetMethod(pSet);
            container.DefindedMethods.Add(pSet);
        }
    }
}