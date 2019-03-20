using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace AmberMeet.Infrastructure.Serialization
{
    public static class EnumSerializationExtension
    {
        public static string ToEnumText<T>(this T enumValue)
        {
            var value = enumValue.ToString();
            var field = enumValue.GetType().GetField(value);
            var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false); //获取描述属性
            if (objs.Length == 0) //当描述属性没有时，直接返回名称
                return value;
            var descriptionAttribute = (DescriptionAttribute) objs[0];
            return descriptionAttribute.Description;
        }

        /// <summary>
        ///     获取枚举所有值和描述
        ///     new Enum1().GetDescriptions(); or Enum1.Attribute1.GetDescriptions();
        /// </summary>
        /// <returns></returns>
        public static IDictionary<int, string> GetDescriptions<T>(this T enumValue)
        {
            var enumType = enumValue.GetType();
            var valueDescriptions = new Dictionary<int, string>();
            var typeDescription = typeof(DescriptionAttribute);
            var fields = enumType.GetFields();
            foreach (var field in fields)
                if (field.FieldType.IsEnum)
                {
                    var strText = string.Empty;
                    var strValue = (int) enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                    var arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        var descriptionAttribute = (DescriptionAttribute) arr[0];
                        strText = descriptionAttribute.Description;
                    }
                    valueDescriptions.Add(strValue, strText);
                }
            return valueDescriptions;
        }
    }
}