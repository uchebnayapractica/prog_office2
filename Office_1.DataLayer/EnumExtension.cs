using System.ComponentModel;

namespace Office_1.DataLayer;

public static class EnumExtension
{

    public static string? GetDescription(this Enum value)
    {
        var type = value.GetType();

        var name = Enum.GetName(type, value);
        if (name == null) return null;

        var field = type.GetField(name);
        if (field == null) return null;

        if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
        {
            return attr.Description;
        }

        return null;
    }

    public static T? GetValueFromDescription<T>(string description) where T : Enum
    {
        foreach (var field in typeof(T).GetFields())
        {
            if (Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                    return (T?)field.GetValue(null);
            }
            else
            {
                if (field.Name == description)
                    return (T?)field.GetValue(null);
            }
        }

        throw new ArgumentException("Not found.", nameof(description));
    }

}