
using System.Collections;

namespace Core.Lib.Shared.Unit
{
    public abstract class Unit<T,TValue>
    {
        protected Unit(TValue value) => Value = value;
        protected TValue Value { get; }
        public int CompareTo(object other, IComparer comparer)
            => comparer.Compare(this.Value, other);

        public bool Equals(object other, IEqualityComparer comparer)
            => comparer.Equals(this.Value, other);

        public int GetHashCode(IEqualityComparer comparer)
            => comparer.GetHashCode(this.Value);


        public override bool Equals(object obj)
            => Value.Equals(obj);

        public override int GetHashCode()
            => Value.GetHashCode() ^ typeof(T).GetHashCode() ^ typeof(TValue).GetHashCode();
        public static bool operator ==(Unit<T, TValue> x, Unit<T, TValue> y) => x.Value.Equals(y.Value);
        public static bool operator !=(Unit<T, TValue> x, Unit<T, TValue> y) => !(x == y);
    }
}
