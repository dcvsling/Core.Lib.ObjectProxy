using System;
using System.Collections;

namespace Core.Lib.Shared.Unit
{

    public abstract class ByteUnit<T> : Unit<T, byte> where T : ByteUnit<T>
    {
        protected ByteUnit(byte value) : base(value)
        {
        }

        public static T operator ^(ByteUnit<T> x, T y)
            => (T)(Activator.CreateInstance(typeof(T), (byte)(x.Value ^ y.Value)));

        public static ByteUnit<T> operator ^(ByteUnit<T> x, ByteUnit<T> y)
            => (T)(Activator.CreateInstance(typeof(T),(byte)(x.Value ^ y.Value)));
    }
}
