using System.Linq.Expressions;
using Core.Lib.ObjectProxy.Abstractions;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using Xunit;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Reflection.Emit;

namespace Core.Lib.Shared.Tests
{

    public class ILTests
    {
        [Fact]
        public void test_NoArg_Action()
        {
            FieldInfo field = null;
            var method = IL<Func<string, string>>.Create(il =>
             {
                 il.Emit(OpCodes.Ldarg_0);
                 il.Emit(OpCodes.Ldfld, field);
                 il.Emit(OpCodes.Ldarg_1);
                 il.EmitCall(OpCodes.Callvirt, typeof(MyEcho).GetMethod("Echo"), null);
                 il.Emit(OpCodes.Ret);
             }).WithType(builder => {
                 field = builder.DefineField("Echo", typeof(MyEcho), FieldAttributes.Public);
                 var il = builder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes)
                    .GetILGenerator();
                 il.Emit(OpCodes.Ldarg_0);
                 il.Emit(OpCodes.Newobj, typeof(MyEcho).GetConstructor(Type.EmptyTypes));
                 il.Emit(OpCodes.Stfld, field);
                 il.Emit(OpCodes.Ret);
             })
             .Build();


            Assert.Equal("test", method("test"));
        }
    }

    public class MyEndEcho
    {
        public string Text = string.Empty;
        public void Echo()
        {
            Text = "Ok";
        }
    }

    public class MyEcho
    {
        public MyEndEcho Final = new MyEndEcho();
        public void Echo() => Final.Echo();
    }

    public class IL<TDelegate>
    {
        public static IL<TDelegate> Create(Action<ILGenerator> builder)
            => new IL<TDelegate>(builder);

        private readonly Action<ILGenerator> _builder;
        private Action<TypeBuilder> _typebuilder;
        public IL(Action<ILGenerator> builder)
        {
            _builder = builder;
        }

        public TDelegate Build()
        {
            var tb = CreateTypeBuilder();
            _typebuilder?.Invoke(tb);
            var argtypes = (typeof(TDelegate).Name.StartsWith("Func")
                        ? typeof(TDelegate).GenericTypeArguments.Reverse().Skip(1).Reverse()
                        : typeof(TDelegate).GenericTypeArguments);
            _builder(tb.DefineMethod(
                "testmethod",
                MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                CallingConventions.HasThis,
                typeof(TDelegate).Name.StartsWith("Func") ? typeof(TDelegate).GenericTypeArguments[0] : typeof(void),
                argtypes.ToArray())
                .GetILGenerator());
            var type = tb.CreateTypeInfo();
            var expargs = argtypes.Select(x => Expression.Parameter(x)).ToArray();
            return Expression.Lambda<TDelegate>(
                Expression.Call(
                    Expression.Constant(Activator.CreateInstance(type)),
                    type.GetMethod("testmethod"),
                    expargs),
                expargs).Compile();
        } 

        private TypeBuilder CreateTypeBuilder()
            => AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("testassembly"), AssemblyBuilderAccess.RunAndCollect)
                .DefineDynamicModule("testmodule")
                .DefineType("testclass");
                
        public IL<TDelegate> WithType(Action<TypeBuilder> builder)
        {
            _typebuilder = builder;
            return this;
        }
    }
}
