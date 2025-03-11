using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                .FirstOrDefault() as DescriptionAttribute;
            return attribute?.Description ?? value.ToString();
        }

    }
}
