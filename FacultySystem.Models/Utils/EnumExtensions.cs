using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.Utils
{
    public static class EnumExtensions
    {
        public class EnumObject
        {
            public Enum ValueMember { get; set; }
            public int intValueMember
            {
                get { return int.Parse(ValueMember.ToString("D")); }
            }
            public string stringValueMember
            {
                get { return ValueMember.ToString(""); }
            }
            public string DisplayMember
            {
                get { return ValueMember.GetDescription(); }
            }
        }

        public static List<EnumObject> EnumToList<T>()
        {
            Type enumType = typeof(T);
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            List<EnumObject> li = new List<EnumObject>();
            foreach (var item in enumType.GetEnumValues())
            {
                li.Add(new EnumObject { ValueMember = (Enum)item });
            }
            return li;
        }

        public static List<EnumObject> GetEnumList(this Enum enu)
        {
            List<EnumObject> li = new List<EnumObject>();
            foreach (var item in enu.GetType().GetEnumValues())
            {
                li.Add(new EnumObject { ValueMember = (Enum)item });
            }
            return li;
        }

        public static string GetDescription(this Enum enu)
        {
            Type type = enu.GetType();

            MemberInfo[] memInfo = type.GetMember(enu.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {

                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enu.ToString();
        }
    }
}
