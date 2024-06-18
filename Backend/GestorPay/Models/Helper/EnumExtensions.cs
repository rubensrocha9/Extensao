using System.ComponentModel;
using System.Reflection;

namespace GestorPay.Models.Helper
{
    public static class EnumExtensions
    {
        public static Type GetEnumTypeByName(this string enumName)
        {
            var assembly = Assembly.Load("GestorPay");

            return assembly
                .GetTypes()
                .FirstOrDefault(p => p.IsEnum && p.Name.ToLower() == enumName.ToLower());
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return value.ToString();
        }
    }
}
