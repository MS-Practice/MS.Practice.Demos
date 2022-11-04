using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TypeId
{
    [JsonConverter(typeof(TypeIdJsonConverter))]
    [TypeConverter(typeof(TypeIdTypeConverter))]
    public readonly struct TypeId : IComparable<TypeId>, IEquatable<TypeId>
    {
        public TypeId(Guid value) => Value = value;
        public Guid Value { get; }
        public static TypeId New() => new TypeId(Guid.NewGuid());
        public static TypeId Empty { get; } = new TypeId(Guid.Empty);

        public int CompareTo(TypeId other) => Value.CompareTo(other);

        public bool Equals(TypeId other) => Value.Equals(other);

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is null) return false;
            return obj is TypeId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator ==(TypeId a, TypeId b) => a.CompareTo(b) == 0;
        public static bool operator !=(TypeId a, TypeId b) => !(a == b);

        // 序列化处理
        class TypeIdJsonConverter : JsonConverter<TypeId>
        {
            public override bool CanConvert(Type typeToConvert) => typeof(TypeId).IsAssignableFrom(typeToConvert);

            public override TypeId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => new(Guid.Parse(reader.GetString()!));

            public override void Write(Utf8JsonWriter writer, TypeId value, JsonSerializerOptions options) => writer.WriteStringValue(value.Value);
        }

        class TypeIdTypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
            {
                var stringValue = value as string;
                if (!string.IsNullOrEmpty(stringValue)
                    && Guid.TryParse(stringValue, out var guid))
                {
                    return new TypeId(guid);
                }

                return base.ConvertFrom(context, culture, value);
            }
        }
    }
}
