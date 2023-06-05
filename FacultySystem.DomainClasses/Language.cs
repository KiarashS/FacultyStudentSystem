using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class Language : DomainClassBase
    {
        public long Id { get; set; }
        //[Index("IX_ProfessorIdAndLanguageName", 1, IsUnique = true)]
        public int ProfessorId { get; set; }
        //[Index("IX_ProfessorIdAndLanguageName", 2, IsUnique = true)]
        public ProfessorLanguageName Name { get; set; }
        public ProfessorLanguageLevel Level { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        [ForeignKey("ProfessorId")]
        public virtual Professor ProfessorProfile { get; set; }
    }

    public enum ProfessorLanguageName: byte
    {
        [Description("--")]
        [CssClass(" ")]
        NotDefined = 1,
        [Description("فارسی")]
        [LongTitle("Farsi")]
        [ShortTitle("IR")]
        [CssClass("flag-IR")]
        Persian = 2,
        [Description("انگلیسی")]
        [LongTitle("English")]
        [ShortTitle("GB")]
        [CssClass("flag-GB")]
        English = 3,
        [Description("عربی")]
        [LongTitle("Arabic")]
        [ShortTitle("AE")]
        [CssClass("flag-AE")]
        Arabic = 4,
        [Description("ترکی")]
        [LongTitle("Turkey")]
        [ShortTitle("TR")]
        [CssClass("flag-TR")]
        Turkey = 5,
        [Description("هندی")]
        [LongTitle("Indian")]
        [ShortTitle("IN")]
        [CssClass("flag-IN")]
        India = 6,
        [Description("ژاپنی")]
        [LongTitle("Japan")]
        [ShortTitle("JP")]
        [CssClass("flag-JP")]
        Japan = 7,
        [Description("کره ای")]
        [LongTitle("Korea")]
        [ShortTitle("KR")]
        [CssClass("flag-KR")]
        Korea = 8,
        [Description("چینی")]
        [LongTitle("China")]
        [ShortTitle("CN")]
        [CssClass("flag-CN")]
        China = 9,
        [Description("آلمانی")]
        [LongTitle("Germany")]
        [ShortTitle("DE")]
        [CssClass("flag-DE")]
        Germany = 10,
        [Description("فرانسوی")]
        [LongTitle("France")]
        [ShortTitle("FR")]
        [CssClass("flag-FR")]
        France = 11,
        [Description("ایتالیایی")]
        [LongTitle("Italy")]
        [ShortTitle("IT")]
        [CssClass("flag-IT")]
        Italy = 12,
        [Description("اسپانیایی")]
        [LongTitle("Spain")]
        [ShortTitle("ES")]
        [CssClass("flag-ES")]
        Spain = 13,
        [Description("روسی")]
        [LongTitle("Russia")]
        [ShortTitle("RU")]
        [CssClass("flag-RU")]
        Russia = 14,
        [Description("لهستانی")]
        [LongTitle("Poland")]
        [ShortTitle("PL")]
        [CssClass("flag-PL")]
        Poland = 15,
        [Description("اردو")]
        [LongTitle("Pakistan")]
        [ShortTitle("PK")]
        [CssClass("flag-PK")]
        Pakistan = 16,
        [Description("ارمنی")]
        [LongTitle("Armenia")]
        [ShortTitle("AM")]
        [CssClass("flag-AM")]
        Armenia = 17,
        [Description("آذربایجانی")]
        [LongTitle("Azerbaijan")]
        [ShortTitle("AZ")]
        [CssClass("flag-AZ")]
        Azerbaijan = 18,
        [Description("هلندی")]
        [LongTitle("Netherlands")]
        [ShortTitle("NL")]
        [CssClass("flag-NL")]
        Netherlands = 19,
    }

    public enum ProfessorLanguageLevel: byte
    {
        [Description("--")]
        NotDefined = 1,
        [Description("اصلی")]
        Native = 2,
        [Description("مبتدی")]
        Beginner = 3,
        [Description("متوسط")]
        Intermediate = 4,
        [Description("خوب")]
        Good = 5,
        [Description("عالی")]
        Excellent = 6,
    }

    [AttributeUsageAttribute(AttributeTargets.All)]
    public class CssClassAttribute : Attribute
    {
        public string CssClass { get; private set; }

        public CssClassAttribute(string cssclass)
        {
            this.CssClass = cssclass;
        }
    }

    [AttributeUsageAttribute(AttributeTargets.All)]
    public class LongTitleAttribute : Attribute
    {
        public string Title { get; private set; }

        public LongTitleAttribute(string title)
        {
            this.Title = title;
        }
    }

    [AttributeUsageAttribute(AttributeTargets.All)]
    public class ShortTitleAttribute : Attribute
    {
        public string Title { get; private set; }

        public ShortTitleAttribute(string title)
        {
            this.Title = title;
        }
    }

    public static partial class DomainEnumExtensions
    {
        public static string LongTitle(this Enum enu)
        {
            Type type = enu.GetType();

            MemberInfo[] memInfo = type.GetMember(enu.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {

                object[] attrs = memInfo[0].GetCustomAttributes(typeof(LongTitleAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return ((LongTitleAttribute)attrs[0]).Title;
            }

            return enu.ToString();
        }

        public static string ShortTitle(this Enum enu)
        {
            Type type = enu.GetType();

            MemberInfo[] memInfo = type.GetMember(enu.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {

                object[] attrs = memInfo[0].GetCustomAttributes(typeof(ShortTitleAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return ((ShortTitleAttribute)attrs[0]).Title;
            }

            return enu.ToString();
        }

        public static string CssClass(this Enum enu)
        {
            Type type = enu.GetType();

            MemberInfo[] memInfo = type.GetMember(enu.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {

                object[] attrs = memInfo[0].GetCustomAttributes(typeof(CssClassAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return ((CssClassAttribute)attrs[0]).CssClass;
            }

            return enu.ToString();
        }
    }
}
