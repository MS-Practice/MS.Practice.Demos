using System.Diagnostics.CodeAnalysis;

namespace AsyncLocalDemo
{
    public interface IValueAccessor<T>
    {
        T Value { get; set; }
    }

    public class ValueAccessor<T> : IValueAccessor<T>
    {
        private static AsyncLocal<ValueHolder<T>> _asyncLocal = new();
        public T Value
        {
            get => _asyncLocal.Value != null ? _asyncLocal.Value.Value : default; 
            set
            {
                _asyncLocal.Value ??= new ValueHolder<T>();
                _asyncLocal.Value.Value = value;
            }
        }
    }

    class ValueHolder<T>
    {
        [MaybeNull]
        public T Value { get; set; }
    }
}
