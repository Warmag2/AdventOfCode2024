using System.Reflection;

namespace AdventOfCode2024.Helpers;

public static class TypeHelpers
{
    public static IEnumerable<(Attribute attribute, Type implementingType)> GetTypesWithAttribute(Assembly assembly, Type attributeType)
    {
        foreach (Type type in assembly.GetTypes())
        {
            var attribute = type.GetCustomAttribute(attributeType);
            if (attribute != null)
            {
                yield return (attribute, type);
            }
        }
    }
}
