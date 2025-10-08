using System.Collections;
using System.Reflection;

namespace Pure3DDataViewerPluginAPI.Extensions;

public static class Extensions
{
    public static bool IsEnumerable(this PropertyInfo propertyInfo) => propertyInfo.PropertyType.IsEnumerable();

    public static bool IsEnumerable(this Type type)
    {
        if (type == typeof(string))
            return false;

        if (type.IsArray)
            return true;

        return typeof(IEnumerable).IsAssignableFrom(type);
    }

    public static bool IsStruct(this PropertyInfo propertyInfo)
    {
        Type propertyType = propertyInfo.PropertyType;

        return propertyType.IsValueType && !propertyType.IsEnum && !propertyType.IsPrimitive;
    }

    public static Type GetUnderlyingType(this MemberInfo member) => member.MemberType switch
        {
            MemberTypes.Event => ((EventInfo)member).EventHandlerType!,
            MemberTypes.Field => ((FieldInfo)member).FieldType,
            MemberTypes.Method => ((MethodInfo)member).ReturnType,
            MemberTypes.Property => ((PropertyInfo)member).PropertyType,
            _ => throw new ArgumentException("Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo", nameof(member)),
        };

    private static readonly HashSet<Type> NumericTypes =
        [
            typeof(byte), typeof(sbyte), typeof(short), typeof(ushort),
            typeof(int), typeof(uint), typeof(long), typeof(ulong),
            typeof(float), typeof(double), typeof(decimal)
        ];
    public static bool IsNumeric(this PropertyInfo propertyInfo) => NumericTypes.Contains(propertyInfo.PropertyType);

    public static bool HasFlagsAttribute(this Type type) => type.IsEnum && type.GetCustomAttribute<FlagsAttribute>() != null;

    public static object? GetDefault(this Type type)
    {
        if (type == typeof(string))
            return string.Empty;

        if (type == typeof(Color))
            return Color.FromArgb(255, 255, 255, 255);

        if (type.IsEnum)
        {
            var values = Enum.GetValues(type).Cast<IComparable>();
            return values.Min();
        }

        if (type.IsArray)
        {
            var elementType = type.GetElementType() ?? typeof(object);
            return Array.CreateInstance(elementType, 0);
        }

        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            if (type.IsGenericType)
            {
                var genericTypeDef = type.GetGenericTypeDefinition();

                if (genericTypeDef == typeof(IList<>) ||
                    genericTypeDef == typeof(List<>) ||
                    genericTypeDef == typeof(IEnumerable<>) ||
                    genericTypeDef == typeof(ICollection<>))
                {
                    var elementType = type.GetGenericArguments()[0];
                    var listType = typeof(List<>).MakeGenericType(elementType);
                    return Activator.CreateInstance(listType);
                }
            }

            return null;
        }  

        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    public static string ToFirstUpper(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input ?? string.Empty;

        if (input.Length == 1)
            return input.ToUpper();

        return char.ToUpper(input[0]) + input[1..];
    }

    private static readonly NullabilityInfoContext _nullabilityInfoContext = new();
    public static NullabilityInfo? GetNullabilityInfo(this MemberInfo member) =>  member switch
        {
            PropertyInfo prop => _nullabilityInfoContext.Create(prop),
            FieldInfo field => _nullabilityInfoContext.Create(field),
            EventInfo ev => _nullabilityInfoContext.Create(ev),
            _ => null
        };

    public static NullabilityInfo? GetNullabilityInfo(this ParameterInfo parameter) => _nullabilityInfoContext.Create(parameter);
}
