using System.Reflection;

namespace Pure3DDataViewerPluginAPI.Helpers;

public static class PropertyHelper
{
    private static readonly Dictionary<Type, PropertyInfo[]> PropertyCache = [];

    public static PropertyInfo[] GetProperties(Type type)
    {
        if (!PropertyCache.TryGetValue(type, out var properties))
        {
            properties = [..type.GetProperties().OrderBy(x => x.DeclaringType == type)];
            PropertyCache.Add(type, properties);
        }
        return properties;
    }
}
