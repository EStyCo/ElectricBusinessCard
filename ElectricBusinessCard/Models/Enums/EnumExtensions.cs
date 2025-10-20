using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ElectricBusinessCard.Models.Enums
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                          .GetMember(enumValue.ToString())
                          .First()
                          .GetCustomAttribute<DisplayAttribute>()
                          ?.Name ?? enumValue.ToString();
        }
    }
}
