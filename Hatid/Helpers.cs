using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Hatid
{
    public class Helpers
    {
        public static List<KeyValuePair<int, string>> EnumToDataList<T>()
        {
            List<KeyValuePair<int, string>> enumList = new List<KeyValuePair<int, string>>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                int enumValue = (int)(object)value;
                string enumText = EnumDisplayNameFor(value as Enum);
                enumList.Add(new KeyValuePair<int, string>(enumValue, enumText));
            }
            return enumList;
        }

        public static string EnumDisplayNameFor(Enum item)
        {
            Type type = item.GetType();
            MemberInfo[] member = type.GetMember(item.ToString());
            DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayName != null)
            {
                return displayName.Name;
            }

            return item.ToString();
        }
    }
}
